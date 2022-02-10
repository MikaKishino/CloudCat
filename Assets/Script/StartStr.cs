using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スタート文字オブジェクトクラス
/// </summary>
public class StartStr : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //--------------------Instance--------------------
    //--------------------Property--------------------
    //コルーチンがうごいているか
    public bool runCoroutine{get; set;} = false;


    //--------------------Variable--------------------


    /// <summary>
    /// スタート文字がぽよんぽよんする
    /// </summary>
    /// <returns></returns>
    IEnumerator PoyonStartStr()
    {
        runCoroutine = true;
        float sclX, sclY, sclZ;
        for(int j=0; j<=4; j++){
            Vector3 scl = transform.localScale;
            sclX = scl.x;
            sclY = scl.y;
            sclZ = scl.z;
            transform.localScale = new Vector3(sclX*1.01f, sclY*1.01f, sclZ);
            yield return new WaitForSeconds(0.05f);
        }
        for(int j=0; j<=4; j++){
            Vector3 scl = transform.localScale;
            sclX = scl.x;
            sclY = scl.y;
            sclZ = scl.z;
            transform.localScale = new Vector3(sclX/1.01f, sclY/1.01f, sclZ);
            yield return new WaitForSeconds(0.05f);
        }
        runCoroutine = false;
    }


    /// <summary>
    /// コルーチンを動かす
    /// </summary>
    public void MoveCoroutine()
    {
        if(runCoroutine == false){
            StartCoroutine("PoyonStartStr");
        }
    }
}
