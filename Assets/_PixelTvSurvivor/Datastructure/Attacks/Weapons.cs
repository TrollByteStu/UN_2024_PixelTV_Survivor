using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapons : ScriptableObject
{
    internal float LastShot;
    public WeaponType Type;
    public GameObject weapon;
    public GameObject Bullets;
    public AnimationCurve weaponCurve;
    public float AttackSpeed;
    public float AttackDamage;

    public enum WeaponType
    {
        Aura,
        Bullet,
        Homing
    }

    public void Aura(Vector3 position,float timeOfAttack,float area)
    {
        LastShot = timeOfAttack;
        foreach( RaycastHit Hit in Physics.SphereCastAll(position, area, Vector3.zero))
        {

        }
        
    }
    public void Bullet(float timeOfAttack)
    {
        LastShot = timeOfAttack;
    }
    public void Homing(float timeOfAttack)
    {
        LastShot = timeOfAttack;
    }
}
