using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UI_HUD : MonoBehaviour
{
    [Header("Sections")]
    public RectTransform Section_HUD;
    public RectTransform Section_Looser;
    public RectTransform Section_Upgrade;
    public RectTransform Section_Upgrade_List;
    public RectTransform Section_SlotMachine;

    [Header("Looser Options")]
    public RectTransform Looser_Button;
    public RectTransform Looser_Inputfield;
    public TMP_Text Looser_Inputfield_Text;

    [Space(10)]

    [Header("Health Settings")]
    public RectTransform HealthSlider;
    public TMP_Text HealthText;

    [Header("XP Settings")]
    public RectTransform XpSlider;
    public TMP_Text XpText;

    [Header("Misc Settings")]
    public TMP_Text PointsText;
    public TMP_Text TimeText;
    public TMP_Text DebugText;

    [Header("Prefabs to spawn in UI")]
    public GameObject UpgradeIconPrefab;

    [Header("UI Sounds")]
    public AudioClip UI_Sound_Upgrade;


    // Make it a singleton
    public static UI_HUD Instance { get; private set; }

    // in script used references
    private PlayerStats PlayerStatRef;
    private AudioSource myAS;
    private GameController myGC;

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
        // show only HUD for now, but activate all
        Section_HUD.localScale = new Vector2(1, 1);
        Section_HUD.gameObject.SetActive(true);
        Section_Looser.localScale = new Vector2(0, 0);
        Section_Looser.gameObject.SetActive(true);
        Section_Upgrade.localScale = new Vector2(0, 0);
        Section_Upgrade.gameObject.SetActive(true);
        Section_SlotMachine.localScale = new Vector2(0, 0);
        Section_SlotMachine.gameObject.SetActive(true);
    }

    private void Start()
    {
        // get references
        myAS = GetComponent<AudioSource>();
        myGC = GameController.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
        // wanted to put it in Start(), but it didnt update.. not sure why.. but this fixed it, get fresh one every time
        PlayerStatRef = myGC.PlayerReference.Stats;

        // Health
        HealthText.text = math.floor( PlayerStatRef.Health ) + "/" + PlayerStatRef.MaxHealth;
        HealthSlider.localScale = new Vector2(PlayerStatRef.Health / PlayerStatRef.MaxHealth, 1);

        // XP
        XpText.text = PlayerStatRef.Coins + " €";
        //XpSlider.localScale = new Vector2(PlayerStatRef.Xp / math.pow(PlayerStatRef.Level * 10, 1.4f), 1);

        // time
        TimeText.text = math.floor(PlayerStatRef.TimeUntilDeath / 60) +":" + math.floor(PlayerStatRef.TimeUntilDeath % 60);

        // points
        PointsText.text = math.floor(PlayerStatRef.Points).ToString();

        // Debug
        DebugText.text = "Debug: \nFps: " + myGC.currentFPS + "\nEnemies: " + myGC.currentEnemies+"\nConfetti: "+ myGC.currentBloodSplats;
    }

    public void PlayerIsDead()
    {
        Section_HUD.localScale = new Vector2(0, 0);
        Section_Looser.localScale = new Vector2(1, 1);
        Cursor.visible = true;
        if (myGC.minimumPointsForHighscore < myGC.gamePoints )
        { // got a highscore
            Looser_Button.gameObject.SetActive(false);
            Looser_Inputfield.gameObject.SetActive(true);
        } else { // no highscore
            Looser_Button.gameObject.SetActive(true);
            Looser_Inputfield.gameObject.SetActive(false);
        }
    }
    void clearTransformChildren(Transform needToClear)
    {
        if (needToClear.childCount > 0)
        {
            foreach (Transform child in needToClear)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
    }

    public void UpgradeShow()
    {
        Time.timeScale = 0;
        Section_HUD.localScale = new Vector2(0, 0);
        Section_Upgrade.localScale = new Vector2(1, 1);
        // play sound
        myAS.clip = UI_Sound_Upgrade;
        myAS.Play();
        // get upgrades to list
        var acceptableUpgrades = UpgradeShow_GetListOfUpgrades();
        // remove old list
        if ( Section_Upgrade_List.childCount > 0 ) clearTransformChildren(Section_Upgrade_List.transform);
        if ( acceptableUpgrades.Length > 0)
        { // show the upgrade buttons
            for ( int i = 0 ; i < acceptableUpgrades.Length ; i++ )
            {
                if ( acceptableUpgrades[i] != null)
                {
                     var newUI = Instantiate(UpgradeIconPrefab);
                    newUI.transform.SetParent(Section_Upgrade_List);
                    var newUIscript = newUI.GetComponent<UI_Upgrade_Icon>();
                    newUIscript.chosenUpgradeType = acceptableUpgrades[i];
                    newUIscript.SetUp();
                    var rectTransform = newUI.GetComponent<RectTransform>();
                    rectTransform.localPosition = new Vector2(0, 0);
                }
            }
        } else
        { // no upgrade buttons to show
            UpgradeHide();
        }
    }
    public UpgradeScriptable[] UpgradeShow_GetListOfUpgrades()
    { // get the full list of upgrades to show the player
        var outputList = new UpgradeScriptable[3];
        int counter = 0;
        var completeList = GameController.Instance.allUpgradesInGame;
        var playerRef = GameController.Instance.PlayerReference;
        // go through them all
        foreach (UpgradeScriptable testThis in completeList)
        {
            if ( UpgradeShow_TestUpgrade( testThis , playerRef ) && counter< 3 )
            { // this works and we have not exceeded our max
                outputList[counter] = testThis;
            }
        }
        return outputList;
    }
    public bool UpgradeShow_TestUpgrade(UpgradeScriptable testThis, PlayerController playerRef)
    { // can this upgrade be show to this player at this time?
        // the many reasons it can fail
        if (testThis.UpgradeName.Length < 1) return false;
        //if (testThis.levelRequired > playerRef.Stats.Level) return false;
        // no fails means it should be shown
        return true;
    }
    public void UpgradeHide()
    {
        Time.timeScale = 1;
        Section_HUD.localScale = new Vector2(1, 1);
        Section_Upgrade.localScale = new Vector2(0, 0);
    }

    public void ButtonPress_GameOver()
    {
        SceneManager.LoadScene("_MainMenu");
    }

    public void EnterHighScore_GameOver(string name)
    {
        myGC.gamePlayerName = Looser_Inputfield_Text.text;
        SceneManager.LoadScene("_MainMenu");
    }

    public void ShowSlotMachine()
    {
        Section_SlotMachine.localScale = new Vector2(1, 1);
        Section_SlotMachine.GetComponent<UI_Slotmachine>().StartRoll();
    }
    public void ShowSlotMachineDemand(LootItemScriptable item)
    {
        Section_SlotMachine.localScale = new Vector2(1, 1);
        Section_SlotMachine.GetComponent<UI_Slotmachine>().StartRollDemandPrize(item);
    }
    public void HideSlotMachine()
    {
        Section_SlotMachine.localScale = new Vector2(0, 0);
    }
}
