using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyCharacter", menuName = "ScriptableObjects/EnemyCharacter", order = 1)]
public class EnemyCharacter : ScriptableObject
{
    [System.Serializable]
    public class LootTableStructure
    {
        [Tooltip("What kind of loot?")]
        public LootItemScriptable LootType;

        [Tooltip("The chance this loot will drop")]
        [Range(0f, 100f)]
        public float LootChancePercent;
    }

    [Header("Basic Stats")]

    [Tooltip("The name of this enemy")]
    public string EnemyName;

    [Tooltip("The stats of this enemy")]
    public EnemyStats Stats;

    [Tooltip("Does this enemy spawn a gravestone when it dies? The gravestone will spawn 2 more..")]
    public bool SpawnsGravestoneUponDeath = false;

    [Space]
    [Header("Loot")]

    [Tooltip("The Loot table of this enemy")]
    public LootTableStructure[] LootTable;

    [Space]
    [Header("Looks")]

    [Tooltip("the sprite for this enemy")]
    public Sprite enemySprite;

    [Tooltip("the size scale of this enemy")]
    public float spriteScale = 1;

    [Tooltip("the color of this enemy")]
    public Color32 spriteColor = Color.white;

    [Tooltip("the color of the blood/gunk this enemy spews when hit")]
    public Color32 bloodColor = Color.red;

    [Space]
    [Header("Audio")]

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
