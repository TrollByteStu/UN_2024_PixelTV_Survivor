using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item", order = 1)]
public class LootItemScriptable : ScriptableObject
{
    [Header("Appearance")]
    [Tooltip("The name of this Item")]
    public string ItemName;

    [Tooltip("The image of this Item")]
    public Sprite ItemSprite;

    [Tooltip("The size scale of this Item")]
    public float spriteScale = 1;

    [Tooltip("The color of this sprite")]
    public Color32 spriteColor = Color.white;

    [Space]
    [Header("Requirements to spawn")]

    [Tooltip("The Level requires for this upgrade")]
    public int levelRequired = 0;

    [Tooltip("Any weapon required for this upgrade?")]
    public WeaponStats requiedWeapon;

    [Space]
    [Header("Effects when Picked up")]

    [Tooltip("The popup text")]
    public string PopupText;

    [Tooltip("The color of the popup text")]
    public Color32 PopupColor = Color.white;

    [Tooltip("The sound played when players pick it up")]
    public AudioClip pickupAudio;

    [Tooltip("A list of Stat Effects")]
    public UpgradeStats Stats;

    [Tooltip("Any weapon given")]
    public WeaponStats givenWeapon;

    [Tooltip("Any weapon removed (like level 1 of same item)")]
    public WeaponStats removedWeapon;
}
