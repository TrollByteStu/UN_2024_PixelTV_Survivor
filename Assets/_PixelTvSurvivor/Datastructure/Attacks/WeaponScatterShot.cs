using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponScatterShot", menuName = "ScriptableObjects/WeaponTypes/ScatterShot", order = 1)]
public class WeaponScatterShot : WeaponBase
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
        public float bulletSpread;
        public float AttackSpeed;
        public float AttackDamage;
        public int ShootQuantity;
    }
    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
    {

        
        for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
        {
            float SpreadAngle = math.radians(LevelStats[level].bulletSpread / 2 ) / LevelStats[level].ShootQuantity * i;
            SpreadAngle -= math.radians(LevelStats[level].bulletSpread/2) / 2 ;
            Vector3 SpreadDirection = new Vector3 (math.asin(direction.x),math.acos(direction.y),0) + new Vector3(SpreadAngle, SpreadAngle, 0);
            SpreadDirection = new Vector3(math.sin(SpreadDirection.x),math.cos(SpreadDirection.y),0);
            SpreadDirection.Normalize();

            if (HasCurve)
            {
                if (FlipCurve)
                    Instantiate(BulletPrefab, playerPosition + SpreadDirection, Quaternion.identity).AddComponent<BulletBasic>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, SpreadDirection, Curve, Convert.ToBoolean(i % 2), bulletSprite);
                else
                    Instantiate(BulletPrefab, playerPosition + SpreadDirection, Quaternion.identity).AddComponent<BulletBasic>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, SpreadDirection, Curve, false, bulletSprite);
            }
            else
            {
                Instantiate(BulletPrefab, playerPosition + SpreadDirection, Quaternion.identity).AddComponent<BulletBasic>()
                .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, SpreadDirection, null, false, bulletSprite);
            }


        }
    }



}
