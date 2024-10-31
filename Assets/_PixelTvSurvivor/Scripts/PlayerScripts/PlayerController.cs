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
    [Header("Player Stats")]
    public PlayerStats Stats = new("Player",100,100,0.1f,0,10);
    public WeaponStats[] WeaponsArray;

    [Header("Santa Sprite Layered")]
    public GameObject Santa_Body_Idle;
    public GameObject Santa_Body_Walk;
    public GameObject Santa_Gun_Idle;
    public GameObject Santa_Gun_Walk;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.Instance.SetupForGame( this );

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
                WeaponsArray[i].Weapon.Attack(WeaponsArray[i].Level, transform, AimDirection, Stats);
            
            }
        }
    }
    void PickupXP()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, 5, Vector3.forward))
        {
            if (hit.transform.CompareTag("PickupAble"))
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
        if (Stats.Health <= 0) GameController.Instance.PlayerIsDead();
    }
    public void AddPoints(int Points, float time)
    {
        Stats.Points += Points;
        Stats.TimeUntilDeath += time;
        GameController.Instance.gamePoints = Stats.Points;
    }
    public void AddXp(float xp)
    {
        Stats.Xp += xp * Stats.XpModifier;
        if ((int)Stats.Xp >= (int)math.pow(Stats.Level * 10, 1.4f))
        {
            LevelUp();
            //UI_HUD.Instance.UpgradeShow();
        }
    }
    public void LevelUp()
    {
        Stats.Level++;
        Stats.Xp = 0;
        //GameController.Instance.myWUG.SlotMachine();
        UI_HUD.Instance.ShowSlotMachine();
    }

    // Event called by Player Input Component on press and release of move keybind
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>().normalized;
        if (MoveDirection != Vector2.zero)
        { // is moving
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

            Santa_Body_Idle.SetActive(false);
            Santa_Body_Walk.SetActive(true);
            Santa_Gun_Idle.SetActive(false);
            Santa_Gun_Walk.SetActive(true);
        } else { // not moving
            Santa_Body_Idle.SetActive(true);
            Santa_Body_Walk.SetActive(false);
            Santa_Gun_Idle.SetActive(true);
            Santa_Gun_Walk.SetActive(false);
        }
    }

    public bool DoesPlayerHaveWeapon(WeaponBase checkWeapon)
    {
        foreach (WeaponStats lookForWeapon in WeaponsArray)
        {
            if (lookForWeapon.Weapon == checkWeapon) return true;
        }
        return false;
    }

    public void AddWeapon(WeaponBase addWeapon)
    {
        if (DoesPlayerHaveWeapon(addWeapon)) return;
        //adds new weapon to array
        WeaponsArray = WeaponsArray.Concat(new WeaponStats[] { new WeaponStats(addWeapon) }).ToArray();
    }

    public void AddWeapon(WeaponStats addWeapon)
    {
        if (DoesPlayerHaveWeapon(addWeapon.Weapon)) return;
        //adds new weapon to array
        WeaponsArray = WeaponsArray.Concat(new WeaponStats[] { addWeapon }).ToArray();
    }

    public void UpgradeWeapon(WeaponBase weapon)
    {
        if (WeaponUpgradeable(weapon))
            WeaponsArray[FindWeapon(weapon)].Level++;
    }

    int FindWeapon(WeaponBase weapon)
    {
        for (int i = 0; i < WeaponsArray.Length; i++)
        {
            if (WeaponsArray[i].Weapon == weapon)
                return i;
        }
        Debug.LogError("none found");
        return 99999999;
    }

    public bool WeaponUpgradeable(WeaponBase weapon)
    {
        return WeaponsArray[FindWeapon(weapon)].Level < weapon.GetMaxLevel();
    }

}
