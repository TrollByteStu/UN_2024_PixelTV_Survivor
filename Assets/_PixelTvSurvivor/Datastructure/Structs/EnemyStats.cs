using System;
[Serializable]
public struct EnemyStats
{
    public enum EnemyAiType { Zombie,Shooter,LootGoblin };

    public EnemyAiType AiType;
    public float MaxHealth;
    public float Health;
    public float MoveSpeed;
    public float AttackDamage;
    public float AttackSpeed;
    public float AttackRange;
    public float XpValue;
    public int PointValue;
    public float TimeSecondsValue;
}
