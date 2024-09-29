using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stats are the most basic numbers describing the nuanced diffrences in who characters are and what they can do.
// They are rarely challenged directly(rolled for), but often "checked".
// Several types exists

[CreateAssetMenu(fileName = "CharacterStat", menuName = "ScriptableObjects/CharacterStat", order = 1)]
public class CharacterStat : ScriptableObject
{
    [System.Serializable]
    public enum statTypes
    {
        Fixed,          // Is just a fixed number. Example: player choose "Elf" and gets a stat "Wilderness Kinship +2"
        SkillDerived,   // Derived from skills linked to this stat. Example: "Strength" is Derived from the skills "Lift" and "Blacksmithing"
        Dynamic,        // A changeable stat with another stat as maximum. Example "Hitpoints" is based on "Fortitude" and "Mana" on "Willpower"
        Independent     // Simply a number, not dependant on other skills/stats. Example: "Cursed Unholy" or "BufFed Luck"
    };

    [Tooltip("The name shown by this stat")]
    public string statName;

    [Tooltip("Explain what this stat does")]
    [TextArea(3, 5)]
    public string statsDescription;

    #if UNITY_EDITOR
    [Help(".: StatTypes :.\nFixed: A number that never changes. Example; Elf has Wilderness +2\nSkillDerived: It derives its stats from Skills linked to that stats. Example; Willpower derives from EndurePain and TolerateBoredom\nDynamic: A changeable stats based on another. Example; Hitpoints based on Fortitude, Stamina on Grit\nIndependant: Simply a number. Example; UnholyCurse or BuffedLuck or Karma", UnityEditor.MessageType.Warning)]
    [Tooltip("What kind of stat is this?")]
    #endif
    public statTypes statType;

    [Tooltip("What stat does this stat get its maximum from?")]
    [DrawIf("statType", statTypes.Dynamic)]
    public CharacterStat statMax;

    [Tooltip("If this stat reaches zero, will the character die?")]
    [DrawIf("statType", statTypes.Dynamic)]
    public bool ifZeroWillDie = false;

    [Tooltip("Does this stat go below zero?")]
    public bool goesBelowZero = false;

    [Tooltip("Is this visible on a character sheet?")]
    public bool isVisible = false;
}
