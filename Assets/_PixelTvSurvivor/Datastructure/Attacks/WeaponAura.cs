using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponAura", menuName = "ScriptableObjects/WeaponTypes/Aura", order = 3)]
public class WeaponAura : WeaponBase
{
    public List<WeaponStats> LevelStats;

    [Serializable]
    public struct WeaponStats
    {
        public float AttackSpeed;
        public float AttackDamage;
        public float AOE;
    }
    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
    {
        foreach (RaycastHit2D Hit in Physics2D.CircleCastAll(playerPosition, LevelStats[level].AOE * playerStats.Area, Vector3.forward))
        {
            if (Hit.transform.CompareTag("Enemy"))
            {
                Hit.transform.GetComponent<Enemy_Main>().EnemyTakesDamage(LevelStats[level].AttackDamage * playerStats.DamageModifier);
            }
        }
    }

    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
}
