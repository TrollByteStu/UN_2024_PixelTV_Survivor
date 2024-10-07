using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyCharacter", menuName = "ScriptableObjects/EnemyCharacter", order = 1)]
public class EnemyCharacter : ScriptableObject
{
    [Tooltip("The name of this enemy")]
    public string EnemyName;

    [Tooltip("A list of this enemies Stats")]
    public EnemyStats Stats;

    [Tooltip("the sprite for this enemy")]
    public Sprite enemySprite;

    [Tooltip("the size scale of this enemy")]
    public float spriteScale = 1;

    [Tooltip("the color of this enemy")]
    public Color32 spriteColor = Color.white;

    [Tooltip("the color of the blood/gunk this enemy spews when hit")]
    public Color32 bloodColor = Color.red;

    [Tooltip("the list of sounds for Walk")]
    public AudioClip[] enemyAudioWalk;

    [Tooltip("How often to play Walk sounds")]
    public float enemyAudioWalkProbablity;

    [Tooltip("the list of sounds for Walk")]
    public AudioClip[] enemyAudioAttack;

    [Tooltip("How often to play Walk sounds")]
    public float enemyAudioAttackProbablity;

    [Tooltip("the list of sounds for Walk")]
    public AudioClip[] enemyAudioDeath;

    [Tooltip("How often to play Walk sounds")]
    public float enemyAudioDeathProbablity;

}
