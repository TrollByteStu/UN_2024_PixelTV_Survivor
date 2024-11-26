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
    public string[] Names;
    public List<int> CurrentPortraits;
    public List<int> RemainingPortraits;
    public bool test = false;
    private void Update()
    {
        if (test)
        {
            SpawnPortrait(GameController.Instance.PlayerReference.transform.position);
            test = false;
        }
    }

    public void SpawnPortrait(Vector3 position)
    {
        if (RemainingPortraits.Count > 0)
        {
            int id = Random.Range(0, RemainingPortraits.Count);
            Instantiate(Prefab, position, Quaternion.identity).Setup(id);
            RemainingPortraits.Remove(id);
        }
            
    }
    public void PickUp(int id)
    {
        CurrentPortraits.Add(id);
        UI_HUD.Instance.AddPortrait(Portraits[id],CurrentPortraits.Count-1, CurrentPortraits.Count % 2 == 1, Names[id]);
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
