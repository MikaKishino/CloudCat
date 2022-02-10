using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //跳んでない時の猫のイラスト
    [SerializeField ] public Sprite catNormalIllust;
    //ジャンプ中の猫のイラスト
    [SerializeField] Sprite catJumpIllust;
    [SerializeField] Sprite catClashIllust;
    [SerializeField] Sprite catBurn1, catBurn2, catBurn3;
    //猫のジャンプ力
    [SerializeField] float jumpPower = 10;
    //ジャンプ間隔
    [SerializeField] float jumpTime = 5;
    //基本の移動速度
    [SerializeField] float basicMoveSpeed = 2;


    //--------------------Instance--------------------
    BoxCollider2D[] catCol;
    SpriteRenderer catsp;
    Earthing earthing;
    Rigidbody2D catRbody;
    AudioSource audioSource;


    //--------------------Property--------------------
    //--------------------Variable--------------------
    //猫の状態ごとのコライダーのオフセットとサイズ
    Vector2[] normalCatRightOffSet = new Vector2[2];
    Vector2[] normalCatRightSize = new Vector2[2];
    Vector2[] normalCatLeftOffSet = new Vector2[2];
    Vector2[] normalCatLeftSize = new Vector2[2];
    Vector2[] jumpCatRightOffSet = new Vector2[2];
    Vector2[] jumpCatRightSize = new Vector2[2];
    Vector2[] jumpCatLeftOffSet = new Vector2[2];
    Vector2[] jumpCatLeftSize = new Vector2[2];
    //横移動速度
    float moveSpeed = 0;
    //ジャンプコルーチン
    Coroutine jumpCoroutine;
    //maincamera
    Camera cam;
    //画面タッチ位置
    Vector3 touchPosition, screenPosition;
    //猫の開始位置
    Vector3 catStartPos;
    //ジャンプ元雲
    GameObject jumpStart;
    //画面内外の判定
    Rect rect = new Rect(0, 0, 1, 1);


    //
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(LayerMask.LayerToName(collision.gameObject.layer) == "airplane" || 
        LayerMask.LayerToName(collision.gameObject.layer) == "bird")
        {
            catRbody.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);    //ちょっと跳ねる
        }

        //雷に当たった時
        if(LayerMask.LayerToName(collision.gameObject.layer) == "thunder")
        {
            StartCoroutine("CatBurnAnimation");
            catRbody.AddForce(new Vector2(0, 2), ForceMode2D.Impulse);    //ちょっと跳ねる
        }
    }


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        //後でearthingに変える
        earthing.action = MoveCoroutine;
        cam = Camera.main;
        catStartPos = this.transform.position;

        //猫のコライダーのオフセット、サイズ
        for(int i=0; i<=1; i++){
            normalCatRightOffSet[i] = catCol[i].offset;
            normalCatRightSize[i] = catCol[i].size;
        }
        jumpCatRightOffSet[0] = new Vector2(-0.2893643f, -0.6430984f);
        jumpCatRightOffSet[1] = new Vector2(0.2569892f, 0.6175704f);
        jumpCatRightSize[0] = new Vector2(0.5072355f, 1.3133290f);
        jumpCatRightSize[1] = new Vector2(1.198889f, 1.127917f);
        jumpCatLeftOffSet[0] = new Vector2(0.2876863f, -0.6853214f);
        jumpCatLeftOffSet[1] = new Vector2(-0.2778635f, 0.6386817f);
        jumpCatLeftSize[0] = new Vector2(0.4790869f, 1.228883f);
        jumpCatLeftSize[1] = new Vector2(1.255237f, 1.282735f);
        normalCatLeftOffSet[0] = new Vector2(-0.06812263f, -0.6853215f);
        normalCatLeftOffSet[1] = new Vector2(-0.2980036f, 0.5446943f);
        normalCatLeftSize[0] = new Vector2(1.029583f, 1.228883f);
        normalCatLeftSize[1] = new Vector2(1.295517f, 1.09476f);
    }

    /// <summary>
    /// インスタンス作成
    /// </summary>
    public void MakeInstance()
    {
        catRbody = GetComponent<Rigidbody2D>();
        catRbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        catsp = GetComponent<SpriteRenderer>();
        catCol = GetComponents<BoxCollider2D>();
        //後でearthingに変える
        earthing = this.transform.GetChild(0).gameObject.GetComponent<Earthing>();
        audioSource = GetComponent<AudioSource>();
    }

    
    /// <summary>
    /// スタート時初期化
    /// </summary>
    public void Init()
    {
        catsp.sprite = catNormalIllust;
        this.transform.position = catStartPos;
        catsp.flipX = false;
    }

    
    /// <summary>
    /// 左右に操作
    /// </summary>
    public void OperateCat()
    {
        moveSpeed = 0;
        //エディター
        if(Application.isEditor)
        {
            if(Input.GetKey("left"))
            {
                moveSpeed = -basicMoveSpeed;
            }
            if(Input.GetKey("right"))
            {
                moveSpeed = basicMoveSpeed;
            }
        }
        //実機
        else
        {
            //タッチされてる
            if(Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPosition.x = touch.position.x;
                touchPosition.y = touch.position.y;
                touchPosition.z = 1;
            }
            //タップされてない
            else
            {
                moveSpeed = 0;
            }

            screenPosition = cam.ScreenToWorldPoint(touchPosition);
            
            //猫より左側をタッチ
            if(screenPosition.x < this.transform.position.x - 0.15f)
            {
                moveSpeed = -basicMoveSpeed;
            }
            //右側
            else if(screenPosition.x > this.transform.position.x + 0.15f)
            {
                moveSpeed = basicMoveSpeed;
            }
        }
        transform.Translate(new Vector3(moveSpeed/50.0f, 0));
    }

    
    /// <summary>
    /// 猫の画像変更
    /// </summary>
    public void IllustController()
    {
        if(moveSpeed < 0){
            catsp.flipX = true;
        }
        else if(moveSpeed > 0){
            catsp.flipX = false;
        }

        //ジャンプ中
        if(!earthing.isGround)
        {
            catsp.sprite = catJumpIllust;
        }
        else
        {
            catsp.sprite = catNormalIllust;
        }

        if(GameMaster.isCatHitEnemy)
        {
            catsp.sprite = catClashIllust;
            //ちょっと跳ねる
            //catRbody.AddForce(new Vector2(0, 1), ForceMode2D.Impulse);
        }
    }

    
    /// <summary>
    /// 端から反対の端へ
    /// </summary>
    public void SideToSide()
    {
        float posX;
        var viewportPos = cam.WorldToViewportPoint(this.transform.position);
        posX = this.transform.position.x;
        //画面外にいる時
        if(!rect.Contains(viewportPos))
        {
            if(this.transform.position.x <= 0)
            {
                posX = posX * -1 - 0.01f;
            }
            else
            {
                posX = posX * -1 + 0.01f;
            }
            this.transform.position = new Vector3(posX, transform.position.y, transform.position.z);
        }
    }


    /// <summary>
    /// ジャンプコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpCoroutine()
    {
        //ゲーム状態がプレイになるまで繰り返し待機
        while(GameMaster.nowState != GameMaster.State.play){
            yield return new WaitForSeconds(0.5f);
        }
        WaitForSeconds waitForSeconds = new WaitForSeconds(jumpTime);
        yield return waitForSeconds;
        //接地しているとき
        if(earthing.isGround == true)
        {
            //雲と接地している
            if(this.transform.childCount > 1)
            {
                //ジャンプ元の雲取得
                jumpStart = this.transform.GetChild(1).gameObject;
            }
            //ジャンプ
            catRbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
        jumpCoroutine = null;
    }

    
    /// <summary>
    /// コルーチンの実行
    /// </summary>
    void MoveCoroutine()
    {
        // 着地したタイミングで着地音鳴らす
        audioSource.Play();
        //コルーチンが動いてる時
        if(jumpCoroutine != null)
        {
            StopCoroutine(jumpCoroutine);
        }
        jumpCoroutine = StartCoroutine(JumpCoroutine());
    }


    /// <summary>
    /// ジャンプの高さ上限
    /// </summary>
    public void LimitJumpHight()
    {
        //スコア1以上かつジャンプ中
        if(!earthing.isGround && Score.score >= 1)
        {
            if(this.transform.position.y >= jumpStart.transform.position.y+1.5f)
            {
                catRbody.AddForce(new Vector3(0, -3.0f, 0));
            }
            else if(this.transform.position.y >= jumpStart.transform.position.y+1.0f)
            {
                catRbody.AddForce(new Vector3(0, -2.0f, 0));
            }
        }
    }

    //猫の画像に合わせてコライダー変更
    public void ColliderColtroller()
    {
        if(catsp.sprite == catJumpIllust && catsp.flipX == false){    //右向きにジャンプ
            for(int i=0; i<=1; i++){
                catCol[i].offset = jumpCatRightOffSet[i];
                catCol[i].size = jumpCatRightSize[i];
            }
        }
        else if(catsp.sprite == catJumpIllust && catsp.flipX){    //左向きにジャンプ
            for(int i=0; i<=1; i++){
                catCol[i].offset = jumpCatLeftOffSet[i];
                catCol[i].size = jumpCatLeftSize[i];
            }
        }
        else if(catsp.sprite == catNormalIllust && catsp.flipX == false){
            for(int i=0; i<=1; i++){
                catCol[i].offset = normalCatRightOffSet[i];
                catCol[i].size = normalCatRightSize[i];
            }
        }
        else if(catsp.sprite == catNormalIllust && catsp.flipX){
            for(int i=0; i<=1; i++){
                catCol[i].offset = normalCatLeftOffSet[i];
                catCol[i].size = normalCatLeftSize[i];
            }
        }
    }


    /// <summary>
    /// 猫が燃えるアニメーション
    /// </summary>
    /// <returns></returns>
    IEnumerator CatBurnAnimation(){
        for(int i=0; i<=5; i++){
            catsp.sprite = catBurn1;
            yield return new WaitForSeconds(0.05f);
            catsp.sprite = catBurn2;
            yield return new WaitForSeconds(0.05f);
        }
        catsp.sprite = catBurn3;
    }
}
