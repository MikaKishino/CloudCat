using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 鳥オブジェクトクラス
/// </summary>
public class Bird : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    [SerializeField] GameObject gameManager;
    //鳥が出てくる最大間隔
    [SerializeField] float maxInterval;
    //鳥が出始めるスコア
    [SerializeField] int startScore;
    //曇り空オブジェクト
    [SerializeField] public int oppositionScore = 400;
    //鳥がふわふわする円の半径
    [SerializeField] float radius = 0.2f;


    //--------------------Instance--------------------
    SpriteRenderer birdSprite;
    AudioSource gameManagerAudioSource;
    AudioSource birdAudioSource;


    //--------------------Property--------------------
    //鳥の出現位置
    public float startPositionY{get; private set;}
    //反対
    public bool isOpposition{get; private set;} = false;
    //鳥が動いているか
    public bool isMove{get; private set;} = false;


    //--------------------Variable--------------------
    //maincamera
    bool runCoroutine = false;
    //鳥が出てくる間隔
    float interval;
    //元の位置
    float startPositionX = 7.0f;
    //出てくる方向（右左）
    int leftOrRight;
    //鳥のスタート位置の範囲
    float startPosYRange = 3.5f;


    /// <summary>
    /// インスタンスの作成
    /// </summary>
    public void MakeInstance()
    {
        gameManagerAudioSource = gameManager.GetComponent<AudioSource>();
        birdAudioSource = this.GetComponent<AudioSource>();
        birdSprite = this.GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        SetStartPosition();
        isMove = false;
        birdAudioSource.Stop();
    }


    /// <summary>
    /// 鳥のスタート位置を決める
    /// </summary>
    void SetStartPosition()
    {
        startPositionY = Random.Range(-startPosYRange, startPosYRange);
        //基本左から出てくる
        //右からも出てくるか
        if(Score.score >= oppositionScore)
        {
            leftOrRight = Random.Range(0, 3);
            //右から出てくる
            if(leftOrRight == 0)
            {
                this.transform.position = new Vector3(startPositionX, startPositionY, this.transform.position.z);
                isOpposition = true;
                birdSprite.flipX = true;
            }
            //左から
            else
            {
                this.transform.position = new Vector3(-startPositionX, startPositionY, this.transform.position.z);
                isOpposition = false;
                birdSprite.flipX = false;
            }
        }
        //左側のみ
        else
        {
            this.transform.position = new Vector3(-startPositionX, startPositionY, this.transform.position.z);
            isOpposition = false;
            birdSprite.flipX = false;
        }
    }


    //ふわふわ動かす
    public void fuwafuwa()
    {
        if(isMove)
        {
            float phase = Time.time * 2 * Mathf.PI;
            float yPos = radius * Mathf.Sin(phase);
            yPos = yPos + startPositionY;
            if(isOpposition==false){
                this.transform.position = new Vector3(this.transform.position.x+0.02f, yPos, this.transform.position.z);
            }
            else if(isOpposition){
                this.transform.position = new Vector3(this.transform.position.x-0.02f, yPos, this.transform.position.z);   
            }
        }
    }


    /// <summary>
    /// 鳥待機のコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBirdCoroutine()
    {
        runCoroutine = true;
        //スコアが開始スコアより低い
        if(Score.score < startScore){
            isMove = false;
        }
        //雷が出るときは出てこない
        //鳥が動いてない時
        if(GameMaster.bgState != GameMaster.BackgroundState.gray && !isMove)
        {
            //鳥が出てくる間隔
            interval = Random.Range(0, maxInterval);
            yield return new WaitForSeconds(interval);
            //待機中にゲームオーバーになってないか確認
            if(GameMaster.nowState != GameMaster.State.play || Score.score <= startScore){
                runCoroutine = false;
                yield break;
            }
            isMove = true;
        }

        runCoroutine = false;
    }

    
    //コルーチンの実行
    public void MoveCoroutine()
    {
        if(!runCoroutine)
        {
            StartCoroutine("WaitBirdCoroutine");
        }
    }


    /// <summary>
    /// 通り過ぎたら最初の位置に戻る
    /// </summary>
    public void BackStartPosition()
    {
        //鳥が通り過ぎた
        if((!isOpposition && this.transform.position.x>=3.3f) || 
            (isOpposition && this.transform.position.x <= -3.3f))
        {
            Init();
        }
    }


    //猫に当たった時
    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.name == "cat"){    //猫にぶつかる
            GameMaster.isCatHitEnemy = true;
            gameManagerAudioSource.Play();
        }
    }
}
