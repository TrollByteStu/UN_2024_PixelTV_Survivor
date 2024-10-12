using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSlash", menuName = "ScriptableObjects/WeaponTypes/Slash", order = 3)]
public class WeaponSlash : WeaponBase
{
    public GameObject Slash;
    public Sprite SlashSprite;
    public Side AttackSide;
    public List<WeaponStats> LevelStats = new List<WeaponStats> { new WeaponStats() };

    private Vector3 Aim;

    [Serializable]
    public struct WeaponStats
    {
        public float AttackSpeed;
        public float AttackDamage;
        public int AttackQuantity;
        public Vector2 AOE;

        public WeaponStats(int i)
        {
            AttackSpeed = 1;
            AttackDamage = 10;
            AttackQuantity = 1;
            AOE = new Vector2(2,3);
        }
    }

    public enum Side
    {
        Forward,
        Back,
        Alternate
    }

    public override void SetAim(Vector3 direction)
    {
        switch (direction.x)
        {
            case 0:
                break;
            case < 0:
                Aim = Vector3.left;
                break;
            case > 0:
                Aim = Vector3.right;
                break;
        }
    }
    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
    {

        for (int i = 0; i < LevelStats[level].AttackQuantity; i++)
        {
            Vector3 offset;
            switch (AttackSide)
            {

                case Side.Back:
                    offset = -Aim * (LevelStats[level].AOE.x * playerStats.Area / 2);
                    Instantiate(Slash, playerPosition + offset, Quaternion.identity).GetComponent<SlashBase>()
                        .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area * -new Vector3(Aim.x,1,1));


                    foreach (RaycastHit2D Hit in Physics2D.BoxCastAll(playerPosition + offset , LevelStats[level].AOE * playerStats.Area,0,Vector2.zero))
                    {
                        if (Hit.transform.CompareTag("Enemy"))
                        {
                            Hit.transform.GetComponent<Enemy_Main>().EnemyTakesDamage(LevelStats[level].AttackDamage * playerStats.DamageModifier);
                        }
                    }
                    break;
                case Side.Forward:
                    offset = Aim * (LevelStats[level].AOE.x * playerStats.Area / 2);
                    Instantiate(Slash, playerPosition + offset, Quaternion.identity).GetComponent<SlashBase>()
                        .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area * new Vector3(Aim.x, 1, 1));
                    foreach (RaycastHit2D Hit in Physics2D.BoxCastAll(playerPosition+ offset, LevelStats[level].AOE * playerStats.Area, 0, Vector2.zero))
                    {
                        if (Hit.transform.CompareTag("Enemy"))
                        {
                            Hit.transform.GetComponent<Enemy_Main>().EnemyTakesDamage(LevelStats[level].AttackDamage * playerStats.DamageModifier);
                        }
                    }
                    break;
                case Side.Alternate:
                    offset = Aim * Math.Pow(-1, i % 2).ConvertTo<float>() * (LevelStats[level].AOE.x * playerStats.Area / 2);
                    Instantiate(Slash, playerPosition + offset, Quaternion.identity).GetComponent<SlashBase>()
                        .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area * new Vector3(Aim.x, 1, 1) * Math.Pow(-1,i%2).ConvertTo<float>());
                    foreach (RaycastHit2D Hit in Physics2D.BoxCastAll(playerPosition + offset, LevelStats[level].AOE * playerStats.Area, 0,Vector2.zero))
                    {
                        if (Hit.transform.CompareTag("Enemy"))
                        {
                            Hit.transform.GetComponent<Enemy_Main>().EnemyTakesDamage(LevelStats[level].AttackDamage * playerStats.DamageModifier);
                        }
                    }
                    break;

            }

        }


    }

    public override float GetAttackSpeed(int level)
    {
        return LevelStats[level].AttackSpeed;
    }
}
