using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;

public class UI_HighScore_Single : MonoBehaviour
{
    [Header("References")]
    public RectTransform myPos;
    public TMP_Text Score_Name;
    public TMP_Text Score_Score;

    private string cleanname(string name)
    {
        name= name.Replace("u200b", "");
        name = name.Replace("&aelig;", "�");
        name = name.Replace("&oslash;", "�");
        name = name.Replace("&aring;", "�");
        name = name.Replace("&AElig;", "�");
        name = name.Replace("&Oslash;", "�");
        name = name.Replace("&Aring;", "�");
        return name;
    }

    public void Setup(string name, string score, int rank)
    {
        Score_Name.text = (rank+1).ToString()+": "+ cleanname(name);
        Score_Score.text = score;
        myPos.anchoredPosition = new Vector2(0, -50 * rank -30);
        myPos.offsetMin = new Vector2(0, myPos.offsetMin.y);
        myPos.offsetMax = new Vector2(0, myPos.offsetMax.y);
    }
}
