using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "WeaponBasic", menuName = "ScriptableObjects/WeaponTypes/Basic", order = 1)]
public class WeaponBasic : WeaponBase
{
    public GameObject BulletPrefab;
    public Sprite bulletSprite;
    public bool AutoAim;
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
    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
    public override async void Attack(int level, Transform playerTransform, Vector3 direction, PlayerStats playerStats)
    {
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
                for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
                {
                    if (HasCurve)
                    {
                        if (FlipCurve)
                            Instantiate(BulletPrefab, playerTransform.position + (Hits[i % Hits.Count].transform.position - playerTransform.position).normalized, Quaternion.identity).AddComponent<BulletBasic>()
                            .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, (Hits[i % Hits.Count].transform.position - playerTransform.position).normalized, Curve, Convert.ToBoolean(i % 2), bulletSprite);
                        else
                            Instantiate(BulletPrefab, playerTransform.position + (Hits[i % Hits.Count].transform.position - playerTransform.position).normalized, Quaternion.identity).AddComponent<BulletBasic>()
                            .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, (Hits[i % Hits.Count].transform.position - playerTransform.position).normalized, Curve, false, bulletSprite);
                    }
                    else
                    {
                        Instantiate(BulletPrefab, playerTransform.position + (Hits[i % Hits.Count].transform.position - playerTransform.position).normalized, Quaternion.identity).AddComponent<BulletBasic>()
                        .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, (Hits[i % Hits.Count].transform.position - playerTransform.position).normalized, null, false, bulletSprite);
                    }
                    await Awaitable.WaitForSecondsAsync(LevelStats[level].ShotDelay);
                }
            }
            return;
        }



        for (int i = 0; i < LevelStats[level].ShootQuantity; i++)
        {
            if (HasCurve)
            {
                if (FlipCurve)
                    Instantiate(BulletPrefab, playerTransform.position + direction, Quaternion.identity).AddComponent<BulletBasic>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, direction, Curve, Convert.ToBoolean(i % 2), bulletSprite);
                else
                    Instantiate(BulletPrefab, playerTransform.position + direction, Quaternion.identity).AddComponent<BulletBasic>()
                    .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, direction, Curve, false, bulletSprite);
            }
            else
            {
                Instantiate(BulletPrefab, playerTransform.position + direction, Quaternion.identity).AddComponent<BulletBasic>()
                .Setup(LevelStats[level].bulletSpeed, LevelStats[level].AttackDamage * playerStats.DamageModifier, direction, null, false, bulletSprite);
            }
            await Awaitable.WaitForSecondsAsync(LevelStats[level].ShotDelay);
        }
    }
    public override int GetMaxLevel()
    {
        return LevelStats.Count - 1;
    }
}
