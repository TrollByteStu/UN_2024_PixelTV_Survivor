using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "WeaponSpiral", menuName = "ScriptableObjects/WeaponTypes/Spiral", order = 1)]
public class WeaponSpiral : WeaponBase
{
    public GameObject BulletPrefab;
    public Sprite bulletSprite;
    public Spin SpinDirection;
    public List<WeaponStats> LevelStats = new List<WeaponStats> { new WeaponStats() };
    [Serializable]
    public struct WeaponStats
    {
        public float ClimbSpeed;
        public float bulletSpeed;
        public float AttackSpeed;
        public float AttackDamage;
        public float ShootQuantity;
        public float ShotDelay;

        public WeaponStats(int i)
        {
            ClimbSpeed = 3;
            bulletSpeed = 1;
            AttackSpeed = 1;
            AttackDamage = 10;
            ShootQuantity = 5;
            ShotDelay = 0;
        }
    }
    public enum Spin
    {
        Left,
        Right,
        Alternate
    }

    public async override void Attack(int level, Transform playerTransform, Vector3 direction, PlayerStats playerStats)
    {
        for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
        {
            switch (SpinDirection)
            {
                case Spin.Left:
                    Instantiate(BulletPrefab, playerTransform.position, Quaternion.identity).AddComponent<BulletSpiral>()
                        .Setup(i / LevelStats[level].ShootQuantity * math.PI * 2,-1, LevelStats[level].ClimbSpeed, LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, playerTransform.position);
                    break;
                case Spin.Right:
                    Instantiate(BulletPrefab, playerTransform.position, Quaternion.identity).AddComponent<BulletSpiral>()
                        .Setup(i / LevelStats[level].ShootQuantity * math.PI * 2,1, LevelStats[level].ClimbSpeed, LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, playerTransform.position);
                    break;
                case Spin.Alternate:
                    Instantiate(BulletPrefab, playerTransform.position, Quaternion.identity).AddComponent<BulletSpiral>()
                        .Setup(i / LevelStats[level].ShootQuantity * math.PI * 2,math.pow(-1,i % 2), LevelStats[level].ClimbSpeed, LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, playerTransform.position);
                    break;
            }
            await Awaitable.WaitForSecondsAsync(LevelStats[level].ShotDelay);
        }
    }

    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
    public override int GetMaxLevel()
    {
        return LevelStats.Count - 1;
    }
}
