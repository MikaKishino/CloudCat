using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// スコアクラス
/// </summary>
public class Score : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------
    //猫
    [SerializeField] GameObject cat;
    [SerializeField] GameObject[] cloud;
    //数字イラスト
    [SerializeField] Sprite[] numImg = new Sprite[10];
    //スコア
    [SerializeField] GameObject[] chdScore = new GameObject[4];


    //--------------------Instance--------------------
    Image[] Img = new Image[4];


    //--------------------Property--------------------
    //--------------------Variable--------------------   
    //猫の子の雲、ひとつ前の猫の子の雲
    GameObject catChildCloud1, catChildCloud2 = null;
    //スコア
    public static int score = 0;
    //スコア表示作業用変数
    int num, digit;
    //子雲の雲番号、ひとつ前の子の雲番号
    int cloudNumber1, cloudNumber2;
    //下の雲に落ちた
    bool isFall = false;


    /// <summary>
    /// インスタンス作成
    /// </summary>
    public void MakeInstance()
    {
        for(int i=0; i<4; i++){
            Img[i] = chdScore[i].GetComponent<Image>();
        }
    }


    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        score = 0;
        catChildCloud1 = null;
        catChildCloud2 = null;
        isFall = false;
    }


    /// <summary>
    /// 雲が変わった時スコアをカウント
    /// </summary>
    public void ScoreCount()
    {
        if(cat.transform.childCount > 1)
        {
            //子の雲取得
            catChildCloud1 = cat.transform.GetChild(1).gameObject;
            for(int i=0; i<cloud.Length; i++)
            {
                if(catChildCloud1 == cloud[i])
                {
                    cloudNumber1 = i;
                }
            }

            //スコアの計算
            if(catChildCloud1!=catChildCloud2)
            {
                //最初0点のとき
                if(catChildCloud2==null)
                {
                    score += 1;
                    StartCoroutine("NumberPoyon");
                    isFall = false;
                }
                else if(cloudNumber2==15)
                {
                    if(cloudNumber1==1)
                    {
                        score += 2;
                        StartCoroutine("NumberPoyon");
                        isFall = false;
                    }
                    else if(cloudNumber1==0)
                    {
                        score += 1;
                        StartCoroutine("NumberPoyon");
                        isFall = false;
                    }
                    else
                    {
                        isFall = true;
                    }
                }
                else if(cloudNumber2==14)
                {
                    if(cloudNumber1==0)
                    {
                        score += 2;
                        StartCoroutine("NumberPoyon");
                        isFall = false;
                    }
                    else if(cloudNumber1==15)
                    {
                        score += 1;
                        StartCoroutine("NumberPoyon");
                        isFall = false;
                    }
                    else
                    {
                        isFall = true;
                    }
                }
                else
                {
                    if(cloudNumber1 == cloudNumber2+2)
                    {
                        score += 2;
                        StartCoroutine("NumberPoyon");
                        isFall = false;
                    }
                    else if(cloudNumber1 == cloudNumber2+1)
                    {
                        score += 1;
                        StartCoroutine("NumberPoyon");
                        isFall = false;
                    }
                    else
                    {
                        isFall = true;
                    }
                }
            }

            if(!isFall)
            {
                catChildCloud2 = catChildCloud1;
                cloudNumber2 = cloudNumber1;
            }
        }
    }


    /// <summary>
    /// スコアの表示
    /// </summary>
    public void NumberIllust(){
        List<int> number = new List<int>();
        bool z = false;    //スコアが0
        digit = score;
        //スコアを各桁ごとにリストにいれる
        while(digit != 0){
            num = digit%10;    //スコアを10で割った余り(一番小さい位の数字)
            digit = digit/10;    //10で割る
            number.Add(num);
            z = true;
        }
        if(z == false){  //0のとき
            number.Add(digit);
        }

        for(int i=0; i<number.Count; i++){    //桁数分繰り返す
            chdScore[i].SetActive(true);
            Img[i].sprite = numImg[number[i]];
        }
        for(int i=number.Count; i<=3; i++){
            chdScore[i].SetActive(false);
        }
    }
    

    /// <summary>
    /// スコアが変わるごとにぽよんてなる
    /// </summary>
    /// <returns></returns>
    IEnumerator NumberPoyon(){
        float sclX, sclY, sclZ;
        for(int j=0; j<=4; j++){
            for(int i=0; i<chdScore.Length; i++){
                Vector3 scl = chdScore[i].transform.localScale;
                sclX = scl.x;
                sclY = scl.y;
                sclZ = scl.z;
                chdScore[i].transform.localScale = new Vector3(sclX*1.05f, sclY*1.05f, sclZ);
            }
            yield return new WaitForSeconds(0.05f);
        }

        for(int j=0; j<=4; j++){
            for(int i=0; i<chdScore.Length; i++){
                Vector3 scl = chdScore[i].transform.localScale;
                sclX = scl.x;
                sclY = scl.y;
                sclZ = scl.z;
                chdScore[i].transform.localScale = new Vector3(sclX/1.05f, sclY/1.05f, sclZ);
            }
            yield return new WaitForSeconds(0.05f);
        }
    }
}
