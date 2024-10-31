using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;

public class UI_Slotmachine : MonoBehaviour
{

    public UI_Slot Slot1;
    public UI_Slot Slot2;
    public UI_Slot Slot3;

    private bool isRolling = false;

    // Start is called before the first frame update
    void Start() {
        Slot1.Setup(this,11f);
        Slot2.Setup(this,22f);
        Slot3.Setup(this,33f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRoll()
    {
        Debug.Log("Start roll");
        Slot1.StartRolling();
        Slot2.StartRolling();
        Slot3.StartRolling();
        isRolling = true;
    }

}
