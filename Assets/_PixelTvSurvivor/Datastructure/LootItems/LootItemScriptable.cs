using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item", order = 1)]
public class LootItemScriptable : ScriptableObject
{
    [Header("Basic Stats")]
    [Tooltip("The name of this Item")]
    public string UpgradeName;

    [Tooltip("The image of this Item")]
    public Sprite upgradeSprite;

    [Space]
    [Header("Requirements to spawn")]

    [Tooltip("The Level requires for this upgrade")]
    public int levelRequired = 0;

    [Tooltip("Any weapon required for this upgrade?")]
    public WeaponStats requiedWeapon;

    [Space]
    [Header("Effects when Picked up")]

    [Tooltip("A list of Stat Effects")]
    public UpgradeStats Stats;

    [Tooltip("Any weapon given")]
    public WeaponStats givenWeapon;

    [Tooltip("Any weapon removed (like level 1 of same item)")]
    public WeaponStats removedWeapon;
}
