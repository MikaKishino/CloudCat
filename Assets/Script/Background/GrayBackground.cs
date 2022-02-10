using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 曇り背景オブジェクトクラス
/// </summary>
public class GrayBackground : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //下から何番目の背景か
    [SerializeField] int grayBackgroundNumber;


    //--------------------Instance--------------------
    //--------------------Property--------------------
    //--------------------Variable--------------------
    //背景の初期位置
    Vector3 startPos;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        startPos = this.transform.position;
    }


    /// <summary>
    /// 背景を動かす
    /// </summary>
    public void MoveGrayBackground()
    {
        //青空の時以外に動かす
        if(GameMaster.bgState != GameMaster.BackgroundState.blue)
        {
            this.transform.Translate(0, BlueBackground.scrollSpeed*Time.deltaTime, 0);
        }
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        this.transform.position = startPos;
    }



    public void BackStartPosition()
    {
        if(GameMaster.bgState == GameMaster.BackgroundState.blue)
        {
            Init();
        }
    }


    /// <summary>
    /// 現在の背景状態の管理
    /// </summary>
    public void BGStateController()
    {
        //一番最後に流れてくる背景が流れ切った
        if(grayBackgroundNumber==4 && this.transform.position.y <= -10.0f)
        {
            GameMaster.bgState = GameMaster.BackgroundState.blue;
        }
        //一番最後に流れてくる背景が半分画面にある
        else if(grayBackgroundNumber==4 && this.transform.position.y <= 5.0f)
        {
            GameMaster.bgState = GameMaster.BackgroundState.graytoBlue;
        }

        //一番最初に流れてくる背景が画面中央に来た
        else if(GameMaster.bgState == GameMaster.BackgroundState.bluetoGray && grayBackgroundNumber==1 && this.transform.position.y <= 0f)
        {
            GameMaster.bgState = GameMaster.BackgroundState.gray;
        }
    }
}
