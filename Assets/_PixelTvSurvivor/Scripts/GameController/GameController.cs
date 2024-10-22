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
    public GameObject[] BloodSplatPrefabs;
    public GameObject GenericItemPrefab;
    public GameObject GraveStonePrefab;
    public GameObject PopupTextPrefab;

    [Header("Currently Disabled")]
    public UpgradeScriptable[] allUpgradesInGame;

    [Header("Gathered Metrics")]
    public int currentFPS;
    public int currentEnemies;

    [Header("Other GameController Scripts")]
    private GameController_FPS myFps;
    public GameController_ObjectPool myOP;
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
        return (limit > myFps.getFPS() );
    }

}
