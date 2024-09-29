using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryCharacter", menuName = "ScriptableObjects/StoryCharacter", order = 1)]
public class Character : ScriptableObject
{
    [Tooltip("The name of the character")]
    public string ChracterName;

    [Tooltip("A list of the characters Stats")]
    public List<CharacterStatImplementation> Stats;

    [Tooltip("A list of the characters Skills")]
    public List<CharacterSkillImplementation> Skills;

}
