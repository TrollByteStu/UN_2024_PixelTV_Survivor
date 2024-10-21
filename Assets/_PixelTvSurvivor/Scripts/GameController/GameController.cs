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

    [Header("Other GameController Scripts")]
    public GameController_FPS myFps;

    private void Awake()
    {
        // Make it a singleton, and prevent duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        // Other game controller scripts
        myFps = GetComponent<GameController_FPS>();
    }

    public void PlayerIsDead()
    {
        Time.timeScale = 0;
        UI_HUD.Instance.PlayerIsDead();
    }

}
