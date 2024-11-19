using UnityEngine;
using TMPro;

public class UI_HUD_Highscore : MonoBehaviour
{

    [Header("References")]
    public TMP_Text myScore;
    public TMP_Text[] Names;
    public TMP_Text[] Scores;

    //private
    private UI_HUD MyHUD;
    private GameController MyGC;
    private PlayerController MyPlayer;
    private int LastScore = -5;
    private int playerRank;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyGC = GameController.Instance;
        MyHUD = UI_HUD.Instance;
        MyPlayer = MyGC.PlayerReference;
    }

    private void FindPlayerRank()
    { // find out what position player is at..
        playerRank = 0;
        for (int i = 0; i < MyGC.myScores.Length; i++)// Score theScore in myScores)
        {
            if (int.Parse( MyGC.myScores[i].score ) > LastScore )
            { // this score is higher
                return;
            } else { // this score is not higher
                playerRank = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (MyPlayer == null) MyPlayer = MyGC.PlayerReference;
        if (MyPlayer.Stats.Points == LastScore) return;

        // update own points
        myScore.text = MyPlayer.Stats.Points.ToString();
        // update
        LastScore = MyPlayer.Stats.Points;

        // find rank
        FindPlayerRank();
    }
}
