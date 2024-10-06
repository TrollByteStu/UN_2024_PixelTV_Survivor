using System;
[Serializable]
public struct WeaponStats
{
    public Weapons Weapon;
    public float LastShot;
    public int Level;

    public WeaponStats(Weapons weapon)
    {
        Weapon = weapon;
        LastShot = 0;
        Level = 0;
    }
    public WeaponStats(Weapons weapon, int level)
    {
        Weapon = weapon;
        LastShot = 0;
        Level = level;
    }
}
