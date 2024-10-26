using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System;

public class UI_HighScore : MonoBehaviour
{

    [Header("References")]
    public CanvasGroup myCG;
    public RectTransform Content;
    public GameObject UI_Single_Score_Prefab;

    [Header("Database Data")]
    public string ScoreDataTableName = "stuTesting01";

    // Score data
    private ApiData myData;
    private Score[] myScores;

    // Start is called before the first frame update
    void Start()
    {
        myCG.alpha = 0f;

        // get highscore
        StartCoroutine(Post("https://www.trollbyte.io/PixelTv/api.php?apiVersion=One&trygethighscore=true&scorename="+ ScoreDataTableName, ""));
    }

    IEnumerator Post(string url, string json)
    {
        // set up request
        UnityWebRequest request = new UnityWebRequest(url, "POST");

        // is there a new highscore, and a new name?
        if (GameController.Instance.gamePoints > GameController.Instance.minimumPointsForHighscore && GameController.Instance.gamePlayerName.Length > 0)
        {
            Debug.Log("There is a new highscore, sending it.");
            json = "{\"newhighscore\":[\"" + GameController.Instance.gamePlayerName + "\",\"" + GameController.Instance.gamePoints + "\"]}";
        }

        // Testing
        json = "{\"newhighscore\":[\"tester\",\"1\"]}";

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
        } else { // there is no error
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.downloadHandler.text);
            HandleHighScoreResponse(request.downloadHandler.text);
        }
    }

    private void HandleHighScoreResponse(string response)
    {
        // decode json main object
        myData = JsonUtility.FromJson<ApiData>(response);

        // Now, deserialize the score field, which is JSON-encoded
        myScores = myData.GetScores();

        // Test it by logging the first score
        Debug.Log("debug " + myData.debug);
        for( int i = 0; i < myScores.Length; i++)// Score theScore in myScores)
        {
            Debug.Log("score: " + myScores[i].name + " " + myScores[i].score + " pts");
            var uiScore = Instantiate(UI_Single_Score_Prefab).GetComponent<UI_HighScore_Single>();
            uiScore.transform.SetParent(Content);
            uiScore.Setup(myScores[i].name, myScores[i].score,i);
        }

        // set the minimum points, from API
        GameController.Instance.minimumPointsForHighscore =  int.Parse( myData.minimum );
        // reset points from last game
        GameController.Instance.gamePoints = 0;

        // show the ui
        myCG.alpha = 1f;
    }

    [Serializable]
    public class ApiData
    {
        public string debug;
        public string minimum;
        public string score;

        public static ApiData CreateFromJSON(string jsonString)
        {
            return JsonUtility.FromJson<ApiData>(jsonString);
        }
        public Score[] GetScores()
        {
            return JsonUtility.FromJson<ScoreWrapper>("{\"scores\":" + score + "}").scores;
        }

        public class ScoreWrapper
        {
            public Score[] scores;
        }
    }

    [Serializable]
    public class Score
    {
        public string name;
        public string score;
    }
}
