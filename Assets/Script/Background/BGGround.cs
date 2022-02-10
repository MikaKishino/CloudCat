using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景地面オブジェクトクラス
/// </summary>
public class BGGround : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //--------------------Instance--------------------
    //--------------------Property--------------------
    //--------------------Variable-------------------
    //地面の初期値保存
    Vector3 startPos;

    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        startPos = this.transform.position;
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        this.transform.position = startPos; 
    }


    /// <summary>
    /// スクロール
    /// </summary>
    public void Scroll()
    {
        if(this.transform.position.y >= -10 && Score.score >= 1){
            transform.Translate(0, BlueBackground.scrollSpeed*Time.deltaTime, 0);
        }
    }
}
