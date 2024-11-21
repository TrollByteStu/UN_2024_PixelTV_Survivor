using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;


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
        StartCoroutine(Post("https://www.trollbyte.io/PixelTv/api.php?apiVersion=One&universalstats=true&scorename=" + GameController.Instance.ScoreDataTableName, ""));
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
    public void AddStatDamageDone(float damage)
    {
        TotalDamageDone += (int)damage;
    }

    IEnumerator Post(string url, string json)
    {
        // set up request
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        Debug.Log("There is new universal data, sending it.");
        json = "{\"universalstats\":[" + TotalKills + "," + TotalPoints + "," + TotalTimePlayed + "," + TotalShotFired + "," + TotalShotsHit + "," + TotalDamageDone + "]}";

        // testing
        //json = "{\"universalstats\":[1,2,3,4,5,6]}";

        // prep data
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.method = UnityWebRequest.kHttpVerbPOST;

        // set headers
        request.SetRequestHeader("accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("User-Agent", "TrollByteUserAgent/1.0");

        // wait until the request gets back
        yield return request.SendWebRequest();

        // handle the reply
        if (request.error != null)
        { // there is an error
            Debug.Log("Error: " + request.error);
            Debug.Log("Response Code: " + request.responseCode);
            Debug.Log("Response Text: " + request.downloadHandler.text);
        }
        else
        { // there is no error
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.downloadHandler.text);
        }
    }
}
