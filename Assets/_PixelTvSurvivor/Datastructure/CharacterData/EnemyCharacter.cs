using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCharacter", menuName = "ScriptableObjects/PlayerCharacter", order = 1)]
public class EnemyCharacter : ScriptableObject
{
    [Tooltip("The name of the character")]
    public string ChracterName;

    [Tooltip("A list of the characters Stats")]
    public List<CharacterStatImplementation> Stats;

    [Tooltip("A list of the characters Skills")]
    public List<CharacterSkillImplementation> Skills;
}
