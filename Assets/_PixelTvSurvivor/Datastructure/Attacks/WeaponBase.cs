using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ScriptableObject 
{

    public virtual float GetAttackSpeed(int level) { return 0; }
    public virtual void Attack(int level, Vector3 playerPosition,Vector3 direction,PlayerStats playerStats) { }


}
