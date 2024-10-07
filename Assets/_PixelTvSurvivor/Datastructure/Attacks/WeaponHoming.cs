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
    }
    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
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

        if (Hits.Count > 0)
        {
            for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
            {
                if (HasCurve)
                {
                    if (FlipCurve)
                        Instantiate(BulletPrefab, playerPosition + direction, Quaternion.identity).AddComponent<BulletHoming>()
                        .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, Hits[i].transform.gameObject, Curve, Convert.ToBoolean(i % 2), bulletSprite);
                    else
                        Instantiate(BulletPrefab, playerPosition + direction, Quaternion.identity).AddComponent<BulletHoming>()
                        .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, Hits[i].transform.gameObject, Curve, false, bulletSprite);
                }
                else
                {
                    Instantiate(BulletPrefab, playerPosition + direction, Quaternion.identity).AddComponent<BulletHoming>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, Hits[i].transform.gameObject, null, false, bulletSprite);
                }
            }
        }
    }
    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
}
