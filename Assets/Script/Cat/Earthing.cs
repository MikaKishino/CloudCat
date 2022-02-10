using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 猫の接地判定オブジェクトクラス
/// </summary>
public class Earthing : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //--------------------Instance--------------------
    Cat cat;


    //--------------------Property--------------------
    public bool isGround{get; private set;} = true;


    //--------------------Variable--------------------
    public UnityAction action;


    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name!="bird"){
            isGround = true;
            action.Invoke();
        }
    }


    void OnTriggerExit2D(Collider2D collision){
        //ジャンプ中
        isGround = false;
    }


    public void MakeInstance()
    {
        cat = this.transform.parent.gameObject.GetComponent<Cat>();
    }
}
