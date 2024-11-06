using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Upgrade_Icon : MonoBehaviour
{
    public UpgradeScriptable chosenUpgradeType;

    public TMP_Text UpgradeTitle;
    public Image UpgradeIcon;
    public TMP_Text UpgradeFlavor;

    private PlayerController PlayerRef;
    public void SetUp()
    {
        UpgradeTitle.text = chosenUpgradeType.UpgradeName;
        UpgradeIcon.sprite = chosenUpgradeType.upgradeSprite;
        UpgradeFlavor.text = chosenUpgradeType.UpgradeFlavor;
    }
    public void ChooseUpgradeButton()
    {
        // basic prep
        UI_HUD.Instance.UpgradeHide();
        PlayerRef = GameController.Instance.PlayerReference;

        // actually upgrade stats
        if (chosenUpgradeType.Stats.MaxHealthIncrease > 0) PlayerRef.Stats.MaxHealth += chosenUpgradeType.Stats.MaxHealthIncrease;
        if (chosenUpgradeType.Stats.HealthModifierIncrease > 0) PlayerRef.Stats.HealthModifier += chosenUpgradeType.Stats.HealthModifierIncrease;
        if (chosenUpgradeType.Stats.RecoveryIncrease > 0) PlayerRef.Stats.Recovery += chosenUpgradeType.Stats.RecoveryIncrease;
        if (chosenUpgradeType.Stats.ArmorIncrease > 0) PlayerRef.Stats.Armor += chosenUpgradeType.Stats.ArmorIncrease;
        if (chosenUpgradeType.Stats.MoveSpeedIncrease > 0) PlayerRef.Stats.MoveSpeed += chosenUpgradeType.Stats.MoveSpeedIncrease;
        if (chosenUpgradeType.Stats.MoveSpeedModifierIncrease > 0) PlayerRef.Stats.MoveSpeedModifier += chosenUpgradeType.Stats.MoveSpeedModifierIncrease;
        if (chosenUpgradeType.Stats.DamageModifierIncrease > 0) PlayerRef.Stats.DamageModifier += chosenUpgradeType.Stats.DamageModifierIncrease;
        if (chosenUpgradeType.Stats.CooldownModifierIncrease > 0) PlayerRef.Stats.CooldownModifier += chosenUpgradeType.Stats.CooldownModifierIncrease;

        // actually add/upgrade weapons
    }
}
