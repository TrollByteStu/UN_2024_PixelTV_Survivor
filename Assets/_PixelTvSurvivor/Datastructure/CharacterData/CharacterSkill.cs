using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definition: A Skill is something a character learns. Learning is progressive.
// Experience is stored for each skill, each use of the skills gives one experience in that skill.
// When you have as many experience, as the skill level, the skill increases and the experience resets.

[CreateAssetMenu(fileName = "CharacterSkill", menuName = "ScriptableObjects/CharacterSkill", order = 1)]
public class CharacterSkill : ScriptableObject
{

    [Tooltip("The name if this skill")]
    public string skillName;

    [Tooltip("Explain what this skill encompasses")]
    [TextArea(3, 5)]
    public string skillDescription;

    [Tooltip("What stat is this skill based on?")]
    public CharacterStat statLink;

    [Tooltip("Is this visible on a character sheet?")]
    public bool isVisible = true;
}
