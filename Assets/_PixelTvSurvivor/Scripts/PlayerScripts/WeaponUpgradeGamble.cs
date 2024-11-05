using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponUpgradeGamble : MonoBehaviour
{
    [Range(0f, 100f)]
    public float ChanceToHit = 100;
    public float ChanceToHitWeapon = 100;

    public float SuccsesfullWeaponHits = 0;

    public bool Roll;

    public PlayerController Player;
    public List<LootItemScriptable> UpgadeChances;
    public List<WeaponBase> WeaponChances;

    private void Start()
    {
        Player = GameController.Instance.PlayerReference;

    }

    private void Update()
    {
        if (Roll)
        {
            Roll = false;
            SlotMachine();
        }
    }

    public void ShowSlotMachine()
    {
        //UI_HUD.Instance.Section_SlotMachine;
    }
    public void SlotMachine()
    {
        // lost its reference and crashed, getting it again
        Player = GameController.Instance.PlayerReference;

        if (WeaponChances.Count == 0)
        {
            return;
        }

        if (RollFailed())
        {
            print("failed");
            ChanceToHit = math.clamp(ChanceToHit + 10, 0, 100);
            ChanceToHitWeapon = math.clamp(ChanceToHitWeapon + 4 * 10, 0, 100);
            return;
        }
        
        if (RollForWeapon())
        {
            ChanceToHit = 20;
            ChanceToHitWeapon = 10;

            WeaponBase weapon = RollWeapon();
        
            if (Player.DoesPlayerHaveWeapon(weapon))
            {
                if (Player.WeaponUpgradeable(weapon))
                {
                    Player.UpgradeWeapon(weapon);
                    if (!Player.WeaponUpgradeable(weapon))
                        WeaponChances.Remove(weapon);
                }
                else
                {
                    WeaponChances.Remove(weapon);
                }
            }
            else
            {
                Player.AddWeapon(weapon);
                if (!Player.WeaponUpgradeable(weapon))
                    WeaponChances.Remove(weapon);
            }
        }
        else
        {
            ChanceToHit = 20;
            ChanceToHitWeapon = math.clamp(ChanceToHitWeapon + 4, 0, 100);
            GiveUpgrade(RollUpgrade());
        }
    }

    bool RollFailed()
    {
        return Random.Range(0, 100) > ChanceToHit;
    }
    bool RollForWeapon()
    {
        return Random.Range(0,100) > ChanceToHitWeapon;
    }
    WeaponBase RollWeapon()
    {
        return WeaponChances[Random.Range(0, WeaponChances.Count - 1)];
    }

    LootItemScriptable RollUpgrade()
    {
        return UpgadeChances[Random.Range(0,UpgadeChances.Count - 1)];
    }

    void GiveUpgrade(LootItemScriptable item)
    {
        if (item.Stats.HealthIncrease > 0) Player.Stats.Health += item.Stats.HealthIncrease;
        if (item.Stats.MaxHealthIncrease > 0) Player.Stats.MaxHealth += item.Stats.MaxHealthIncrease;
        if (item.Stats.HealthModifierIncrease > 0) Player.Stats.HealthModifier +=    item.Stats.HealthModifierIncrease;
        if (item.Stats.RecoveryIncrease > 0) Player.Stats.Recovery += item.Stats.RecoveryIncrease;
        if (item.Stats.ArmorIncrease > 0) Player.Stats.Armor += item.Stats.ArmorIncrease;
        if (item.Stats.MoveSpeedIncrease > 0) Player.Stats.MoveSpeed += item.Stats.MoveSpeedIncrease;
        if (item.Stats.MoveSpeedModifierIncrease > 0) Player.Stats.MoveSpeedModifier += item.Stats.MoveSpeedModifierIncrease;
        if (item.Stats.DamageModifierIncrease > 0) Player.Stats.DamageModifier += item.Stats.DamageModifierIncrease;
        if (item.Stats.CooldownModifierIncrease > 0) Player.Stats.CooldownModifier += item.Stats.CooldownModifierIncrease;
        if (item.Stats.XpModifierIncrease > 0) Player.Stats.XpModifier += item.Stats.XpModifierIncrease;
    }


}
