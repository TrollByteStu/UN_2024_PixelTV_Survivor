using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu_Handler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressPlay()
    {
        SceneManager.LoadScene("_Level_01");
    }

    public void PressExit()
    {
        // possibly some animation
        Application.Quit();
    }
}
