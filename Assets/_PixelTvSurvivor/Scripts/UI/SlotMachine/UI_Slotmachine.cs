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

    // Start is called before the first frame update
    void Start() {
        Slot1.Setup(this,11f);
        Slot2.Setup(this,22f);
        Slot3.Setup(this,33f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isRolling) return;
        if (rollTime > Time.time) return;
        switch( rollStage )
        { // the action order
            case 0: // pull handle
                rollStage++;
                rollTime = Time.time + 1f;
                HandleAnimation.Play("playAnim");
                break;
            default: // end
                UI_HUD.Instance.HideSlotMachine();
                GameController.Instance.myWUG.SlotMachine();
                isRolling = false;
                break;
        }
    }

    public void StartRoll()
    {
        Debug.Log("Start roll");
        Slot1.StartRolling();
        Slot2.StartRolling();
        Slot3.StartRolling();
        isRolling = true;
        rollTime = Time.time + 5f;
        rollStage = 0;
    }

}
