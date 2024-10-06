using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 1)]
public class Weapons : ScriptableObject
{
    public WeaponType Type;
    public GameObject weapon;
    public Sprite BulletTexture;
    public GameObject BulletsPrefab;
    public AnimationCurve weaponCurve;
    public bool FlipCurve;
    public List<WeaponStats> LevelStats;


    public enum WeaponType
    {
        Aura,
        Bullet,
        Homing
    }
    [Serializable]
    public struct WeaponStats
    {
        public float bulletSpeed;
        public float AttackSpeed;
        public float AttackDamage;
        public float AttackAOE;
        public float ShootQuantity;
    }

    public void Aura(int level,Vector3 playerPosition, float area,float damageModifier)
    {
        foreach ( RaycastHit2D Hit in Physics2D.CircleCastAll(playerPosition, LevelStats[level].AttackAOE * area, Vector3.forward))
        {
            if (Hit.transform.CompareTag("Enemy"))
            {
                Hit.transform.GetComponent<Enemy_Main>().EnemyTakesDamage(LevelStats[level].AttackDamage * damageModifier);
            }
        }
    }
    public void Bullet(int level, Vector3 playerPosition, Vector3 direction, float damageModifier)
    {
        for (int i = 0; i < LevelStats[level].ShootQuantity; i++) 
        {
            if (FlipCurve) 
                Instantiate(BulletsPrefab, playerPosition + direction,Quaternion.identity).GetComponent<Bullets>()
                .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * damageModifier,direction,weaponCurve,Convert.ToBoolean(i % 2),BulletTexture,Type);
            else
                Instantiate(BulletsPrefab, playerPosition + direction, Quaternion.identity).GetComponent<Bullets>()
                .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * damageModifier, direction, weaponCurve, false, BulletTexture, Type);

        }
    }
    public void Homing(int level, Vector3 playerPosition , float damageModifier)
    {
        List<RaycastHit2D> Hits = Physics2D.CircleCastAll(playerPosition, 15, Vector3.forward).ToList<RaycastHit2D>();
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
        Hits.Sort((h1, h2) => (h1.transform.position - playerPosition).magnitude.CompareTo((h2.transform.position - playerPosition).magnitude));

        if (Hits.Count > 0 )
        {
            for (int i = 0; i < LevelStats[level].ShootQuantity && i !< Hits.Count ; i++)
            {
                if (FlipCurve)
                    Instantiate(BulletsPrefab, playerPosition, Quaternion.identity).GetComponent<Bullets>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * damageModifier, Hits[i].transform.gameObject,weaponCurve, Convert.ToBoolean(i % 2), BulletTexture,Type);
                else
                    Instantiate(BulletsPrefab, playerPosition, Quaternion.identity).GetComponent<Bullets>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * damageModifier, Hits[i].transform.gameObject, weaponCurve, false, BulletTexture, Type);
            }
        }
    }
}
