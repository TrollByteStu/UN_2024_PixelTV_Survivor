using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController_FPS : MonoBehaviour
{
    public int MaxFrames = 120;  //maximum frames to average over

    private int lastFPSCalculated = 0;
    private List<float> frameTimes = new List<float>();

    // Use this for initialization
    void Start()
    {
        lastFPSCalculated = 0;
        frameTimes.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        addFrame();
        lastFPSCalculated = calculateFPS();
    }

    private void addFrame()
    {
        frameTimes.Add(Time.unscaledDeltaTime);
        if (frameTimes.Count > MaxFrames)
        {
            frameTimes.RemoveAt(0);
        }
    }

    private int calculateFPS()
    {
        float newFPS = 0f;

        float totalTimeOfAllFrames = 0f;
        foreach (float frame in frameTimes)
        {
            totalTimeOfAllFrames += frame;
        }
        newFPS = ((float)(frameTimes.Count)) / totalTimeOfAllFrames;

        return (int)newFPS;
    }

    public int getFPS() {
        return lastFPSCalculated;
    }

}
