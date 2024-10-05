using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_HUD : MonoBehaviour
{
    [Header("Sections")]
    public RectTransform Section_HUD;
    public RectTransform Section_Looser;
    public RectTransform Section_Upgrade;

    [Space(10)]

    [Header("Health Settings")]
    public RectTransform HealthSlider;
    public TMP_Text HealthText;

    [Header("XP Settings")]
    public RectTransform XpSlider;
    public TMP_Text XpText;

    // Make it a singleton
    public static UI_HUD Instance { get; private set; }

    private PlayerStats PlayerStatRef;

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
        // show only HUD for now
        Section_HUD.localScale = new Vector2(1, 1);
        Section_Looser.localScale = new Vector2(0, 0);
        Section_Upgrade.localScale = new Vector2(0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // wanted to put it in Start(), but it didnt update.. not sure why.. but this fixed it, get fresh one every time
        PlayerStatRef = GameController.Instance.PlayerReference.Stats;

        // Health
        HealthText.text = PlayerStatRef.Health + "/" + PlayerStatRef.MaxHealth;
        HealthSlider.localScale = new Vector2(PlayerStatRef.Health / PlayerStatRef.MaxHealth, 1);

        // XP
        XpText.text = PlayerStatRef.Xp + "/" + math.pow(PlayerStatRef.Level * 10, 1.4f);
        XpSlider.localScale = new Vector2(PlayerStatRef.Xp / math.pow(PlayerStatRef.Level * 10, 1.4f), 1);
    }
}
