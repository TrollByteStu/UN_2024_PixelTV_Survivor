using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHoming", menuName = "ScriptableObjects/WeaponTypes/Homing", order = 2)]
public class WeaponHoming : WeaponBase
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
        public float ShotDelay;

        public WeaponStats(int i)
        {
            bulletSpeed = 10;
            AttackSpeed = 1;
            AttackDamage = 10;
            ShootQuantity = 1;
            ShotDelay = 0;
        }
    }
    public async override void Attack(int level, Transform playerTransform, Vector3 direction, PlayerStats playerStats)
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
            for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
            {
                if (HasCurve)
                {
                    if (FlipCurve)
                        Instantiate(BulletPrefab, playerTransform.position , Quaternion.identity).AddComponent<BulletHoming>()
                        .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, Hits[i % Hits.Count].transform.gameObject, Curve, Convert.ToBoolean(i % 2), bulletSprite);
                    else
                        Instantiate(BulletPrefab, playerTransform.position , Quaternion.identity).AddComponent<BulletHoming>()
                        .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, Hits[i % Hits.Count].transform.gameObject, Curve, false, bulletSprite);
                }
                else
                {
                    Instantiate(BulletPrefab, playerTransform.position, Quaternion.identity).AddComponent<BulletHoming>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, Hits[i % Hits.Count].transform.gameObject, null, false, bulletSprite);
                }

                await Awaitable.WaitForSecondsAsync(LevelStats[level].ShotDelay);
            }
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
