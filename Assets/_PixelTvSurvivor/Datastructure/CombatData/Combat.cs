using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StoryCombat", menuName = "ScriptableObjects/StoryCombat", order = 1)]
public class Combat : ScriptableObject
{
    [Tooltip("The Title at the top of the scene")]
    public string CombatTitle;

    [Tooltip("The List of opponets")]
    [SerializeField] private List<Character> opponents;

    [Tooltip("The Image shown")]
    public Texture2D Image;

    [Tooltip("If the player wins, goto this story element")]
    [SerializeField] private StoryOption WinGotoElement;

    [Tooltip("If the player looses the fight, goto this story element")]
    [SerializeField] private StoryOption LooseGotoElement;

}
