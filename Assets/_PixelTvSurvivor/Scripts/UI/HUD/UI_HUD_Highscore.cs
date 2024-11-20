using UnityEngine;
using TMPro;
using static UI_HighScore;

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
            if (int.Parse( MyGC.myScores[i].score ) < LastScore )
            { // this score is higher
                return;
            } else { // this score is not higher
                playerRank = i;
            }
        }
    }

    private string cleanname(string name)
    {
        name = name.Replace("u200b", "");
        name = name.Replace("&aelig;", "æ");
        name = name.Replace("&oslash;", "ø");
        name = name.Replace("&aring;", "å");
        name = name.Replace("&AElig;", "Æ");
        name = name.Replace("&Oslash;", "Ø");
        name = name.Replace("&Aring;", "Å");
        return name;
    }

    private void UpdateUiPosition(int position, string name, string score)
    {
        Names[position].text = cleanname( name );
        Scores[position].text = score;
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

        // +2 rank
        if (playerRank - 2 >= 0)
        {
            UpdateUiPosition(0, MyGC.myScores[(playerRank - 1)].name, MyGC.myScores[(playerRank - 1)].score);
        } else {
            UpdateUiPosition(0, "", "");
        }

        // +1 rank
        if (playerRank - 1 >= 0)
        {
            UpdateUiPosition(1, MyGC.myScores[(playerRank)].name, MyGC.myScores[(playerRank)].score);
        }
        else
        {
            UpdateUiPosition(1, "", "");
        }

        // -1 rank
        if (playerRank + 1 < MyGC.myScores.Length)
        {
            UpdateUiPosition(2, MyGC.myScores[(playerRank + 1)].name, MyGC.myScores[(playerRank + 1)].score);
        }
        else
        {
            UpdateUiPosition(2, "", "");
        }

        // -2 rank
        if (playerRank + 2 < MyGC.myScores.Length)
        {
            UpdateUiPosition(3, MyGC.myScores[(playerRank + 2)].name, MyGC.myScores[(playerRank + 2)].score);
        }
        else
        {
            UpdateUiPosition(3, "", "");
        }

    }
}
