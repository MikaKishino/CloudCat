using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    BoxCollider2D col;

    void Start() {
        col = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// 床の当たり判定をなくす
    /// </summary>
    public void ThroughFloor()
    {
        if(Score.score >= 1){
            col.isTrigger = true;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        col.isTrigger = false;
    }
}
