using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //--------------------Instance--------------------
    //--------------------Property--------------------
    //--------------------Variable--------------------
    //コルーチンが動いているか
    bool runCoroutine = false;
    //風船が動く
    bool isBaloonMove = false;
    //風船初期値
    Vector3 baloonStartPos;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        baloonStartPos = this.transform.position;
    }


    /// <summary>
    /// コルーチンの実行
    /// </summary>
    public void MoveCoroutine()
    {
        if(!runCoroutine)
        {
            StartCoroutine("WaitBaloon");
        }
    }


    /// <summary>
    /// 風船が上まで行ったとき初期化
    /// </summary>
    public void Init()
    {
        if(this.transform.position.y >= 6.0f)
        {
            isBaloonMove = false;
            this.transform.position = baloonStartPos;
        }
    }


    /// <summary>
    /// 風船を動かす
    /// </summary>
    public void MoveBaloon()
    {
        if(isBaloonMove)
        {
            this.transform.Translate(0, 0.05f, 0);
        }
    }


    /// <summary>
    /// バルーン待機
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitBaloon(){
        runCoroutine = true;
        yield return new WaitForSeconds(15);
        isBaloonMove = true;
        runCoroutine = false;
    }
}
