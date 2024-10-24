using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_HighScore_Single : MonoBehaviour
{
    [Header("References")]
    public RectTransform myPos;
    public TMP_Text Score_Name;
    public TMP_Text Score_Score;

    public void Setup(string name, string score, int rank)
    {
        Score_Name.text = name;
        Score_Score.text = score;
        myPos.anchoredPosition = new Vector2(0, -50 * rank -30);
        myPos.offsetMin = new Vector2(0, myPos.offsetMin.y);
        myPos.offsetMax = new Vector2(0, myPos.offsetMax.y);
    }
}
