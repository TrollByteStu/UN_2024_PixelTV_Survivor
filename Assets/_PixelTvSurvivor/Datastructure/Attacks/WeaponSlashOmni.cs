using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSlashOmni", menuName = "ScriptableObjects/WeaponTypes/SlashOmni", order = 3)]
public class WeaponSlashOmni : WeaponBase
{
    public GameObject Slash;
    public Sprite SlashSprite;
    public bool AutoAim;
    public List<WeaponStats> LevelStats;
    [Serializable]
    public struct WeaponStats
    {
        public float AttackSpeed;
        public float AttackDamage;
        public int AttackQuantity;
        public Vector2 AOE;
        public float AttackDelay;
    }
    public override async void Attack(int level, Transform playerTransform, Vector3 direction, PlayerStats playerStats)
    {
        Vector3 offset;
        if (AutoAim)
        {
            List<RaycastHit2D> Hits = Physics2D.CircleCastAll(playerTransform.position, 15, Vector3.forward).ToList<RaycastHit2D>();
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
            Hits.Sort((h1, h2) => (h1.transform.position - playerTransform.position).magnitude.CompareTo((h2.transform.position - playerTransform.position).magnitude));



            if (Hits.Count > 0)
            {
                for (int i = 0; i < LevelStats[level].AttackQuantity; i++)
                {
                    offset = ( Hits[i % Hits.Count].transform.position - playerTransform.position).normalized * (LevelStats[level].AOE.x * playerStats.Area / 2);
                    Instantiate(Slash, playerTransform.position + offset, Quaternion.identity).GetComponent<SlashBase>()
                        .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area, Hits[i % Hits.Count].transform.position - playerTransform.position, LevelStats[level].AttackDamage * playerStats.DamageModifier);
                    await Awaitable.WaitForSecondsAsync(LevelStats[level].AttackDelay);
                }
            }
            return; 
        }

        for (int i = 0; i < LevelStats[level].AttackQuantity; i++)
        {
            offset = direction * (LevelStats[level].AOE.x * playerStats.Area / 2);
            Instantiate(Slash, playerTransform.position + offset, Quaternion.identity).GetComponent<SlashBase>()
                .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area, direction, LevelStats[level].AttackDamage * playerStats.DamageModifier);
            await Awaitable.WaitForSecondsAsync(LevelStats[level].AttackDelay);
        }
    }

    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
    public override int GetMaxLevel()
    {
        return LevelStats.Count -1;
    }
}
