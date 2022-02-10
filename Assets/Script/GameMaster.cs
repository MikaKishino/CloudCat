using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* copyright (株)DigitalMonkey 制作者 岸野　2021
*/


/// <summary>
/// ゲーム管理クラス
/// </summary>
public class GameMaster : MonoBehaviour
{
    //--------------------Enum--------------------
    // ゲーム状態
    public enum State{
        start,
        play,
        gameover,
        pause
    }
    public static State nowState = State.start;    //現在の状態

    //背景の状態
    public enum BackgroundState{
        //青空
        blue,
        //曇り空
        gray,
        //青空と曇り
        bluetoGray,
        graytoBlue,
    }
    public static BackgroundState bgState = BackgroundState.blue;


    //--------------------Inspector--------------------
    [SerializeField] GameObject catObj;
    [SerializeField] GameObject earthingObj;
    [SerializeField] GameObject startObj;
    [SerializeField] GameObject[] cloudObj;
    [SerializeField] GameObject floorObj;
    [SerializeField] GameObject[] airplaneObj;
    [SerializeField] GameObject thunderObj;
    [SerializeField] GameObject thunderAnnouceObj;
    [SerializeField] GameObject[] birdObj;
    [SerializeField] GameObject[] birdAnnounceObj;
    [SerializeField] GameObject[] grayBackgroundObj;
    [SerializeField] GameObject[] blueBackgroundObj;
    [SerializeField] GameObject bgGroundObj;
    [SerializeField] GameObject highscoreObj;
    [SerializeField] GameObject scoreObj;
    [SerializeField] GameObject baloonObj;


    //--------------------Instance--------------------
    StartStr startStr;
    Cat cat;
    Earthing earthing;
    Cloud[] cloud;
    ad ad;
    Floor floor;
    Airplane[] airplane;
    Thunder thunder;
    ThunderAnnounce thunderAnnounce;
    Bird[] bird;
    BirdAnnounce[] birdAnnounce;
    GrayBackground[] grayBackground;
    BlueBackground[] blueBackground;
    BGGround bgGround;
    Highscore highscore;
    Score score;
    PanelManager panelManager;
    Baloon baloon;


    //--------------------Property--------------------
    //--------------------Variable--------------------
    //ゲームオーバー後の処理が終わった
    bool isFinishGameOverProcess = true;
    //ゲームスタート時の処理が終わった
    bool isFinishStartProcess = false;
    public static bool isCatHitEnemy = false;


    void Start()
    {
        Application.targetFrameRate = 60;
        //インスタンス作成
        startStr = startObj.GetComponent<StartStr>();
        cat = catObj.GetComponent<Cat>();
        earthing = earthingObj.GetComponent<Earthing>();
        cloud = new Cloud[cloudObj.Length];
        for(int i=0; i<cloud.Length; i++)
        {
            cloud[i] = cloudObj[i].GetComponent<Cloud>();
        }
        ad = GetComponent<ad>();
        floor = floorObj.GetComponent<Floor>();
        airplane = new Airplane[airplaneObj.Length];
        for(int i=0; i<airplane.Length; i++)
        {
            airplane[i] = airplaneObj[i].GetComponent<Airplane>();
        }
        thunder = thunderObj.GetComponent<Thunder>();
        thunderAnnounce = thunderAnnouceObj.GetComponent<ThunderAnnounce>();
        grayBackground = new GrayBackground[grayBackgroundObj.Length];
        for(int i=0; i<grayBackgroundObj.Length; i++)
        {
            grayBackground[i] = grayBackgroundObj[i].GetComponent<GrayBackground>();
        }
        bird = new Bird[birdObj.Length];
        birdAnnounce = new BirdAnnounce[birdAnnounceObj.Length];
        for(int i=0; i<bird.Length; i++)
        {
            bird[i] = birdObj[i].GetComponent<Bird>();
            birdAnnounce[i] = birdAnnounceObj[i].GetComponent<BirdAnnounce>();
        }
        blueBackground = new BlueBackground[blueBackgroundObj.Length];
        for(int i=0; i<blueBackground.Length; i++)
        {
            blueBackground[i] = blueBackgroundObj[i].GetComponent<BlueBackground>();
        }
        bgGround = bgGroundObj.GetComponent<BGGround>();
        highscore = highscoreObj.GetComponent<Highscore>();
        score = scoreObj.GetComponent<Score>();
        panelManager = GetComponent<PanelManager>();
        baloon = baloonObj.GetComponent<Baloon>();

        //猫
        cat.MakeInstance();
        cat.FirstSetting();
        //接地
        earthing.MakeInstance();
        //雲
        for(int i=0; i<cloud.Length; i++)
        {
            cloud[i].MakeInstance();
            cloud[i].FirstSetting();
        }
        //飛行機
        for(int i=0; i<airplane.Length; i++)
        {
            airplane[i].MakeInstance();
            airplane[i].FirstSetting();
        }
        //雷
        thunder.MakeInstance();
        //雷アナウンス
        thunderAnnounce.MakeInstance();
        thunderAnnounce.FirstSetting();
        //鳥、鳥アナウンス
        for(int i=0; i<bird.Length; i++)
        {
            bird[i].MakeInstance();
            birdAnnounce[i].MakeInstance();
            birdAnnounce[i].FirstSetting();
        }
        //曇り背景
        for(int i=0; i<grayBackgroundObj.Length; i++)
        {
            grayBackground[i].FirstSetting();
        }
        //青空背景
        for(int i=0; i<blueBackground.Length; i++)
        {
            blueBackground[i].FirstSetting();
        }
        //背景地面
        bgGround.FirstSetting();
        //スコア
        score.MakeInstance();
        panelManager.FirstSetting();
        //風船
        baloon.FirstSetting();
    }

    void Update()
    {
        GameStateController();
        if(nowState == State.start)
        {
            isFinishGameOverProcess = false;

            bgState = BackgroundState.blue;
            isCatHitEnemy = false;
            //スタート文字について
            startObj.SetActive(true);
            startStr.MoveCoroutine();
            //床
            floor.Init();
            //猫
            cat.Init();
            //雲
            for(int i=0; i<cloud.Length; i++)
            {
                cloud[i].Init();
                cloud[i].CheckDummy();
            }
            thunder.Init();
            for(int i=0; i<bird.Length; i++)
            {
                bird[i].Init();
            }
            //曇り背景
            for(int i=0; i<grayBackgroundObj.Length; i++)
            {
                grayBackground[i].Init();
            }
            //青空背景
            for(int i=0; i<blueBackground.Length; i++)
            {
                blueBackground[i].Init();
            }
            //背景地面
            bgGround.Init();
            //ハイスコア
            highscore.MakeInstance();
            highscore.FirstSetting();
            highscore.ShowHighscore();
            //スコア
            score.Init();

            isFinishStartProcess = true;
        }

        if(nowState == State.play)
        {
            isFinishStartProcess = false;

            //スタート文字について
            startStr.runCoroutine = false;
            startObj.SetActive(false);
            //床
            floor.ThroughFloor();
            //猫
            cat.IllustController();
            cat.SideToSide();
            cat.LimitJumpHight();
            cat.ColliderColtroller();
            //雲
            for(int i=0; i<cloud.Length; i++)
            {
                cloud[i].CloudTypeProcess();
                cloud[i].ThroughCloud();
                cloud[i].CloudParent();
                cloud[i].ChangeMoveSpeed();
                cloud[i].OutFrame();
                cloud[i].changeScrolSpeed();
                cloud[i].CloudBackToScreenOver();
                cloud[i].KeepPosition();
            }
            //飛行機
            for(int i=0; i<airplane.Length; i++)
            {
                airplane[i].MoveCoroutine();
            }
            //雷
            thunder.BackStartPosition();
            thunder.MoveCoroutine();
            thunderAnnounce.ShowAnnounce();
            //曇り背景
            for(int i=0; i<grayBackgroundObj.Length; i++)
            {
                grayBackground[i].BGStateController();
                grayBackground[i].MoveGrayBackground();
                grayBackground[i].BackStartPosition();
            }
            //鳥、鳥アナウンス
            for(int i=0; i<bird.Length; i++)
            {
                bird[i].BackStartPosition();
                bird[i].MoveCoroutine();
                birdAnnounce[i].ShowAnnounce();
            }
            //青空背景
            for(int i=0; i<blueBackground.Length; i++)
            {
                blueBackground[i].Scroll();
                blueBackground[i].BackStartPosition();
                blueBackground[i].HideCloud();
            }
            //背景地面
            bgGround.Scroll();
            //ハイスコア
            highscore.SaveHighscore();
            highscore.ShowHighscore();
            //スコア
            score.ScoreCount();
            score.NumberIllust();
            //ポーズ
            panelManager.SwitchPause();
        }

        if(nowState == State.gameover)
        {
            //雲
            for(int i=0; i<cloud.Length; i++)
            {
                cloud[i].InitGameOver();
            }
            //広告
            ad.PlayAd();
            //飛行機
            for(int i=0; i<airplane.Length; i++)
            {
                airplane[i].Init();
            }

            isFinishGameOverProcess = true;
        }

        if(nowState == State.pause)
        {
            panelManager.MovePoyonCoroutine();
            panelManager.SwitchPause();
        }
    
        if(nowState != State.pause)
        {
            baloon.MoveCoroutine();
            baloon.Init();
        }
    }

    void FixedUpdate()
    {
        if(nowState == State.play)
        {
            //猫
            cat.OperateCat();
            //雲
            for(int i=0; i<cloud.Length; i++)
            {
                cloud[i].MoveCloud();
            }
            //飛行機
            for(int i=0; i<airplane.Length; i++)
            {
                airplane[i].MoveAirplane();
            }
            //雷
            thunder.MoveThunder();
            for(int i=0; i<bird.Length; i++)
            {
                bird[i].fuwafuwa();
            }
        }

        if(nowState != State.pause)
        {
            baloon.MoveBaloon();
        }
    }


    /// <summary>
    /// ゲーム状態の管理
    /// </summary>
    void GameStateController()
    {
        //スタート→プレイ
        if(nowState == State.start)
        {
            if(isFinishStartProcess)
            {
                //エディター
                if(Application.isEditor){
                    if(Input.GetKey("left") || Input.GetKey("right")){
                        nowState = State.play;
                        Debug.Log("play");
                    }
                }
                else{
                    if(Input.touchCount > 0){
                        nowState = State.play;
                    }
                }
            }
        }

        //プレイ→ゲームオーバー
        if(nowState == State.play)
        {
            if(catObj.transform.position.y <= -8.0f)
            {
                nowState = State.gameover;
                Debug.Log("over");
            }
        }

        //ゲームオーバー→ゲームスタート
        if(nowState == State.gameover)
        {
            //ゲームオーバー後の処理が全部終わった
            if(isFinishGameOverProcess)
            {
                nowState = State.start;
                Debug.Log("start");
            }
        }
    }
}
