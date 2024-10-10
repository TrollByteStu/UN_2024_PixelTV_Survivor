using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Make it a singleton
    public static GameController Instance { get; private set; }

    public PlayerController PlayerReference;

    [Header("Prefabs")]
    public GameObject[] BloodSplatPrefabs;
    public GameObject GenericItemPrefab;

    public UpgradeScriptable[] allUpgradesInGame;

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
    }

    public void PlayerIsDead()
    {
        Time.timeScale = 0;
        UI_HUD.Instance.PlayerIsDead();
    }

}
