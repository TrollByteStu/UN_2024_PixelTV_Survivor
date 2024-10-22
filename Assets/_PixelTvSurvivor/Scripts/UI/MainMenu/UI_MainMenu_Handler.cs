using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu_Handler : MonoBehaviour
{
    public GameObject NewAnimationExit;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void tooltipLettuce() { 
    NewAnimationExit.GetComponent<TextMeshProUGUI>().text = "+MP";
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

    public void PressExit()
    {
        // possibly some animation
        Application.Quit();
    }
}
