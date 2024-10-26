using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Make it a singleton
    public static GameController Instance { get; private set; }

    [Header("References")]
    public PlayerController PlayerReference;

    [Header("Object Holders")]
    public Transform SatelliteHolder;
    public Transform BloodHolder;
    public Transform XpHolder;
    public Transform EnemyHolder;

    [Header("Prefabs")]
    public GameObject BloodSplatPrefab;
    public GameObject GenericItemPrefab;
    public GameObject GraveStonePrefab;
    public GameObject PopupTextPrefab;

    [Header("Currently Disabled")]
    public UpgradeScriptable[] allUpgradesInGame;

    [Header("Gathered Metrics")]
    public int currentFPS = 120;
    public int currentEnemies = 0;
    public int currentBloodSplats = 0;
    public int minimumPointsForHighscore = 0;
    public float gameTime = 0f;
    public int gamePoints = 0;
    public string gamePlayerName = "For Highscore";

    [Header("Other GameController Scripts")]
    private GameController_FPS myFps;
    private GameController_ObjectPool myOP;
    public WeaponUpgradeGamble myWUG;

    private void Awake()
    {
        // Make it a singleton, and prevent duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        } else {
            Instance = this;
        }
        // Other game controller scripts
        myFps = GetComponent<GameController_FPS>();
        myOP = GetComponent<GameController_ObjectPool>();
        myWUG = GetComponent<WeaponUpgradeGamble>();

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
        myOP.EnemyPool.Release(enemy);
    }

    public GameObject BloodPool_Get()
    {
        return myOP.BloodPool.Get();
    }

    public void BloodPool_Release(GameObject blood)
    {
        myOP.BloodPool.Release(blood);
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

        myOP.Start();
    }
}
