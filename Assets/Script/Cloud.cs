using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雲オブジェクト
/// </summary>
public class Cloud : MonoBehaviour
{
    //--------------------Enum--------------------
    //雲のタイプ
    public enum CloudType{
        normal,    //普通に消えない雲
        onlyOneRide,    //1回乗るとチカチカしだして消える
        dissapear,    //この雲より猫が上に行くと消える
        dummy,    //最初から乗れん
    }
    CloudType type = CloudType.normal;


    //--------------------Inspector--------------------
    //猫オブジェクト
    [SerializeField] GameObject catObj;
    //最大速度
    [SerializeField] float maxSpeed = 1.5f;
    //一つ下の雲
    [SerializeField] GameObject underCloudObj;
    [SerializeField] GameObject[] birdObj;
    [SerializeField] GameObject thunder;
    [SerializeField] GameObject gameManager;
    [SerializeField] GameObject[] airplaneObj;
    [SerializeField] GameObject earthingObj;
    //雲番号
    [SerializeField]int cloudNumber;
    [SerializeField] int changeMoveSpeedScore = 175;
    [SerializeField] int changeScrollSpeedScore = 200;


    //--------------------Instance--------------------
    Rigidbody2D cloudRbody;
    Rigidbody2D catRbody;
    Cat cat;
    Earthing earthing;
    BoxCollider2D cloudCol;
    GameMaster gameMaster;
    SpriteRenderer cloudsp;
    Airplane[] airplane;


    //--------------------Property--------------------
    //--------------------Variable--------------------
    //スクロールする速さ
    float scrolSpeed = 0;
    //もとのスクロールスピードを保存
    public float originScrolSpeed;
    //雲の基本スピード
    float basicMoveSpeed;
    //雲のスピード
    float moveSpeed;
    //上に猫がいるか
    bool isCatonCloud = false;
    //雲が画面外に出て戻るときの座標
    Vector3 backFrame;
    //maincamera
    Camera cam;
    //画面内にあるか
    Rect rect = new Rect(0.05f, 0, 0.9f, 2);
    //雲の出現場所
    float appearPosX, appearPosY;
    //雲の出現位置などの初期設定が終わったか
    bool isFinishInit = false;
    //不透明にしていくか
    bool isAlpaplus = false;
    //ランダムに雲の種類選ぶ
    int randomType;
    //猫が通り過ぎたか（グレー雲で使う）
    bool isNextCloud = false;
    //1回乗ったか（チカチカ雲で使う）
    bool isOnceRide=false;
    //タイプが決まってあるか
    bool isDecidedType = false;
    //雲の最初のx座標の範囲
    float cloudStartPosXRange = 2.0f;


    void OnCollisionEnter2D(Collision2D collision)
    {
        cloudRbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        if(collision.gameObject.name == "cat"){
            if(earthing.isGround){
                isCatonCloud = true;
            }
        }
    }


    /// <summary>
    /// インスタンスの作成
    /// </summary>
    public void MakeInstance()
    {
        cloudCol = GetComponent<BoxCollider2D>();
        catRbody = catObj.GetComponent<Rigidbody2D>();
        cloudRbody = this.GetComponent<Rigidbody2D>();
        gameMaster = gameManager.GetComponent<GameMaster>();
        cloudsp = GetComponent<SpriteRenderer>();
        airplane = new Airplane[airplaneObj.Length];
        for(int i=0; i<airplane.Length; i++)
        {
            airplane[i] = airplaneObj[i].GetComponent<Airplane>();
        }
        cat = catObj.GetComponent<Cat>();
        earthing = earthingObj.GetComponent<Earthing>();
    }


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        cloudRbody.constraints = RigidbodyConstraints2D.FreezePositionY;
        cloudRbody.gravityScale = 0;
        //速さを乱数で決める
        moveSpeed = Random.Range(maxSpeed, -maxSpeed);
        cam = Camera.main;
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        cloudsp.color = new Color(cloudsp.color.b, cloudsp.color.g, cloudsp.color.r, 1);
        cloudsp.color = Color.white;
        isAlpaplus = false;
        isNextCloud = false;
        isOnceRide = false;
        if(isDecidedType==false){
            DecideType();
        }
        if(isFinishInit==false)
        {
            appearPosX = Random.Range(-cloudStartPosXRange, cloudStartPosXRange);
            isFinishInit = true;
            if(cloudNumber < 10)
            {
                appearPosY = -4.0f + cloudNumber*1.5f;
            }
            else
            {
                appearPosY = -3.25f + (cloudNumber%10)*1.5f;
            }
            this.transform.position = new Vector3(appearPosX, appearPosY, this.transform.position.z);
        }
    }

    
    /// <summary>
    /// ゲームオーバー後のフラグ初期化
    /// </summary>
    public void InitGameOver()
    {
        isDecidedType = false;
        isFinishInit = false;
    }

    
    //雲のタイプを決める
    void DecideType()
    {
        isDecidedType = true;
        randomType = Random.Range(0, 15);
        if(Score.score == 0)
        {
            randomType = Random.Range(0, 20);
        }
        switch(randomType){
            case 0: type = Cloud.CloudType.onlyOneRide;
                break;
            case 1: type = Cloud.CloudType.dissapear;
                break;
            case 2: 
                type = Cloud.CloudType.dummy;
                CheckDummy();
                break;
                        
            case 3: 
                if(Score.score >= 75){
                    type = Cloud.CloudType.dissapear;
                }
                else{
                    type = Cloud.CloudType.normal;
                }
                break;
            case 4:
                if(Score.score >= 125){
                    type = Cloud.CloudType.onlyOneRide;
                }
                else{
                    type = Cloud.CloudType.normal;
                }
                break;
            case 5:
                if(Score.score >= 200){
                    type = Cloud.CloudType.dummy;
                    CheckDummy();
                }
                else{
                    type = Cloud.CloudType.normal;
                }
                break;
            default: type = Cloud.CloudType.normal;
                break;
        }
    }


    //ダミータイプの雲が連続してないかチェックする
    public void CheckDummy()
    {
        if(type == CloudType.dummy)
        {
            Cloud underCloudObjCloud = underCloudObj.GetComponent<Cloud>();
            if(underCloudObjCloud.type == CloudType.dummy)
            {
                type = CloudType.normal;
            }

            //最初の雲は透明でない
            if(cloudNumber==0 && Score.score == 0){
                type = CloudType.normal;
            }
        }
    }


    //猫が邪魔オブジェクトにぶつかったらすり抜ける
    public void ThroughCloud()
    {
        if(GameMaster.isCatHitEnemy){    //猫と鳥がぶつかったとき
            cloudCol.isTrigger = true;
        }
    }


    /// <summary>
    /// 雲タイプごとの処理
    /// </summary>
    public void CloudTypeProcess()
    {
        if(type != CloudType.dummy)
        {
            //跳んでるときは下からすり抜けられるようにする
            if(catRbody.velocity.y > 0){
                cloudCol.isTrigger = true;
            }
            else{
                cloudCol.isTrigger = false;
            }
        }

        //ノーマル雲
        if(type == CloudType.normal)
        {}
        //チカチカしていて1度乗ると消える雲
        else if(type == CloudType.onlyOneRide)
        {
            //チカチカ
            float a = cloudsp.color.a;
            if(isAlpaplus==false){  //透明にしていく
                a -= 0.02f;
                cloudsp.color = new Color(0.94f, 0.78f, 0.63f, a);
                if(a <= 0.3){
                    isAlpaplus = true;
                }
            }
            else if(isAlpaplus){    //不透明にしていく
                a += 0.02f;
                cloudsp.color = new Color(0.94f, 0.78f, 0.63f, a);
                if(a >= 1){
                    isAlpaplus = false;
                }
            }

            //猫が乗った処理
            if(this.transform.parent!=null)
            {
                isOnceRide = true;
            }
            //猫が離れた
            if(isOnceRide==true && this.transform.parent==null)
            {
                cloudCol.isTrigger = true;
                cloudsp.color = new Color(cloudsp.color.b, cloudsp.color.g, cloudsp.color.r, 0.1f);
            }
        }
        //猫が上のほうに行くと消える雲
        else if(type == CloudType.dissapear)
        {
            cloudsp.color = Color.gray;
            if(catObj.transform.position.y >= this.transform.position.y+2.5f || isNextCloud == true){    //猫が次の雲より高い場所
                cloudCol.isTrigger = true;
                cloudsp.color = new Color(cloudsp.color.b, cloudsp.color.g, cloudsp.color.r, 0.1f);
                isNextCloud = true;
            }
        }
        //最初から乗れない雲
        else if(type == CloudType.dummy)
        {
            cloudCol.isTrigger = true;
            cloudsp.color = new Color(cloudsp.color.b, cloudsp.color.g, cloudsp.color.r, 0.25f);
        }
    }


    /// <summary>
    /// 猫が雲の上にいるとき、雲を猫の子にする
    /// </summary>
    public void CloudParent()
    {
        //猫がジャンプ中
        if(!earthing.isGround)
        {
            isCatonCloud = false;
            this.transform.parent = null;
        }
        if(isCatonCloud)
        {
            this.transform.parent = catObj.transform;
        }
    }


    /// <summary>
    /// スコアによって最高速度が変わる
    /// </summary>
    public void ChangeMoveSpeed()
    {
        if(Score.score >= changeMoveSpeedScore){
            maxSpeed = 1.8f;
        }
    }


    /// <summary>
    /// 雲の移動処理
    /// </summary>
    public void MoveCloud()
    {
        //猫が乗ってない時
        if(!isCatonCloud)
        {
            this.transform.Translate(new Vector3(moveSpeed/50.0f,scrolSpeed, 0));
        }
        //乗ってるとき
        else
        {
            catObj.transform.Translate(new Vector3(0, scrolSpeed, 0));
            this.transform.Translate(new Vector3(0, 0, 0));
        }
    }


    /// <summary>
    /// 端まで行ったら折り返す
    /// </summary>
    public void OutFrame()
    {
        var viewportPos = cam.WorldToViewportPoint(this.transform.position);
        if(!rect.Contains(viewportPos))
        {
            moveSpeed = -moveSpeed;
            if(this.transform.position.x >= 0)
            {
                float posX = this.transform.position.x + -0.01f;
                this.transform.position = new Vector3(posX, this.transform.position.y, this.transform.position.z);
            }
            else
            {
                float posX = this.transform.position.x + 0.01f;
                this.transform.position = new Vector3(posX, this.transform.position.y, this.transform.position.z);
            }
        }
    }


    /// <summary>
    /// 猫が上のほうに行くとスクロールスピードが変わる
    /// </summary>
    public void changeScrolSpeed()
    {
        if(Score.score <= 0){
            scrolSpeed = 0;
        }
        else if(catObj.transform.position.y <= -1.5){
            scrolSpeed = originScrolSpeed;
        }
        else if(catObj.transform.position.y > 0 && scrolSpeed >= -0.025f){
            scrolSpeed += -0.00005f;
        }
        else if(catObj.transform.position.y <= 0 && scrolSpeed <= originScrolSpeed){
            scrolSpeed += 0.00005f;
        }

        if(Score.score >= changeScrollSpeedScore){
            originScrolSpeed = -0.015f;
        }
    }


    /// <summary>
    /// 雲が下まで行ったとき、一番上の雲の上に戻る
    /// </summary>
    public void CloudBackToScreenOver()
    {
        //雲が下に行った
        if(this.transform.position.y <= -5.3f)
        {
            //フラグなどを元に戻す
            cloudCol.isTrigger = false;
            cloudsp.color = new Color(cloudsp.color.b, cloudsp.color.g, cloudsp.color.r, 1);    //色をもとに戻す
            cloudsp.color = Color.white;
            isNextCloud = false;
            isOnceRide = false;
            //画面下から上へ
            float posX = Random.Range(-cloudStartPosXRange, cloudStartPosXRange);
            float posY;
            posY = underCloudObj.transform.position.y + 1.5f/2.0f;
            this.transform.position = new Vector3(posX, posY, this.transform.position.z);
            moveSpeed = Random.Range(maxSpeed, -maxSpeed);
            //雲のタイプを決める
            DecideType();
        }
    }


    //雲と雲の間隔を一定に保つ処理
    public void KeepPosition()
    {
        if(this.transform.position.y > underCloudObj.transform.position.y)
        {
            if(this.transform.position.y - underCloudObj.transform.position.y <= 1.5f/2.0f - 0.1f)
            {    //雲と雲の差が小さい
                float y = underCloudObj.transform.position.y + 1.5f/2.0f;
                this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
            }
        }
    }
}
