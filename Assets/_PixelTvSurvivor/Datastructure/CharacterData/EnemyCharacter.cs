using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyCharacter", menuName = "ScriptableObjects/EnemyCharacter", order = 1)]
public class EnemyCharacter : ScriptableObject
{
    [Tooltip("The name of the enemy")]
    public string EnemyName;

    [Tooltip("A list of the enemies Stats")]
    public EnemyStats Stats;

    //[Tooltip("A list of the characters Skills")]
    //public List<CharacterSkillImplementation> Skills;
}
