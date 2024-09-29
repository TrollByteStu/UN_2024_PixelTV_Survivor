using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatImplementation
{
    [Tooltip("Drag the actual stat here")]
    public CharacterStat Stat;

    [Tooltip("What is the value of this stat")]
    public int value;
}
