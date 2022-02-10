using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// パネル管理クラス
/// </summary>
public class PanelManager : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    [SerializeField] GameObject panel;
    [SerializeField] Button button;
    [SerializeField] GameObject scoreManager;
    [SerializeField] GameObject canvas;
    [SerializeField] GameObject poseStr;


    //--------------------Instance--------------------
    //--------------------Property--------------------
    //--------------------Variable--------------------
    bool runCoroutine = false;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        panel.SetActive(false);
    }


    /// <summary>
    /// ポーズ画面を表示非表示
    /// </summary>
    public void SwitchPause()
    {
        if(Application.isEditor){//エディター（キーボード操作）
            if(Input.GetKeyDown("space")){
                //プレイ→ポーズ
                if(panel.gameObject.activeSelf == false && GameMaster.nowState == GameMaster.State.play){
                    StartCoroutine("PlayToPose");
                }
                //ポーズ→プレイ
                else if(panel.gameObject.activeSelf == true){
                    StartCoroutine("PoseToPlay");
                }
            }
        }
        else{
            //プレイ→ポーズはOnButtonClick
            //ポーズ→プレイ
            if(GameMaster.nowState == GameMaster.State.pause){
                if(Input.touchCount > 0){
                    StartCoroutine("PoseToPlay");
                }
            }
        }
    }


    /// <summary>
    /// ポーズボタンを押された
    /// </summary>
    public void OnButtonClick(){
        if(GameMaster.nowState == GameMaster.State.play){
            StartCoroutine("PlayToPose");
        }
    }


    IEnumerator PlayToPose()
    {
        scoreManager.transform.parent = canvas.transform;

        float sclX, sclY, sclZ;
        for(int j=0; j<=4; j++){
            Vector3 scl = button.transform.localScale;
            sclX = scl.x;
            sclY = scl.y;
            sclZ = scl.z;
            button.transform.localScale = new Vector3(sclX*1.02f, sclY*1.02f, sclZ);
            yield return new WaitForSeconds(0.01f);
        }

        for(int j=0; j<=4; j++){
            Vector3 scl = button.transform.localScale;
            sclX = scl.x;
            sclY = scl.y;
            sclZ = scl.z;
            button.transform.localScale = new Vector3(sclX/1.02f, sclY/1.02f, sclZ);
            yield return new WaitForSeconds(0.01f);
        }

        panel.SetActive(true);
        GameMaster.nowState = GameMaster.State.pause;
        Time.timeScale = 0;
    }

    IEnumerator PoseToPlay()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        panel.SetActive(false);
        Time.timeScale = 1;
        GameMaster.nowState = GameMaster.State.play;
        runCoroutine = false;
    }


    IEnumerator Poyon(){
        runCoroutine = true;
        float sclX, sclY, sclZ;
        for(int j=0; j<=4; j++){
            Vector3 scl = poseStr.transform.localScale;
            sclX = scl.x;
            sclY = scl.y;
            sclZ = scl.z;
            poseStr.transform.localScale = new Vector3(sclX*1.01f, sclY*1.01f, sclZ);
            yield return new WaitForSecondsRealtime(0.05f);
        }

        for(int j=0; j<=4; j++){
            Vector3 scl = poseStr.transform.localScale;
            sclX = scl.x;
            sclY = scl.y;
            sclZ = scl.z;
            poseStr.transform.localScale = new Vector3(sclX/1.01f, sclY/1.01f, sclZ);
            yield return new WaitForSecondsRealtime(0.05f);
        }
        runCoroutine = false;
    }


    public void MovePoyonCoroutine()
    {
        if(!runCoroutine)
        {
            StartCoroutine("Poyon");
        }
    }
}
