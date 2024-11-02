using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Slot : MonoBehaviour
{
    public UI_Slotmachine myMachine;

    public RectTransform Mover;
    public Image Image1;
    public Image Image2;
    public Image Image3;
    public Image Image4;

    private bool isRolling = false;
    private bool stopNextRoll = false;
    private float mover = 0f;
    private float offset = 0f;

    private List<LootItemScriptable> myPrizes;

    private AudioSource myWheelAS;
    private AudioSource myStopAS;

    private void Awake()
    {
        myPrizes = new List<LootItemScriptable>();
        myWheelAS = GetComponent<AudioSource>();
        myStopAS = Mover.GetComponent<AudioSource>();
    }
    public void Setup( UI_Slotmachine master, float newOffset)
    {
        myMachine = master;
        offset = newOffset;
        // setup icons
        myPrizes.Clear();
        for (int i = 0; i < 4; i++)
        {
            myPrizes.Add(myMachine.MainPrizes[Random.Range(0,myMachine.MainPrizes.Length)]);
        }
        UpdateIcons();
    }

    public void StartRolling()
    {
        isRolling = true;
        stopNextRoll = false;
        mover = 0f;
        myWheelAS.Play();
    }
    public void StopRollingNextIcon()
    {
        stopNextRoll = true;
    }
    private void StopRolling()
    {
        myMachine.AddPrizeFromRoll(myPrizes[2]);
        isRolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling) return;
        mover -= Time.deltaTime*500f;
        Mover.localPosition = new Vector2(0f,mover+offset);
        while (mover<-35f)
        { // moveup, reshuffle
            if (stopNextRoll)
            {
                Mover.localPosition = new Vector2(0f, -35f);
                StopRolling();
                myWheelAS.Stop();
                myStopAS.Play();
                return;
            }
            mover += 35f;
            myPrizes.RemoveAt(0);
            myPrizes.Add(myMachine.MainPrizes[Random.Range(0, myMachine.MainPrizes.Length)]);
            UpdateIcons();
        }
    }

    private void UpdateIcons()
    {
        Image4.sprite = myPrizes[0].ItemSprite;
        //Image4.transform.name = myPrizes[0].ItemName;
        Image3.sprite = myPrizes[1].ItemSprite;
        //Image3.transform.name = myPrizes[1].ItemName;
        Image2.sprite = myPrizes[2].ItemSprite;
        //Image2.transform.name = myPrizes[2].ItemName;
        Image1.sprite = myPrizes[3].ItemSprite;
        //Image1.transform.name = myPrizes[3].ItemName;
    }
}
