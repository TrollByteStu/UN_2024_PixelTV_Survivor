using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SatelliteBase : MonoBehaviour
{
    public WeaponBase Weapon;
    public void SetUp(WeaponBase weaponBase)
    {
        Weapon = weaponBase;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<Enemy_Main>() != null)
            {
                collision.GetComponent<Enemy_Main>().EnemyTakesDamage(FindDamage());

            }
        }
    }

    float FindDamage()
    {
        foreach (WeaponStats ws in GameController.Instance.PlayerReference.WeaponsArray)
        {
            if (ws.Weapon == Weapon)
            {
                return ws.Weapon.GetDamage(ws.Level);
            }    
        }
        Debug.LogError("could not find weapon");
        return 0;
    }
}
