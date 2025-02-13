using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Make it a singleton
    public static GameController Instance { get; private set; }

    [Header("References")]
    public PlayerController PlayerReference;

    [HideInInspector]
    public Transform SatelliteHolder;
    [HideInInspector]
    public Transform BloodHolder;
    [HideInInspector]
    public Transform XpHolder;
    [HideInInspector]
    public Transform EnemyHolder;
    [HideInInspector]
    public Transform CoinHolder;

    [Header("Prefabs")]
    public GameObject BloodSplatPrefab;
    public GameObject GenericItemPrefab;
    public GameObject GraveStonePrefab;
    public GameObject PopupTextPrefab;
    public Sprite[] CoinSprites;

    [Header("Highscores")]
    public string ScoreDataTableName = "stuTesting01";
    public bool DynamicTableName = false;
    public UI_HighScore.Score[] myScores;

    [Header("Currently Disabled")]
    public UpgradeScriptable[] allUpgradesInGame;

    [Header("Gathered Metrics")]
    public int currentFPS = 120;
    public int currentEnemies = 0;
    public int currentBloodSplats = 0;
    public int currentCoins = 0;
    public int minimumPointsForHighscore = 0;
    public float gameTime = 0f;
    public int gamePoints = 0;
    public string gamePlayerName = "For Highscore";

    [Header("Other GameController Scripts")]
    private GameController_FPS myFps;
    private GameController_ObjectPool myOP;
    public WeaponUpgradeGamble myWUG;
    public GameController_Steamrollers mySR;
    private SurroundSpawner mySS;
    public GameController_UniversalStats myUS;
    public GameController_PortraitPickUp myPP;

    private void Awake()
    {
        // Make it a singleton, and prevent duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
        // Other game controller scripts
        myFps = GetComponent<GameController_FPS>();
        myOP = GetComponent<GameController_ObjectPool>();
        myWUG = GetComponent<WeaponUpgradeGamble>();
        mySR = GetComponent<GameController_Steamrollers>();
        mySS = GetComponent<SurroundSpawner>();
        myUS = GetComponent<GameController_UniversalStats>();
        myPP = GetComponent<GameController_PortraitPickUp>();
        myPP.ResetPortraits();

        // make this object permanent, so it carries over from scene to scene
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        currentFPS = myFps.getFPS();
    }

    public void PlayerIsDead()
    {
        Time.timeScale = 0;
        UI_HUD.Instance.PlayerIsDead();
    }
    public bool FPS_isWithinLimit(float limit)
    {
        return (limit < myFps.getFPS() );
    }

    public GameObject EnemyPool_Get()
    {
        return myOP.EnemyPool.Get();
    }

    public void EnemyPool_Release(GameObject enemy)
    {
        if ( enemy ) myOP.EnemyPool.Release(enemy);
    }

    public GameObject BloodPool_Get()
    {
        return myOP.BloodPool.Get();
    }

    public void BloodPool_Release(GameObject blood)
    {
        myOP.BloodPool.Release(blood);
    }
    public GameObject CoinPool_Get()
    {
        return myOP.CoinPool.Get();
    }

    public void CoinPool_Release(GameObject coin)
    {
        myOP.CoinPool.Release(coin);
    }

    public void SetupForGame(PlayerController controller)
    {
        PlayerReference = controller;
        var holders = new GameObject("ObjectHolders");
        // satellite
        var SatHolder = new GameObject("SatelliteHolder");
        SatelliteHolder = SatHolder.transform;
        SatHolder.transform.SetParent(holders.transform);
        // blood
        var BloHolder = new GameObject("BloodHolder");
        BloodHolder = BloHolder.transform;
        BloHolder.transform.SetParent(holders.transform);
        // xp
        var eXpHolder = new GameObject("XpHolder");
        XpHolder = eXpHolder.transform;
        eXpHolder.transform.SetParent(holders.transform);
        // enemy
        var EneHolder = new GameObject("EnemyHolder");
        EnemyHolder = EneHolder.transform;
        EneHolder.transform.SetParent(holders.transform);
        // coin
        var CoiHolder = new GameObject("CoinHolder");
        CoinHolder = CoiHolder.transform;
        CoinHolder.transform.SetParent(holders.transform);
        // spawner scripts
        mySS.StartFromGameController(PlayerReference.transform,EnemyHolder);
        mySR.StartFromGameController();
        // object pool
        myOP.Start();
        // gamble script
        myWUG.StartFromGameController();
        // universal stats
        myUS.SetupForGame();

        myPP.ResetPortraits();
    }

    public void SetupForMenu()
    {
        // universal stats
        //myUS.SetupForMenu();
    }

    public string GetScoreTableName()
    {
        if ( DynamicTableName )
            return ("Y" + System.DateTime.Now.Year + "M" + System.DateTime.Now.Month);
        else
            return ScoreDataTableName;
    }
}
