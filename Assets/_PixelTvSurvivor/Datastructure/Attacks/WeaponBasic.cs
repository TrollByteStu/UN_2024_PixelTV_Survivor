using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponBasic", menuName = "ScriptableObjects/WeaponTypes/Basic", order = 1)]
public class WeaponBasic : WeaponBase
{
    public GameObject BulletPrefab;
    public Sprite bulletSprite;
    public List<WeaponStats> LevelStats;
    public bool HasCurve;
    public AnimationCurve Curve;
    public bool FlipCurve;

    [Serializable]
    public struct WeaponStats
    {
        public float bulletSpeed;
        public float AttackSpeed;
        public float AttackDamage;
        public float ShootQuantity;

        public WeaponStats(int i)
        {
            bulletSpeed = 10;
            AttackSpeed = 1;
            AttackDamage = 10;
            ShootQuantity = 1;
        }
    }
    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
    {
        for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
        {
            if (HasCurve)
            {
                if (FlipCurve)
                    Instantiate(BulletPrefab, playerPosition + direction, Quaternion.identity).AddComponent<BulletBasic>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, direction, Curve, Convert.ToBoolean(i % 2), bulletSprite);
                else
                    Instantiate(BulletPrefab, playerPosition + direction, Quaternion.identity).AddComponent<BulletBasic>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, direction, Curve, false, bulletSprite);
            }
            else
            {
                Instantiate(BulletPrefab, playerPosition + direction, Quaternion.identity).AddComponent<BulletBasic>()
                .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, direction, null, false, bulletSprite);
            }


        }
    }
    public override int GetMaxLevel()
    {
        return LevelStats.Count - 1;
    }
}
