using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponOrbit", menuName = "ScriptableObjects/WeaponTypes/Orbit", order = 1)]
public class WeaponOrbit : WeaponBase
{
    private Transform SatelliteHolder;
    private List<GameObject> Satellites = new List<GameObject>();
    public GameObject Satellite;
    public Spin SpinDirection;
    public List<WeaponStats> LevelStats = new List<WeaponStats> { new WeaponStats() };
    [Serializable]
    public struct WeaponStats
    {
        public float DistanceFromPlayer;
        public float RotationSpeed;
        public float AttackDamage;
        public int SatelliteQuantity;
        public int LayerQuantity;

        public WeaponStats(int i)
        {
            DistanceFromPlayer = 5;
            RotationSpeed = 5;
            AttackDamage = 10;
            SatelliteQuantity = 1;
            LayerQuantity = 1;
        }
    }

    public enum Spin
    {
        Left,
        Right,
        AlternateA,
        AlternateB
    }
    public override void Attack(int level, Vector3 playerPosition, Vector3 direction, PlayerStats playerStats)
    {
        if (SatelliteHolder == null)
            SatelliteHolder = GameController.Instance.SatelliteHolder;

        int QuantityPerLayer = LevelStats[level].SatelliteQuantity / LevelStats[level].LayerQuantity;
        for (int i = 0; i < LevelStats[level].SatelliteQuantity; i++)
        { 
            if (Satellites.Count == i)
            {
                var sat = Instantiate(Satellite, SatelliteHolder);
                sat.GetComponent<SatelliteBase>().SetUp(this);
                Satellites.Add(sat);
            }
            if (Satellites[i] == null)
            {
                var sat = Instantiate(Satellite, SatelliteHolder);
                sat.GetComponent<SatelliteBase>().SetUp(this);
                Satellites[i] = sat;
            }

            switch (SpinDirection)
            {
                case Spin.Left:
                    Satellites[i].transform.position = new Vector3(
                    /*x*/    -math.sin(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)),
                    /*y*/    math.cos(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)), 0) + playerPosition;
                    break;
                case Spin.Right:
                    Satellites[i].transform.position = new Vector3(
                    /*x*/    math.sin(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)),
                    /*y*/    math.cos(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)), 0) + playerPosition;
                    break;
                case Spin.AlternateA:
                    Satellites[i].transform.position = new Vector3(
                    /*x*/    math.pow(-1,math.floor((i + QuantityPerLayer) / QuantityPerLayer) % 2) * math.sin(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)),
                    /*y*/    math.cos(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)), 0) + playerPosition;
                    break;
                case Spin.AlternateB:
                    Satellites[i].transform.position = new Vector3(
                    /*x*/    -math.pow(-1, math.floor((i + QuantityPerLayer) / QuantityPerLayer) % 2) * math.sin(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)),
                    /*y*/    math.cos(Time.timeSinceLevelLoad * LevelStats[level].RotationSpeed + (math.PI * 2) / QuantityPerLayer * (i % QuantityPerLayer)) * (LevelStats[level].DistanceFromPlayer / LevelStats[level].LayerQuantity * math.floor((i + QuantityPerLayer) / QuantityPerLayer)), 0) + playerPosition;
                    break;
            }
        }

        //Clean up incase of reduction in satellites
        foreach (GameObject obj in Satellites.GetRange(LevelStats[level].SatelliteQuantity, Satellites.Count - LevelStats[level].SatelliteQuantity))
        {
            Destroy(obj);
            Satellites.Remove(obj);
        }
    }

    public override float GetDamage(int level)
    {
        return LevelStats[level].AttackDamage;
    }


}
