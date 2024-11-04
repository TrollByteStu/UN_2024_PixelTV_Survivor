using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "WeaponAura", menuName = "ScriptableObjects/WeaponTypes/Aura", order = 3)]
public class WeaponAura : WeaponBase
{
    public List<WeaponStats> LevelStats;
    public Texture Texture;
    public GameObject Prefab;

    private AuraBase[] AuraObjects = new AuraBase[2];

    [Serializable]
    public struct WeaponStats
    {
        public float AttackSpeed;
        public float AttackDamage;
        public float AOE;

        public WeaponStats(int i)
        {
            AttackSpeed = 1;
            AttackDamage = 10;
            AOE = 5;
        }
    }
    public override void Attack(int level, Transform playerTransform, Vector3 direction, PlayerStats playerStats)
    {
        SpawnVisuals(level, playerTransform, playerStats);

        foreach (RaycastHit2D Hit in Physics2D.CircleCastAll(playerTransform.position, LevelStats[level].AOE * playerStats.Area, Vector3.forward))
        {
            if (Hit.transform.CompareTag("Enemy"))
            {
                Hit.transform.GetComponent<Enemy_Main>().EnemyTakesDamage(LevelStats[level].AttackDamage * playerStats.DamageModifier);
            }
        }
    }

    void SpawnVisuals(int level, Transform playerTransform, PlayerStats playerStats)
    {
        for (int i = 0; i < AuraObjects.Length; i++)
        {
            if (AuraObjects[i] == null)
            {
                AuraObjects[i] = Instantiate(Prefab, playerTransform).GetComponent<AuraBase>();
                AuraObjects[i].Setup(Texture,Convert.ToBoolean(i%2), 4 * LevelStats[level].AOE * playerStats.Area * Vector3.one, 1);
            }
        }
        foreach (AuraBase gameObject in AuraObjects)
        {
           gameObject.AuraReset(4 * LevelStats[level].AOE * playerStats.Area * Vector3.one);
        }
    }

    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }

    public override int GetMaxLevel()
    {
        return LevelStats.Count-1;
    }
}
