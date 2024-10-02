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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attacks();
        print(math.pow(Stats.Level *10,1.4f));
    }

    private Vector2 MoveDirection;
    private Vector3 FinalMovement;
    // moves Player 
    void Movement()
    {
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
                WeaponsList[i].LastShot = Time.time + WeaponsList[i].Weapon.AttackSpeed * Stats.CooldownModifier;
                switch (WeaponsList[i].Weapon.Type)
                {
                    case Weapons.WeaponType.Aura:
                        WeaponsList[i].Weapon.Aura(transform.position, Stats.Area,Stats.DamageModifier);
                        break;
                    case Weapons.WeaponType.Bullet:
                        WeaponsList[i].Weapon.Bullet(transform.position, AimDirection);
                        break;
                    case Weapons.WeaponType.Homing:
                        WeaponsList[i].Weapon.Homing(transform.position);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    
    public void AddXp(float xp)
    {
        Stats.Xp += xp * Stats.XpModifier;
        if (Stats.Xp > math.pow(Stats.Level * 10, 1.4f))
            LevelUp();
    }
    public void LevelUp()
    {
        Stats.Level++;
    }
    // Event called by Player Input Component on press and release of move keybind
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
        if (MoveDirection != Vector2.zero)
        {
            AimDirection = MoveDirection;
        }
    }
}
