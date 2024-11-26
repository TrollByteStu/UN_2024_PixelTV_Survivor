using System;
using Unity.VisualScripting;

[Serializable]
public struct PlayerStats
{
    public string Name;
    public int Points;
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
    public int Coins;
    public float TimeUntilDeath;

    public PlayerStats (string name, float maxhealth, float health, float recovery,float armor, float movespeed)
    {
        Name = name;
        Points = 0;
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
        Coins = 53;
        TimeUntilDeath = 120;
    }
}
