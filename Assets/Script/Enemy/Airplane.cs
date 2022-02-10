using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 飛行機オブジェクトクラス
/// </summary>
public class Airplane : MonoBehaviour
{
    //--------------------Enum--------------------
    //飛行機の状態
    public enum AirplaneState{
        move,
        stop
    }
    AirplaneState airplaneState = AirplaneState.stop;


    //--------------------Inspector--------------------
    [SerializeField, Header("飛行機の移動速度")] float speed;
    [SerializeField, Header("飛行機開始スコア")] int startScore;
    [SerializeField, Header("飛行機の最大間隔")] float maxinterval;


    //--------------------Instance--------------------
    BoxCollider2D[] airplaneCol;
    //飛行機SE
    AudioSource audioSource;


    //--------------------Property--------------------
    //--------------------Variable--------------------
    //開始位置
    Vector3 startPos;
    //飛行機の間隔
    float interval;
    //コルーチンが動いているか
    bool runCoroutine = false;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        //飛行機の初期値
        startPos = this.transform.position;
    }


    /// <summary>
    /// インスタンスの作成
    /// </summary>
    public void MakeInstance()
    {
        airplaneCol = this.GetComponents<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
    }


    /// <summary>
    /// 飛行機を動かす
    /// </summary>
    public void MoveAirplane()
    {
        if(airplaneState == AirplaneState.move){ 
            this.transform.Translate(speed/50, 0, 0);
        }
        //画面を通り過ぎたら戻る
        if(this.transform.position.x >= 8.0f){ 
            Init();
        }
    }


    /// <summary>
    /// 初期化 (通り過ぎた時とゲームオーバー時)
    /// </summary>
    public void Init()
    {
        //位置の初期化
        this.transform.position = startPos;
        //コライダーの初期化
        for(int i=0; i<airplaneCol.Length; i++){
            airplaneCol[i].isTrigger = false;
        }
        airplaneState = AirplaneState.stop;
        audioSource.Stop();
        runCoroutine = false;
    }


    /// <summary>
    /// 飛行機待機用コルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAirplane()
    {
        runCoroutine = true;

        interval = Random.Range(0, maxinterval);
        yield return new WaitForSeconds(interval);
        airplaneState = AirplaneState.move;
        audioSource.Play();

        runCoroutine = false;
    }


    /// <summary>
    /// コルーチンの実行
    /// </summary>
    public void MoveCoroutine()
    {
        if(runCoroutine == false && Score.score >= startScore && airplaneState == AirplaneState.stop)
        {
            StartCoroutine("WaitAirplane");
        }
    }


    /// <summary>
    /// 猫とぶつかったときの処理
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "cat")
        {
            GameMaster.isCatHitEnemy = true;
            //飛行機がすりぬけるようになる
            for(int i=0; i<airplaneCol.Length; i++){
                airplaneCol[i].isTrigger = true;
            }
        }
    }
}
