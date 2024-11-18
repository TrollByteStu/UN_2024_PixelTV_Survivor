using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;
using System.Drawing;

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
    public AudioSource LooseSoundAS;
    public GameObject Instructions;

    private bool isRolling = false;
    private float rollTime = 0f;
    private int rollStage = 0;
    public List<LootItemScriptable> rolledPrizes;

    private AudioSource myHandleAS;
    private AudioSource myWinningAS;

    private LootItemScriptable determinedPrize;

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
        if (rollTime > Time.timeSinceLevelLoad) return;
        switch( rollStage )
        { // the action order
            case 4: // show all 3,play sound?, maybe particle effects?
                rollStage++;
                if (determinedPrize != null)
                { // winner
                    myWinningAS.time = 0;
                    myWinningAS.Play();
                } else { // looser
                    LooseSoundAS.time = 0;
                    LooseSoundAS.Play();
                }
                determinedPrize = null;
                rollTime = Time.timeSinceLevelLoad + 1f;
                return;
            case 3: // stop 3rd
                rollStage++;
                rollTime = Time.timeSinceLevelLoad + 1f;
                if (determinedPrize != null)
                    Slot3.StopRollingDemandPrize(determinedPrize);
                else
                    Slot3.StopRollingNextIcon();
                return;
            case 2: // stop 2nd
                rollStage++;
                rollTime = Time.timeSinceLevelLoad + 1f;
                if (determinedPrize != null)
                    Slot2.StopRollingDemandPrize(determinedPrize);
                else
                    Slot2.StopRollingNextIcon();
                return;
            case 1: // stop 1st
                rollStage++;
                rollTime = Time.timeSinceLevelLoad + 1f;
                if (determinedPrize != null)
                    Slot1.StopRollingDemandPrize(determinedPrize);
                else
                    Slot1.StopRollingNextIcon();
                return;
            case 0: // waiting for pull handle
                PullHandle();
                return;
            default: // end
                UI_HUD.Instance.HideSlotMachine();
                GameController.Instance.myWUG.FinishedAnimation();
                isRolling = false;
                //if ( determinedPrize != null)
                //{ // we have to give a specific prize
                //    GivePrizeToPlayer(determinedPrize);
                //    determinedPrize = null;
                //} else { // give them hot random garbage(will add a bit to check if all 3 are the same at some point, for now they get garbage)
                //    GivePrizeToPlayer(ConsolationPrizes[UnityEngine.Random.Range(0, ConsolationPrizes.Length)]);
                //}
                break;
        }
    }

    public void Reset()
    {
        isRolling = true;
        rollTime = Time.timeSinceLevelLoad + 6f;
        rollStage = 0;
        rolledPrizes.Clear();
        HandleAnimation.Play("noAnim");
        myWinningAS.Stop();
        Instructions.SetActive(true);
    }
    public void PullHandle()
    {
        Debug.Log("pulled");
        if (!isRolling) return;
        if (rollStage != 0) return;
        Debug.Log("pull success");
        rollStage++;
        rollTime = Time.timeSinceLevelLoad + 1f;
        HandleAnimation.Play("playAnim");
        myHandleAS.Play();
        Slot1.StartRolling();
        Slot2.StartRolling();
        Slot3.StartRolling();
        Instructions.SetActive(false);
    }

    public void ResetDemandPrize(LootItemScriptable prize)
    { // get a specific weapon, if we have to make a list of when each is available
        Reset();
        determinedPrize = prize;
    }

    public void AddPrizeFromRoll(LootItemScriptable prize)
    {
        rolledPrizes.Add(prize);
        rollTime = Time.timeSinceLevelLoad + 1f;
    }

    public void GivePrizeToPlayer(LootItemScriptable prize)
    {
        PlayerController PlayerRef = GameController.Instance.PlayerReference;

        // actually upgrade stats
        if (prize.Stats.HealthIncrease > 0) PlayerRef.Stats.Health += prize.Stats.HealthIncrease;
        if (PlayerRef.Stats.Health > PlayerRef.Stats.MaxHealth) PlayerRef.Stats.Health = PlayerRef.Stats.MaxHealth;
        if (prize.Stats.MaxHealthIncrease > 0) PlayerRef.Stats.MaxHealth += prize.Stats.MaxHealthIncrease;
        if (prize.Stats.HealthModifierIncrease > 0) PlayerRef.Stats.HealthModifier += prize.Stats.HealthModifierIncrease;
        if (prize.Stats.RecoveryIncrease > 0) PlayerRef.Stats.Recovery += prize.Stats.RecoveryIncrease;
        if (prize.Stats.ArmorIncrease > 0) PlayerRef.Stats.Armor += prize.Stats.ArmorIncrease;
        if (prize.Stats.MoveSpeedIncrease > 0) PlayerRef.Stats.MoveSpeed += prize.Stats.MoveSpeedIncrease;
        if (prize.Stats.MoveSpeedModifierIncrease > 0) PlayerRef.Stats.MoveSpeedModifier += prize.Stats.MoveSpeedModifierIncrease;
        if (prize.Stats.DamageModifierIncrease > 0) PlayerRef.Stats.DamageModifier += prize.Stats.DamageModifierIncrease;
        if (prize.Stats.CooldownModifierIncrease > 0) PlayerRef.Stats.CooldownModifier += prize.Stats.CooldownModifierIncrease;

        // actually add/upgrade weapons
        if (prize.givenWeapon.Weapon != null) PlayerRef.AddWeapon(prize.givenWeapon);

        // popup text
        var popup = Instantiate(GameController.Instance.PopupTextPrefab, transform.position, Quaternion.identity).GetComponent<PopupTextHandler>();
        popup.TextContent = prize.PopupText;
        popup.TextColor = prize.PopupColor;
        popup.playThis = prize.pickupAudio;
    }
}
