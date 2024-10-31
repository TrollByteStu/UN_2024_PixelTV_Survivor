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
    private float mover = 0f;
    private float offset = 0f;

    private List<LootItemScriptable> myPrizes;

    private void Awake()
    {
        myPrizes = new List<LootItemScriptable>();
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
        mover = 0f;
    }
    public void StopRolling()
    {
        isRolling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling) return;
        mover -= Time.deltaTime*250f;
        Mover.localPosition = new Vector2(0f,mover+offset);
        while (mover<-35f)
        { // moveup, reshuffle
            mover += 35f;
            myPrizes.RemoveAt(0);
            myPrizes.Add(myMachine.MainPrizes[Random.Range(0, myMachine.MainPrizes.Length)]);
            UpdateIcons();
        }
    }

    private void UpdateIcons()
    {
        Image4.sprite = myPrizes[0].ItemSprite;
        Image3.sprite = myPrizes[1].ItemSprite;
        Image2.sprite = myPrizes[2].ItemSprite;
        Image1.sprite = myPrizes[3].ItemSprite;
    }
}
