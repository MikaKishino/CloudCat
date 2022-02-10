using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    BoxCollider2D col;
    GameObject s1;
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.Translate(new Vector3(0, -0.001f, 0));
    }
}
