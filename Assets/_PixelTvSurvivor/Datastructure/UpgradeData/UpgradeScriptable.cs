using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "ScriptableObjects/Upgrade", order = 1)]
public class UpgradeScriptable : ScriptableObject
{
    [Tooltip("The name of this Upgrade")]
    public string UpgradeName;

    [Tooltip("The image of this Upgrade")]
    public Sprite upgradeSprite;

    [Tooltip("The flavor text of this Upgrade")]
    public string UpgradeFlavor;

    [Tooltip("The Level requires for this upgrade")]
    public int levelRequired = 0;

    [Tooltip("Any weapon required for this upgrade?")]
    public WeaponStats requiedWeapon;

    [Tooltip("A list of this upgrade Stats")]
    public UpgradeStats Stats;

    [Tooltip("A weapon grated for this upgrade")]
    public WeaponStats givenWeapon;

    [Tooltip("Any weapon removed by this upgrade")]
    public WeaponStats removedWeapon;
}
