using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Make it a singleton
    public static GameController Instance { get; private set; }

    public PlayerController PlayerReference;

    private void Awake()
    {
        // Make it a singleton, and prevent duplicates
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

}
