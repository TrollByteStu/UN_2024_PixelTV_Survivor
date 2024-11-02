using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;

public class UI_Slotmachine : MonoBehaviour
{
    [Header("Prizes")]
    public LootItemScriptable[] MainPrizes;
    public LootItemScriptable[] ConsolationPrizes;

    [Header("References")]
    public UI_Slot Slot1;
    public UI_Slot Slot2;
    public UI_Slot Slot3;
    public Animator HandleAnimation;

    private bool isRolling = false;
    private float rollTime = 0f;
    private int rollStage = 0;
    public List<LootItemScriptable> rolledPrizes;

    private AudioSource myHandleAS;
    private AudioSource myWinningAS;

    // Start is called before the first frame update
    void Start() {
        Slot1.Setup(this,11f);
        Slot2.Setup(this,22f);
        Slot3.Setup(this,33f);
        rolledPrizes = new List<LootItemScriptable>();
        myHandleAS = HandleAnimation.GetComponent<AudioSource>();
        myWinningAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling) return;
        if (rollTime > Time.time) return;
        switch( rollStage )
        { // the action order
            case 4: // show all 3,play sound?, maybe particle effects?
                rollStage++;
                myWinningAS.Play();
                myWinningAS.time = 0;
                rollTime = Time.time + 3f;
                return;
            case 3: // stop 3rd
                rollStage++;
                rollTime = Time.time + 1f;
                Slot3.StopRollingNextIcon();
                return;
            case 2: // stop 2nd
                rollStage++;
                rollTime = Time.time + 1f;
                Slot2.StopRollingNextIcon();
                return;
            case 1: // stop 1st
                rollStage++;
                rollTime = Time.time + 1f;
                Slot1.StopRollingNextIcon();
                return;
            case 0: // pull handle
                rollStage++;
                rollTime = Time.time + 1f;
                HandleAnimation.Play("playAnim");
                myHandleAS.Play();
                return;
            default: // end
                UI_HUD.Instance.HideSlotMachine();
                GameController.Instance.myWUG.SlotMachine();
                isRolling = false;
                break;
        }
    }

    public void StartRoll()
    {
        Slot1.StartRolling();
        Slot2.StartRolling();
        Slot3.StartRolling();
        isRolling = true;
        rollTime = Time.time + 5f;
        rollStage = 0;
        rolledPrizes.Clear();
        HandleAnimation.Play("noAnim");
        myWinningAS.Stop();
    }

    public void AddPrizeFromRoll(LootItemScriptable prize)
    {
        rolledPrizes.Add(prize);
        rollTime = 1f;
    }
}
