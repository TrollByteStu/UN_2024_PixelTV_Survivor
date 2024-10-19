using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    public PlayerStats Stats = new("Player",100,100,0.1f,0,10);
    
    public WeaponStats[] WeaponsArray;


    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attacks();
        PickupXP();
        StatsAndStatusHandler();
    }

    private Vector2 MoveDirection;
    private Vector3 FinalMovement;
    // moves Player 
    void Movement()
    {
        FinalMovement = Stats.MoveSpeed * Stats.MoveSpeedModifier * Time.deltaTime * MoveDirection.ConvertTo<Vector3>();
        // raycast check for walls in move direction
        foreach (RaycastHit2D Hit in Physics2D.LinecastAll(transform.position,transform.position + FinalMovement))
        {
            if (Hit.collider.CompareTag("Walls"))
            {
                FinalMovement *= Hit.distance;
                break;
            }
        }
        transform.position += FinalMovement;
    }

    private Vector2 AimDirection = new(1,0);
    void Attacks()
    {
        for (int i = 0; i < WeaponsArray.Length; i++) 
        {
            if (WeaponsArray[i].LastShot < Time.time )
            {
                WeaponsArray[i].LastShot = Time.time + WeaponsArray[i].Weapon.GetAttackSpeed(WeaponsArray[i].Level) * Stats.CooldownModifier;
                WeaponsArray[i].Weapon.Attack(WeaponsArray[i].Level, transform.position, AimDirection, Stats);
            
            }
        }
    }
    void PickupXP()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, 5, Vector3.forward))
        {
            if (hit.transform.tag == "PickupAble")
            {
                if (hit.transform.GetComponent<XpOrb>() != null)
                    hit.transform.GetComponent<XpOrb>().Pickup();
                if (hit.transform.GetComponent<ItemHandler>() != null)
                    hit.transform.GetComponent<ItemHandler>().Pickup();
            }
        }
    }
    void StatsAndStatusHandler()
    {
        // Time
        Stats.TimeUntilDeath -= Time.deltaTime;

        // Stats
        Stats.Health += Time.deltaTime * Stats.Recovery;
        if (Stats.Health > Stats.MaxHealth) Stats.Health = Stats.MaxHealth;

        // Statuses
        if (Stats.TimeUntilDeath <= 0) GameController.Instance.PlayerIsDead();
    }

    public void PlayerTakesDamage(float damage)
    {
        // play sound of player moaning/complaining?
        Stats.Health -= math.clamp( damage - Stats.Armor,0,999999999);
        if (Stats.Health < 0) GameController.Instance.PlayerIsDead();
    }
    public void AddPoints(int Points, float time)
    {
        Stats.Points += Points;
        Stats.TimeUntilDeath += time;
    }
    public void AddXp(float xp)
    {
        Stats.Xp += xp * Stats.XpModifier;
        if (Stats.Xp > math.pow(Stats.Level * 10, 1.4f))
        {
            LevelUp();
            //UI_HUD.Instance.UpgradeShow();
        }
    }
    public void LevelUp()
    {
        Stats.Level++;
        Stats.Xp = 0;
    }

    // Event called by Player Input Component on press and release of move keybind
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
        if (MoveDirection != Vector2.zero)
        {
            AimDirection = MoveDirection;
            foreach (WeaponStats weaponStats in WeaponsArray)
            {
                weaponStats.Weapon.SetAim(AimDirection);
            }

            // Flip player sprite to face movement
            if (MoveDirection.x < 0)
                transform.localScale = new Vector3(-.5f, .5f, 1);
            else
                transform.localScale = new Vector3(.5f, .5f, 1);
        }
    }

    public bool DoesPlayerHaveWeapon(WeaponStats checkWeapon)
    {
        foreach (WeaponStats lookForWeapon in WeaponsArray)
        {
            if (lookForWeapon.Weapon.WeaponName == checkWeapon.Weapon.WeaponName) return true;
        }
        return false;
    }

    public void AddWeapon(WeaponStats addWeapon)
    {
        if (DoesPlayerHaveWeapon(addWeapon)) return;
        WeaponStats[] TmpArray = new WeaponStats[WeaponsArray.Length +1];
        WeaponsArray.CopyTo(TmpArray, 0);
        WeaponsArray = TmpArray;
        WeaponsArray[WeaponsArray.Length-1] = addWeapon;
    }
}
