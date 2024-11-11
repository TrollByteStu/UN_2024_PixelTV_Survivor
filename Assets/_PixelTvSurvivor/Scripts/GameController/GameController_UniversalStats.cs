using UnityEngine;

public class GameController_UniversalStats : MonoBehaviour
{
    /* Stats to track:
    Players who played
    Total Kills
    Total Points
    Total time played
    Total Shots Fired
    Total Shot Hit
    Total damage done
    */

    [Header("Universal Stats")]
    public int TotalKills = 0;
    public int TotalPoints = 0;
    public int TotalTimePlayed = 0;
    public int TotalShotFired = 0;
    public int TotalShotsHit = 0;
    public int TotalDamageDone = 0;

    // internal tracking of the gamestate
    private bool HasPlayed = false;

    public void SetupForGame()
    {
        HasPlayed = true; 
    }
    public void SetupForMenu()
    {
        if (!HasPlayed) return;
        // send the stats
        // reset the stats after sending them
        HasPlayed = false;
        TotalKills = 0;
        TotalPoints = 0;
        TotalTimePlayed = 0;
        TotalShotFired = 0;
        TotalShotsHit = 0;
        TotalDamageDone = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasPlayed) return;
        if (Time.timeSinceLevelLoad > 10) TotalTimePlayed = (int)Time.timeSinceLevelLoad;
    }

    public void AddStatOneKill()
    {
        TotalKills++;
    }
    public void AddStatPoints(int points)
    {
        TotalPoints += points;
    }
    public void AddStatOneShotFired()
    {  
        TotalShotFired++; 
    }
    public void AddStatOneShotHit()
    {
        TotalShotsHit++;
    }
    public void AddStatDamageDone(int damage)
    {
        TotalDamageDone += damage;
    }
}
