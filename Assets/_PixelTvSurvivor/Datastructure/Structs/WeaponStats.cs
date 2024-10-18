using System;
[Serializable]
public struct WeaponStats
{
    public WeaponBase Weapon;
    public float LastShot;
    public int Level;

    public WeaponStats(WeaponBase weapon)
    {
        Weapon = weapon;
        LastShot = 0;
        Level = 0;
    }
    public WeaponStats(WeaponBase weapon, int level)
    {
        Weapon = weapon;
        LastShot = 0;
        Level = level;
    }
    public void setLastShot(float newShot)
    {
        LastShot = newShot;
    }
}
