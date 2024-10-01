using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapons : ScriptableObject
{
    public WeaponType Type;
    public GameObject weapon;
    public Sprite BulletTexture;
    public GameObject Bullets;
    public AnimationCurve weaponCurve;
    public float bulletSpeed;
    public float AttackSpeed;
    public float AttackDamage;

    public enum WeaponType
    {
        Aura,
        Bullet,
        Homing
    }

    //does not work
    public void Aura(Vector3 position,float area)
    {
        foreach( RaycastHit Hit in Physics.SphereCastAll(position, 10, Vector3.zero,1))
        {

        }
        
    }
    public void Bullet(Vector3 position , Vector3 direction)
    {
        Instantiate(Bullets, position + direction,Quaternion.LookRotation(direction,Vector3.forward)).GetComponent<Bullets>().Setup(bulletSpeed,AttackDamage,direction,weaponCurve,BulletTexture);
    }
    public void Homing()
    {
       
    }
}
