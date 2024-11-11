using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu_Handler : MonoBehaviour
{
    public RectTransform NormalMenuRef;
    public CanvasGroup HighScoreRef;
    public RectTransform InfoRef;

    private bool HighScoreShown = false;
    private bool InfoShown = false;

    // Start is called before the first frame update
    void Start()
    {
        HighScoreRef.alpha = 0;
        InfoRef.localScale = new Vector3(0, 0, 0);
        GameController.Instance.SetupForMenu();
    }
    public void tooltipLettuce() { 
    //NewAnimationExit.GetComponent<TextMeshProUGUI>().text = "+MP";
     }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PressPlay()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("_Level_01");
    }

    public void PressInfo()
    {
        InfoRef.localScale = new Vector3(1, 1, 1);
        NormalMenuRef.localScale = new Vector3(0, 0, 0);
        InfoShown = true;
    }

    public void PressHighScore()
    {
        HighScoreRef.alpha = 1;
        NormalMenuRef.localScale = new Vector3(0, 0, 0);
        HighScoreShown = true;
    }

    public void PressExit()
    {
        if ( HighScoreShown )
        { // showing highscore
            NormalMenuRef.localScale = new Vector3(1, 1, 1);
            HighScoreRef.alpha=0;
            HighScoreShown = false;
            return;
        }
        if (InfoShown)
        { // showing info
            NormalMenuRef.localScale = new Vector3(1, 1, 1);
            InfoRef.localScale = new Vector3(0, 0, 0);
            InfoShown = false;
            return;
        }
        // possibly some animation
        Application.Quit();
    }
}
