using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSpiral", menuName = "ScriptableObjects/WeaponTypes/Spiral", order = 1)]
public class WeaponSpiral : WeaponBase
{
    public GameObject BulletPrefab;
    public Sprite bulletSprite;
    public Spin SpinDirection;
    public List<WeaponStats> LevelStats;
    [Serializable]
    public struct WeaponStats
    {
        public float ClimbSpeed;
        public float bulletSpeed;
        public float AttackSpeed;
        public float AttackDamage;
        public float ShootQuantity;
    }
    public enum Spin
    {
        Left,
        Right,
        Alternate
    }

    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
    {
        for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
        {
            switch (SpinDirection)
            {
                case Spin.Left:
                    Instantiate(BulletPrefab, playerPosition, Quaternion.identity).AddComponent<BulletSpiral>()
                        .Setup(i / LevelStats[level].ShootQuantity * math.PI * 2,-1, LevelStats[level].ClimbSpeed, LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, playerPosition);
                    break;
                case Spin.Right:
                    Instantiate(BulletPrefab, playerPosition, Quaternion.identity).AddComponent<BulletSpiral>()
                        .Setup(i / LevelStats[level].ShootQuantity * math.PI * 2,1, LevelStats[level].ClimbSpeed, LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, playerPosition);
                    break;
                case Spin.Alternate:
                    Instantiate(BulletPrefab, playerPosition, Quaternion.identity).AddComponent<BulletSpiral>()
                        .Setup(i / LevelStats[level].ShootQuantity * math.PI * 2,math.pow(-1,i % 2), LevelStats[level].ClimbSpeed, LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, playerPosition);
                    break;
            }
        }
    }

    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
}
