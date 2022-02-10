using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 鳥アナウンスオブジェクトクラス
/// </summary>
public class BirdAnnounce : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    [SerializeField] GameObject birdObj;


    //--------------------Instance--------------------
    Bird bird;
    SpriteRenderer birdAnnounceSprite;


    //--------------------Property--------------------
    //--------------------Variable--------------------
    //maincamera
    Camera cam;
    Rect rect = new Rect(0, 0, 1, 1);
    Color color;
    //コルーチンが動いているか
    bool runCoroutine = false;
    Vector3 onPos = new Vector3(2.2f, 0, -3);
    Vector3 offPos = new Vector3(-4, 0, -3);

    
    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        cam = Camera.main;
        color = birdAnnounceSprite.color;
    }


    /// <summary>
    /// インスタンス作成
    /// </summary>
    public void MakeInstance()
    {
        bird = birdObj.GetComponent<Bird>();
        birdAnnounceSprite = this.GetComponent<SpriteRenderer>();
    }


    /// <summary>
    /// アナウンスの表示非表示
    /// </summary>
    public void ShowAnnounce()
    {
        var viewportPos = cam.WorldToViewportPoint(birdObj.transform.position);
        //鳥が動いてない
        if(!bird.isMove)
        {
            transform.position = offPos;
        }
        //鳥が画面内にいる
        else if(rect.Contains(viewportPos))
        {
            transform.position = offPos;
        }
        //鳥が動いてる
        else if(bird.isMove)
        {
            onPos.y = bird.startPositionY;
            if(!bird.isOpposition && birdObj.transform.position.x <= 0.0f)
            {
                onPos.x = -2.2f;
                transform.position = onPos;
                if(!runCoroutine)
                {
                    StartCoroutine("flash");
                }
            }
            else if(bird.isOpposition && birdObj.transform.position.x >= 0.0f)
            {
                onPos.x = 2.2f;
                transform.position = onPos;
                if(!runCoroutine)
                {
                    StartCoroutine("flash");
                }
            }
        }
        else
        {
            transform.position = offPos;
        }
    }


    /// <summary>
    /// 点滅させるコルーチン
    /// </summary>
    /// <returns></returns>
    IEnumerator flash(){
        runCoroutine = true;
        //画面内にある間
        while((this.transform.position.x > -4 && !bird.isOpposition) || (this.transform.position.x < 4 && bird.isOpposition)){
            yield return new WaitForSecondsRealtime(0.3f);
            color.a = 0;
            birdAnnounceSprite.color = color;
            yield return new WaitForSecondsRealtime(0.3f);
            color.a = 1;
            birdAnnounceSprite.color = color;
        }
        runCoroutine = false;
    }
}
