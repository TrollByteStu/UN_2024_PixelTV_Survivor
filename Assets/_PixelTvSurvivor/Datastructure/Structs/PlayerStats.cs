using System;
using Unity.VisualScripting;

[Serializable]
public struct PlayerStats
{
    public string Name;
    public float MaxHealth;
    public float HealthModifier;
    public float Health;
    public float Recovery;
    public float Armor;
    public float MoveSpeed;
    public float MoveSpeedModifier;
    public float DamageModifier;
    public float CooldownModifier;
    public float Area;
    public float XpModifier;
    public float Xp;
    public float MaxXp;
    public int Level;
    public int Reroll;

    public PlayerStats (string name, float maxhealth, float health, float recovery,float armor, float movespeed)
    {
        Name = name;
        MaxHealth = maxhealth;
        HealthModifier = 1;
        Health = health;
        Recovery = recovery;
        Armor = armor;
        MoveSpeed = movespeed;
        MoveSpeedModifier = 1;
        DamageModifier = 1;
        CooldownModifier = 1;
        Area = 1;
        XpModifier = 1;
        Xp = 0;
        MaxXp = 10;
        Level = 1;
        Reroll = 0;
    }
}
