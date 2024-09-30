using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerStats Stats = new("Player",100,100,0.1f,0,10);
    public List<Weapons> WeaponsList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attacks();
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

    void Attacks()
    {
        foreach (var weapon in WeaponsList) 
        {
            if (weapon.LastShot < Time.time )
            {
                switch (weapon.Type)
                {
                    case Weapons.WeaponType.Aura:
                        weapon.Aura(transform.position , Time.time + weapon.AttackSpeed * Stats.CooldownModifier , Stats.Area);
                        break;
                    case Weapons.WeaponType.Bullet:
                        weapon.Bullet(Time.time + weapon.AttackSpeed * Stats.CooldownModifier);
                        break;
                    case Weapons.WeaponType.Homing:
                        weapon.Homing(Time.time + weapon.AttackSpeed * Stats.CooldownModifier);
                        break;
                }
            }
        }
    }
    
    // Event called by Player Input Component on press and release of move keybind
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }
}
