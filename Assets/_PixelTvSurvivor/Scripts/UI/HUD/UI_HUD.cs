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
    public RectTransform Section_Portraits;
    public UI_HUD_Highscore myHighScore;

    [Header("Looser Options")]
    public TMP_Text Looser_Summarization_Text;
    public TMP_Text Looser_Missed_Text;
    public TMP_Text Looser_Hit_Text;
    public Image Looser_Summarization_Piechart;
    public RectTransform Looser_Button;
    public RectTransform Looser_Inputfield;
    public TMP_Text Looser_Inputfield_Text;
    public GameObject LooserImage;
    public GameObject WinnerImage;

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
    public GameObject PortraitPrefab;

    [Header("UI Sounds")]
    public AudioClip UI_Sound_Upgrade;


    // Make it a singleton
    public static UI_HUD Instance { get; private set; }

    // in script used references
    private PlayerStats PlayerStatRef;
    private AudioSource myAS;
    private GameController myGC;
    private UI_Slotmachine mySlotUI;
    private GameObject AudioMainRef;
    private CanvasGroup PortraitGroup;

    // private ui variables
    private float portraitTimer = 0f;

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

        // internal
        mySlotUI = Section_SlotMachine.gameObject.GetComponent<UI_Slotmachine>();
        AudioMainRef = GameObject.Find("Audio");
        PortraitGroup = Section_Portraits.GetComponent<CanvasGroup>();
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
        HealthSlider.localScale = new Vector2(math.clamp(PlayerStatRef.Health / PlayerStatRef.MaxHealth,0,1), 1);

        // XP
        XpText.text = PlayerStatRef.Coins + "/"+ myGC.myWUG.CostOfRoll + " Kr.";
        XpSlider.localScale = new Vector2(math.clamp(PlayerStatRef.Coins / (float)myGC.myWUG.CostOfRoll, 0,1), 1);

        // time
        TimeText.text = math.floor(PlayerStatRef.TimeUntilDeath / 60) +":" + math.floor(PlayerStatRef.TimeUntilDeath / 10 % 6) + math.floor(PlayerStatRef.TimeUntilDeath % 10);

        // points
        PointsText.text = math.floor(PlayerStatRef.Points).ToString();

        // Debug
        DebugText.text = "Debug: \nFps: " + myGC.currentFPS + " TT:"+((int)Time.time)+ " TSLL:" + ((int)Time.timeSinceLevelLoad) + "\n";
        DebugText.text += "Enemies: " + myGC.currentEnemies+"\nConfetti: "+ myGC.currentBloodSplats + "\nCoins: " + myGC.currentCoins;

        // portrait timer
        if ( portraitTimer < Time.timeSinceLevelLoad && PortraitGroup.alpha > 0 )
        {
            PortraitGroup.alpha -= 0.01f;
        }
    }

    public void PlayerIsDead()
    {
        Section_HUD.localScale = new Vector2(0, 0);
        Section_Looser.localScale = new Vector2(1, 1);
        Cursor.visible = true;
        AudioMainRef.SetActive(false);
        Destroy(Section_SlotMachine.gameObject);
        Looser_Summarization_Text.text = PlayerIsDead_Summarization();
        Looser_Summarization_Piechart.fillAmount = (float)myGC.myUS.TotalShotsHit / (float)myGC.myUS.TotalShotFired;
        Looser_Hit_Text.text = myGC.myUS.TotalShotsHit.ToString() + " Pletskud";
        Looser_Missed_Text.text = (myGC.myUS.TotalShotFired - myGC.myUS.TotalShotsHit).ToString() + " Forbiere";
        if (myGC.PlayerReference.Stats.TimeUntilDeath > 1)
        { // time left, evil kids murdered santa, you lost
            LooserImage.gameObject.SetActive(true);
            WinnerImage.gameObject.SetActive(false);
            LooserImage.GetComponent<AudioSource>().Play();
        } else { // no time left, you ran the clock and won
            LooserImage.gameObject.SetActive(false);
            WinnerImage.gameObject.SetActive(true);
            WinnerImage.GetComponent<AudioSource>().Play();
        }
        if (myGC.minimumPointsForHighscore < myGC.gamePoints )
        { // got a highscore
            Looser_Button.gameObject.SetActive(false);
            Looser_Inputfield.gameObject.SetActive(true);
        } else { // no highscore
            Looser_Button.gameObject.SetActive(true);
            Looser_Inputfield.gameObject.SetActive(false);
        }
        myGC.myUS.SendGameStats();
    }
    private string PlayerIsDead_Summarization()
    {
        string theoutput = "Glade børn: <color=green>" + myGC.myUS.TotalKills + "</color>";
        theoutput += "\nPakker Sendt: <color=blue>" + myGC.myUS.TotalShotFired + "</color>";
        //theoutput += "\nPakker Modtaget: <color=yellow>" + myGC.myUS.TotalShotsHit + "</color>";
        theoutput += "\nPoints Optjent: <color=red>" + myGC.gamePoints+ "</color>";

        return theoutput + "\n\n\n\nYeah, it's hard to be a nissemann";
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
        Section_SlotMachine.GetComponent<UI_Slotmachine>().Reset();
    }
    public void ShowSlotMachine_PullHandle()
    {
        mySlotUI.PullHandle();
    }
    public void ShowSlotMachineDemand(LootItemScriptable item)
    {
        Section_SlotMachine.localScale = new Vector2(1, 1);
        Section_SlotMachine.GetComponent<UI_Slotmachine>().ResetDemandPrize(item);
    }
    public void HideSlotMachine()
    {
        Section_SlotMachine.localScale = new Vector2(0, 0);
    }

    public void AddPortrait(Sprite image, int position, bool flipTopBottom, string name)
    {
        var por = Instantiate(PortraitPrefab).GetComponent<UI_Portrait>();
        por.GetComponent<Image>().sprite = image;
        por.transform.SetParent(Section_Portraits);
        por.transform.localPosition = new Vector3((position*-120)-50,0,0);
        por.transform.localScale = new Vector2(1, 1);
        if (flipTopBottom)
            por.TopName.text = name;
        else por.BottomName.text = name;
        portraitTimer = Time.timeSinceLevelLoad + 2f;
        PortraitGroup.alpha = 1;
    }
}
