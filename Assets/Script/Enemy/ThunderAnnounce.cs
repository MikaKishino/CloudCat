using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// thunderAnnouceオブジェクトクラス
/// </summary>
public class ThunderAnnounce : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //雷
    [SerializeField] GameObject thunderObj1, thunderObj2;
    //雷アナウンス
    [SerializeField] GameObject thunderAnnounce2;


    //--------------------Instance--------------------
    Thunder thunder;
    SpriteRenderer thunderAnnouncesp1, thunderAnnouncesp2;


    //--------------------Property--------------------
    //--------------------Variable--------------------
    Camera cam;
    //画面内にあるかの確認
    Rect rect = new Rect(0, 0, 1, 1);
    Color color1, color2;
    //コルーチンが動いているか
    bool runCoroutine = false;
    //アナウンスを表示中の座標と表示中じゃない時の座標
    Vector3 OnPos = new Vector3(0, 4.5f, -1);
    Vector3 OffPos = new Vector3(0, 5.5f, -1);

    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        cam = Camera.main;
        color1 = thunderAnnouncesp1.color;
        color2 = thunderAnnouncesp2.color;
    }


    /// <summary>
    /// インスタンスの作成
    /// </summary>
    public void MakeInstance()
    {
        thunder = thunderObj1.GetComponent<Thunder>();
        thunderAnnouncesp1 = GetComponent<SpriteRenderer>();
        thunderAnnouncesp2 = thunderAnnounce2.GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// アナウンスの表示非表示
    /// </summary>
    public void ShowAnnounce()
    {
        var viewportPos = cam.WorldToViewportPoint(thunderObj1.transform.position);

        //雷が動いてない
        if(!thunder.isMove)
        {
            transform.position = OffPos;
            thunderAnnounce2.transform.position = OffPos;
        }
        //雷が画面内にいる
        else if(rect.Contains(viewportPos))
        {
            transform.position = OffPos;
            thunderAnnounce2.transform.position = OffPos;
        }
        //雷が動いてて、まだ画面に入ってない
        else if(thunder.isMove && thunderObj1.transform.position.y>=0)
        {   
            //雷が1つ
            if(Score.score<thunder.twoThunderScore)
            {
                OnPos.x = thunder.startPosX1;
                transform.position = OnPos;
                if(runCoroutine==false){
                    StartCoroutine("flash");
                }
            }
            //2つ
            else
            {
                OnPos.x = thunder.startPosX1;
                transform.position = OnPos;
                OnPos.x = thunder.startPosX2;
                thunderAnnounce2.transform.position = OnPos;
                if(runCoroutine==false){
                    StartCoroutine("flash");
                }
            }
        }

    }


    //点滅させるコルーチン
    IEnumerator flash(){
        runCoroutine = true;
        while(this.transform.position.y < 10){
            yield return new WaitForSecondsRealtime(0.3f);
            color1.a = 0;
            color2.a = 0;
            thunderAnnouncesp1.color = color1;
            thunderAnnouncesp2.color = color2;
            yield return new WaitForSecondsRealtime(0.3f);
            color1.a = 1;
            color2.a = 1;
            thunderAnnouncesp1.color = color1;
            thunderAnnouncesp2.color = color2;
        }
        runCoroutine = false;
    }
}
