using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponOrbit", menuName = "ScriptableObjects/WeaponTypes/Orbit", order = 1)]
public class WeaponOrbit : WeaponBase
{
    private List<GameObject> Satellites = new List<GameObject>();
    public GameObject Satellite;
    public List<WeaponStats> LevelStats;
    [Serializable]
    public struct WeaponStats
    {
        public float DistanceFromPlayer;
        public float RotationSpeed;
        public float AttackDamage;
        public float SatelliteQuantity;
    }

    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
    {
        for (int i = 0; i < LevelStats[level].SatelliteQuantity; i++)
        {
            if (Satellites[i] == null)
                Satellites[i] = Instantiate(Satellite);

        }
    }
    public override float GetAttackSpeed(int level)
    {
        return 0;
    }

}
