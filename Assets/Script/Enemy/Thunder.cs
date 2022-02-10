using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 雷オブジェクトクラス
/// </summary>
public class Thunder : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //雷ゾーンに入るまでの最大間隔
    [SerializeField] float maxInterval;
    //雷の落下速度
    [SerializeField] float speed;
    //雷開始のスコア
    [SerializeField] int startScore;
    //もうひとつの雷オブジェクト
    [SerializeField] GameObject thunderObj;
    //雷が二つ落ち始めるスコア
    [SerializeField] public int twoThunderScore;
    [SerializeField] GameObject gameManager;


    //--------------------Instance--------------------
    AudioSource gameManagerAudioSource;


    //--------------------Property--------------------
    //雷を動かすか
    public bool isMove{get; private set;}
    //雷の出現位置
    public float startPosX1{get; private set;}
    public float startPosX2{get; private set;}


    //--------------------Variable--------------------
    //雷モードに入るまでの間隔
    float interval;
    //コルーチン実行中か
    bool runCoroutine;
    float startPosY = 15.0f;
    //雷が出現する範囲
    float startPosRange = 2.3f;


    /// <summary>
    /// インスタンスの作成
    /// </summary>
    public void MakeInstance()
    {
        gameManagerAudioSource = gameManager.GetComponent<AudioSource>();
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        isMove = false;
        SetThunderPosition();
    }


    /// <summary>
    /// 下まで落ちた雷が元の位置に戻る
    /// </summary>
    public void BackStartPosition()
    {
        if(this.transform.position.y <= -6.0f && isMove)
        {
            isMove = false;
            SetThunderPosition();
        }
    }
    

    /// <summary>
    /// 雷を動かす
    /// </summary>
    public void MoveThunder()
    {
        if(isMove)
        {
            transform.Translate(new Vector3(0, speed/50, 0));
            //雷２つのとき
            if(Score.score >= twoThunderScore){
                thunderObj.transform.Translate(new Vector3(0, speed/50, 0));
            }
        }
    }


    /// <summary>
    /// 背景grayモードまで待機し、grayモードになったら雷が動くまで待機するコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitThunderCoroutine()
    {
        runCoroutine = true;
        yield return new WaitForSeconds(maxInterval);
        //雷が出始める点数を超えてる
        if(Score.score >= startScore)
        {
            //青空のとき、interval時間経過後グレイモードに
            if(GameMaster.bgState == GameMaster.BackgroundState.blue)
            {
                //雷ゾーンに入るまでの間隔
                interval = Random.Range(1.0f, maxInterval);
                yield return new WaitForSeconds(interval);
                //待ってる間にゲームオーバーになってないか確認
                if(GameMaster.nowState != GameMaster.State.play || Score.score <= startScore)
                {
                    runCoroutine = false;
                    yield break;
                }
                //グレー背景が出始める
                GameMaster.bgState = GameMaster.BackgroundState.bluetoGray;
            }
            //グレー背景モードのとき
            else if(GameMaster.bgState == GameMaster.BackgroundState.gray)
            {
                //雷が落ちる間隔
                interval = Random.Range(1.0f, 5.0f);
                yield return new WaitForSeconds(interval);
                //待ってる間にゲームオーバーになってないか確認
                if(GameMaster.nowState != GameMaster.State.play || Score.score <= startScore)
                {
                    runCoroutine = false;
                    yield break;
                }
                //雷が動く
                isMove = true;
            }
        }


        runCoroutine = false;
    }

    
    //コルーチンを動かす
    public void MoveCoroutine()
    {
        if(runCoroutine == false)
        {
            StartCoroutine("WaitThunderCoroutine");
        }
    }


    //雷の位置のセット
    void SetThunderPosition()
    {
        startPosX1 = Random.Range(-startPosRange, startPosRange);
        startPosX2 = Random.Range(-startPosRange, startPosRange);
        this.transform.position = new Vector3(startPosX1, startPosY, this.transform.position.z);
        thunderObj.transform.position = new Vector3(startPosX2, startPosY, thunderObj.transform.position.z);
    }


    /// <summary>
    /// 猫に当たった時の処理
    /// </summary>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "cat")
        {
            GameMaster.isCatHitEnemy = true;
            //失敗音再生
            gameManagerAudioSource.Play();
        }
    }
}
