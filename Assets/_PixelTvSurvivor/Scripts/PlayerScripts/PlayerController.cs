using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public PlayerStats Stats = new("Player",100,100,0.1f,0,10);
    
    public WeaponStats[] WeaponsList;

    private Vector3 thisPosition;
    private Vector3 lastPosition;

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
        
    }

    private Vector2 MoveDirection;
    private Vector3 FinalMovement;
    // moves Player 
    void Movement()
    {
        lastPosition = thisPosition;
        thisPosition = transform.position;
        FinalMovement = MoveDirection.ConvertTo<Vector3>() * Stats.MoveSpeed * Stats.MoveSpeedModifier * Time.deltaTime;
        transform.position += FinalMovement.ConvertTo<Vector3>();
        //BackgroundSprite.material.SetVector("_Offset",BackgroundSprite.material.GetVector("_Offset") + FinalMovement / 4);
    }

    private Vector2 AimDirection = new(1,0);
    void Attacks()
    {
        for (int i = 0; i < WeaponsList.Length; i++) 
        {
            if (WeaponsList[i].LastShot < Time.time )
            {
                WeaponsList[i].LastShot = Time.time + WeaponsList[i].Weapon.LevelStats[WeaponsList[i].Level].AttackSpeed * Stats.CooldownModifier;
                switch (WeaponsList[i].Weapon.Type)
                {
                    case Weapons.WeaponType.Aura:
                        WeaponsList[i].Weapon.Aura(WeaponsList[i].Level, transform.position, Stats.Area,Stats.DamageModifier);
                        break;
                    case Weapons.WeaponType.Bullet:
                        WeaponsList[i].Weapon.Bullet(WeaponsList[i].Level,transform.position, AimDirection, Stats.DamageModifier);
                        break;
                    case Weapons.WeaponType.Homing:
                        WeaponsList[i].Weapon.Homing(WeaponsList[i].Level,transform.position, Stats.DamageModifier);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    void PickupXP()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, 5, Vector3.forward))
        {
            if (hit.transform.GetComponent<XpOrb>() != null)
                hit.transform.GetComponent<XpOrb>().MoveToPlayer = true;
        }
    }
    public void PlayerTakesDamage(float damage)
    {
        // play sound of player moaning/complaining?
        Stats.Health -= math.clamp( damage - Stats.Armor,0,999999999);
        if (Stats.Health < 0) GameController.Instance.PlayerIsDead();
    }
    public void AddPoints(int Points)
    {
        Stats.Points += Points;
    }
    public void AddXp(float xp)
    {
        Stats.Xp += xp * Stats.XpModifier;
        if (Stats.Xp > math.pow(Stats.Level * 10, 1.4f))
        {
            LevelUp();
            UI_HUD.Instance.UpgradeShow();
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
            if (MoveDirection.x < 0)
                transform.localScale = new Vector3(-.5f, .5f, 1);
            else
                transform.localScale = new Vector3(.5f, .5f, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Player hit something with tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Walls")
        {
            transform.position -= ((transform.position-lastPosition)*5);
        }
    }

}
