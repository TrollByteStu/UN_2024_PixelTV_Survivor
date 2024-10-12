using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : ScriptableObject 
{
    [Tooltip("What sound does it make when the weapon fires?")]
    public AudioClip AttackAudioFire;

    [Tooltip("What sound does it make when the weapon hits?")]
    public AudioClip AttackAudioHit;

    public virtual float GetAttackSpeed(int level) { return 0; }
    public virtual void Attack(int level, Vector3 playerPosition,Vector3 direction,PlayerStats playerStats) { }
    public virtual void SetAim(Vector3 direction) { }



}
