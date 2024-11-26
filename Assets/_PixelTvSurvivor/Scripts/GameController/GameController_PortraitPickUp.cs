using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController_PortraitPickUp : MonoBehaviour
{
    public Portraits Prefab;
    public Sprite[] Portraits;
    public List<int> CurrentPortraits;
    public List<int> RemainingPortraits;

    public void SpawnPortrait(Vector3 position)
    {
        if (RemainingPortraits.Count > 0)
            Instantiate(Prefab, position, Quaternion.identity).Setup(Random.Range(0,RemainingPortraits.Count));
    }
    public void PickUp(int id)
    {
        CurrentPortraits.Add(id);
        RemainingPortraits.Remove(id);
    }
    public void ResetPortraits()
    {
        CurrentPortraits.Clear();
        int[] id = new int[Portraits.Length];
        int i =0; 
        Array.ForEach(id, p => id[i] = i++);  
        RemainingPortraits = id.ToList();
    }
}
