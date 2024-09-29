using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterSkillImplementation 
{
    [Tooltip("Drag the actual skill here")]
    public CharacterSkill Skill;

    [Tooltip("The higher the number, the better a character is at this skill")]
    public int skillLevel = 0;

    [Tooltip("How much experience have been accrued in this skill")]
    public int skillExperience = 0;

    public int rollSkill()
    {
        gainExperience();
        return Random.Range((int)((float)skillLevel * .5f), skillLevel);
    }

    public void gainExperience()
    {
        skillExperience++;
        if ( skillExperience >= skillLevel)
        {
            skillExperience = 0;
            skillLevel++;
        }
    }
}
