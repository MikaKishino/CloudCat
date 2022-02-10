using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 青空背景オブジェクトクラス
/// </summary>
public class BlueBackground : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //開始位置、終了位置
    [SerializeField] float startLine, deadLine;

    
    //--------------------Instance--------------------
    //--------------------Property--------------------
    //--------------------Variable--------------------
    //子の背景雲
    GameObject[] childCloud;
    //背景のスクロールスピード
    public static float scrollSpeed = -0.3f;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        //子雲の取得
        childCloud = new GameObject[this.transform.childCount];
        for(int i=0; i<this.transform.childCount; i++){
            childCloud[i] = this.transform.GetChild(i).gameObject;
        }
    }


    /// <summary>
    /// 曇りの時の青空の雲を見えなくする
    /// </summary>
    public void HideCloud()
    {
        if(GameMaster.bgState == GameMaster.BackgroundState.bluetoGray ||
        GameMaster.bgState == GameMaster.BackgroundState.gray)
        {
            if(this.transform.position.y >= 9.5f){
                for(int i=0; i<childCloud.Length; i++){
                    childCloud[i].SetActive(false);
                }
            }
        }
        else
        {
            if(this.transform.position.y >= 9.5f)
            {
                for(int i=0; i<childCloud.Length; i++){
                    childCloud[i].SetActive(true);
                }
            }
        }
    }


    //初期化
    public void Init()
    {
        for(int i=0; i<childCloud.Length; i++){
            childCloud[i].SetActive(true);
        }
    }


    /// <summary>
    /// スクロール
    /// </summary>
    public void Scroll()
    {
        if(Score.score > 0)
        {
            transform.Translate(0, scrollSpeed*Time.deltaTime, 0);
        }
    }


    /// <summary>
    /// 下まで来たら元の位置に戻る
    /// </summary>
    public void BackStartPosition()
    {
        if(transform.position.y < deadLine){
            transform.position = new Vector3(0, startLine, 1);
        }
    }
}
