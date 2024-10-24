using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class UI_HighScore : MonoBehaviour
{

    [Header("References")]
    public CanvasGroup myCG;
    public RectTransform Content;

    // Start is called before the first frame update
    void Start()
    {
        myCG.alpha = 0f;

        // get highscore
        StartCoroutine(Post("https://www.trollbyte.io/PixelTv/api.php?apiVersion=One&trygethighscore=true&scorename=testScore", ""));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Post(string url, string json)
    {
        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("accept", "application/json");
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("User-Agent", "TrollByteUserAgent/1.0");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
            Debug.Log("Response Code: " + request.responseCode);
            Debug.Log("Response Text: " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.downloadHandler.text);
        }
    }

}
