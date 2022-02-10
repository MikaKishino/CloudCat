using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ハイスコアクラス
/// </summary>
public class Highscore : MonoBehaviour
{
    //--------------------Enum--------------------
    //--------------------Inspector--------------------]
    //4桁の数字が入る
    [SerializeField] GameObject[] chdNum = new GameObject[4];
    [SerializeField] Sprite[] numImg = new Sprite[10];


    //--------------------Instance--------------------
    Image[] Img = new Image[4];


    //--------------------Property--------------------
    //--------------------Variable--------------------
    //ハイスコア保存する
    public static int highScore = 0;
    //ハイスコアの保存先キー
    string key = "HIGHSCORE";
    //スコア表示用の作業用変数
    int digit, num;


    /// <summary>
    /// 初期設定
    /// </summary>
    public void FirstSetting()
    {
        highScore = PlayerPrefs.GetInt(key, 0);
    }


    /// <summary>
    /// インスタンス作成
    /// </summary>
    public void MakeInstance()
    {
        for(int i=0; i<4; i++){
            Img[i] = chdNum[i].GetComponent<Image>();
        }
    }


    /// <summary>
    /// ハイスコア保存
    /// </summary>
    public void SaveHighscore()
    {
        if(Score.score > highScore){
            highScore = Score.score;
            PlayerPrefs.SetInt(key, highScore);
        }
    }


    /// <summary>
    /// 表示
    /// </summary>
    public void ShowHighscore()
    {
        List<int> number = new List<int>();
        bool z = false;    //スコアが0
        digit = highScore;
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
            chdNum[i].SetActive(true);
            Img[i].sprite = numImg[number[i]];
        }
        for(int i=number.Count; i<=3; i++){
            chdNum[i].SetActive(false);
        }
    }
}
