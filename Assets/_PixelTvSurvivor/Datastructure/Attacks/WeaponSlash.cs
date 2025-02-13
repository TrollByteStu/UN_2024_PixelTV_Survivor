using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "WeaponSlash", menuName = "ScriptableObjects/WeaponTypes/Slash", order = 3)]
public class WeaponSlash : WeaponBase
{
    public GameObject Slash;
    public Sprite SlashSprite;
    public Side AttackSide;
    public List<WeaponStats> LevelStats;

    private Vector3 Aim;

    [Serializable]
    public struct WeaponStats
    {
        public float AttackSpeed;
        public float AttackDamage;
        public int AttackQuantity;
        public Vector2 AOE;
        public float AttackDelay;
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
    public async override void Attack(int level, Transform playerTransform, Vector3 direction, PlayerStats playerStats)
    {

        for (int i = 0; i < LevelStats[level].AttackQuantity; i++)
        {
            Vector3 offset;
            switch (AttackSide)
            {
                case Side.Back:
                    offset = -Aim * (LevelStats[level].AOE.x * playerStats.Area / 2);
                    Instantiate(Slash, playerTransform.position + offset, Quaternion.identity).GetComponent<SlashBase>()
                        .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area , Aim, LevelStats[level].AttackDamage * playerStats.DamageModifier);
                    break;
                case Side.Forward:
                    offset = Aim * (LevelStats[level].AOE.x * playerStats.Area / 2);
                    Instantiate(Slash, playerTransform.position + offset, Quaternion.identity).GetComponent<SlashBase>()
                        .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area , Aim, LevelStats[level].AttackDamage * playerStats.DamageModifier);
                    break;
                case Side.Alternate:
                    offset = Aim * Math.Pow(-1, i % 2).ConvertTo<float>() * (LevelStats[level].AOE.x * playerStats.Area / 2);
                    Instantiate(Slash, playerTransform.position + offset, Quaternion.identity).GetComponent<SlashBase>()
                        .Setup(SlashSprite, LevelStats[level].AOE * playerStats.Area , Aim * Math.Pow(-1, i % 2).ConvertTo<float>(), LevelStats[level].AttackDamage * playerStats.DamageModifier);
                    break;

            }
            await Awaitable.WaitForSecondsAsync(LevelStats[level].AttackDelay);
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
