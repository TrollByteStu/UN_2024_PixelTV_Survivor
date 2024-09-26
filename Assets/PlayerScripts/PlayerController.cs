using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public SpriteRenderer BackgroundSprite;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private Vector2 MoveDirection;
    private float MoveSpeed = 5;
    private float MoveSpeedMultiplier = 1;
    private Vector4 FinalMovement;
    // moves Player 
    void Movement()
    {
        FinalMovement = MoveDirection.ConvertTo<Vector3>() * MoveSpeed * MoveSpeedMultiplier * Time.deltaTime;

        BackgroundSprite.material.SetVector("_Offset",BackgroundSprite.material.GetVector("_Offset") + FinalMovement);
        //BackgroundSprite.material.SetVector("_Offset", BackgroundSprite.material.GetVector("_Offset")  + Vector4.one * Time.deltaTime);
        //transform.position += MoveDirection.ConvertTo<Vector3>() * MoveSpeed * MoveSpeedMultiplier;
    }
    
    // Event called by Player Input Component on press and release of move keybind
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveDirection = context.ReadValue<Vector2>();
    }
}
