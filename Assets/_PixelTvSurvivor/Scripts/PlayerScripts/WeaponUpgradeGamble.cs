using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class WeaponUpgradeGamble : MonoBehaviour
{
    [Range(0f, 100f)]
    public float ChanceToMiss;

    public bool Roll;

    public PlayerController Player;
    public List<WeaponBase> WeaponChances;

    private void Start()
    {
        Player = GameController.Instance.PlayerReference;
    }

    //private void Update()
    //{
    //    if (Roll)
    //    {
    //        Roll = false;
    //        SlotMachine();
    //    }
    //}

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
        }
        else
        {
            WeaponBase weapon = RollWeapon();
            
            if (Player.DoesPlayerHaveWeapon(weapon))
            {
                if (Player.WeaponUpgardeble(weapon))
                {
                    Player.UpgradeWeapon(weapon);
                    if (!Player.WeaponUpgardeble(weapon))
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
                if (!Player.WeaponUpgardeble(weapon))
                    WeaponChances.Remove(weapon);
            }
        }
    }

    bool RollFailed()
    {
        return Random.Range(0, 100) < ChanceToMiss;
    }
    WeaponBase RollWeapon()
    {
        return WeaponChances[Random.Range(0, WeaponChances.Count - 1)];
    }


}
