using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponUpgradeGamble : MonoBehaviour
{

    public float ChanceToHit = 100;

    public float ChanceToHitWeapon = 100;

    public float SuccsesfullWeaponHits = 0;

    public bool Roll;

    public PlayerController Player;
    public List<LootItemScriptable> UpgadeChances;
    public List<LootItemScriptable> WeaponChances;
    public LootItemScriptable[] StartingWeapons;

    private bool OngoingAttempt;

    private int ResultType;
    private LootItemScriptable Item;
    public void StartAttempt()
    {
        // lost its reference and crashed, getting it again
        Player = GameController.Instance.PlayerReference;
        if (OngoingAttempt)
            return;
        if (Player.Stats.Coins < 10)
            return;

        OngoingAttempt = true;
        Player.Stats.Coins -= 10;
        SlotMachine();
        //UI_HUD.Instance.ShowSlotMachine();
    }
    void SlotMachine()
    {

        if (RollFailed())
        {
            ChanceToHit = math.clamp(ChanceToHit + 10, 0, 100);
            ChanceToHitWeapon = math.clamp(ChanceToHitWeapon + 4 * 10, 0, 100);
            UI_HUD.Instance.ShowSlotMachine();
            return;
        }
        
        if (RollForWeapon())
        {
            // first roll
            if (ChanceToHitWeapon > 100)
            {
                ChanceToHit = 20;
                ChanceToHitWeapon = 10;
                Item = StartingWeapons[Random.Range(0, StartingWeapons.Length - 1)];
                ResultType = 1;
                UI_HUD.Instance.ShowSlotMachineDemand(Item);
                //Player.AddWeapon(startingWeapon.givenWeapon.Weapon);
                return;
            }

            ChanceToHit = 20;
            ChanceToHitWeapon = 10;

            Item = RollWeapon();
            UI_HUD.Instance.ShowSlotMachineDemand(Item);
            ResultType = 1;
        
            //if (Player.DoesPlayerHaveWeapon(weapon.givenWeapon.Weapon))
            //{
            //    if (Player.WeaponUpgradeable(weapon.givenWeapon.Weapon))
            //    {
            //        Player.UpgradeWeapon(weapon.givenWeapon.Weapon);
            //        if (!Player.WeaponUpgradeable(weapon.givenWeapon.Weapon))
            //            WeaponChances.Remove(weapon);
            //    }
            //    else
            //    {
            //        WeaponChances.Remove(weapon);
            //    }
            //}
            //else
            //{
            //    Player.AddWeapon(weapon.givenWeapon.Weapon);
            //    if (!Player.WeaponUpgradeable(weapon.givenWeapon.Weapon))
            //        WeaponChances.Remove(weapon);
            //}
        }
        else
        {
            ChanceToHit = 20;
            ChanceToHitWeapon = math.clamp(ChanceToHitWeapon + 4, 0, 100);
            if (UpgadeChances.Count == 0)
                return;
            Item = RollUpgrade();
            UI_HUD.Instance.ShowSlotMachineDemand(Item);
            ResultType = 2;
        }
    }

    public void FinishedAnimation()
    {
        OngoingAttempt = false;

        switch (ResultType)
        {
            case 1:
                if (Player.DoesPlayerHaveWeapon(Item.givenWeapon.Weapon))
                {
                    if (Player.WeaponUpgradeable(Item.givenWeapon.Weapon))
                    {
                        Player.UpgradeWeapon(Item.givenWeapon.Weapon);
                        if (!Player.WeaponUpgradeable(Item.givenWeapon.Weapon))
                            WeaponChances.Remove(Item);
                    }
                    else
                    {
                        WeaponChances.Remove(Item);
                    }
                }
                else
                {
                    Player.AddWeapon(Item.givenWeapon.Weapon);
                    if (!Player.WeaponUpgradeable(Item.givenWeapon.Weapon))
                        WeaponChances.Remove(Item);
                }
                break;

            case 2:
                GiveUpgrade(Item);
                break;
        }
    }

    bool RollFailed()
    {
        return Random.Range(0, 100) > ChanceToHit;
    }
    bool RollForWeapon()
    {
        if (WeaponChances.Count == 0) 
            return false;
        return Random.Range(0,100) < ChanceToHitWeapon;
    }
    LootItemScriptable RollWeapon()
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
    }

}
