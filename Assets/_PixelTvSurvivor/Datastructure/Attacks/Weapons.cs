using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

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
    public float AttackAOE = 4;

    public enum WeaponType
    {
        Aura,
        Bullet,
        Homing
    }

    public void Aura(Vector3 position,float area,float damageModifier)
    {
        foreach ( RaycastHit2D Hit in Physics2D.CircleCastAll(position, AttackAOE * area, Vector3.forward))
        {
            if (Hit.transform.CompareTag("Enemy"))
            {
                Hit.transform.GetComponent<Enemy_Main>().Damage(AttackDamage);
            }
        }
    }
    public void Bullet(Vector3 position , Vector3 direction)
    {
        Instantiate(Bullets, position + direction,Quaternion.LookRotation(direction,Vector3.forward)).GetComponent<Bullets>()
            .Setup(bulletSpeed,AttackDamage,direction,weaponCurve,BulletTexture,Type);
    }
    public void Homing(Vector3 position)
    {
        List<RaycastHit2D> Hits = Physics2D.CircleCastAll(position, 15, Vector3.forward).ToList<RaycastHit2D>();
        List<RaycastHit2D> RemoveList = new List<RaycastHit2D>();
        foreach (RaycastHit2D Hit in Hits)
        {
            if (!Hit.transform.CompareTag("Enemy"))
            {
                RemoveList.Add(Hit);
            }
        }

        // removes all none enemy tag hits from list
        Hits.RemoveAll(h => RemoveList.Contains(h));
        // sorts list based on distance from player
        Hits.Sort((h1, h2) => (h1.transform.position - position).magnitude.CompareTo((h2.transform.position - position).magnitude));
        if (Hits.Count > 0 ) 
            Instantiate(Bullets, position, Quaternion.LookRotation(Hits[0].transform.position.normalized, Vector3.forward)).GetComponent<Bullets>()
                .Setup(bulletSpeed, AttackDamage, Hits[0].transform.gameObject,weaponCurve,BulletTexture,Type);
    }
}
