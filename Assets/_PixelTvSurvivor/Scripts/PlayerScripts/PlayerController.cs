using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Linq;
using System;

public class PlayerController : MonoBehaviour
{
    private Camera Cam;
    [Header("Player Stats")]
    public PlayerStats Stats = new("Player",100,100,0.1f,0,10);
    public WeaponStats[] WeaponsArray;

    [Header("Santa Sprite Layered")]
    public GameObject Santa_Body_Idle;
    private SpriteRenderer Santa_Body_Idle_Rend;
    public GameObject Santa_Body_Walk;
    private SpriteRenderer Santa_Body_Walk_Rend;
    public GameObject Santa_Gun_Idle;
    private SpriteRenderer Santa_Gun_Idle_Rend;
    public GameObject Santa_Gun_Walk;
    private SpriteRenderer Santa_Gun_Walk_Rend;

    public SpriteRenderer Santa2WalkRenderer;
    public SpriteRenderer Santa2IdleRenderer;

    //private internal
    private Shader shaderGUItext;
    private Shader shaderSpritesDefault;
    private float PlayerHitTimer = 0f;
    private bool PlayerHitSwitch = false;
    private CameraShake MyShake;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Cam = FindAnyObjectByType<Camera>();
        MyShake = Camera.main.GetComponent<CameraShake>();
        GameController.Instance.SetupForGame( this );

        //time is stopped until first spin 
        //Time.timeScale = 0f;

        // shaders for hit indicator
        shaderGUItext = Shader.Find("GUI/Text Shader");
        shaderSpritesDefault = Shader.Find("Universal Render Pipeline/2D/Sprite-Unlit-Default");

        // renderers for hit indicator
        Santa_Body_Idle_Rend = Santa_Body_Idle.gameObject.GetComponent<SpriteRenderer>();
        Santa_Body_Walk_Rend = Santa_Body_Walk.gameObject.GetComponent<SpriteRenderer>();
        Santa_Gun_Idle_Rend = Santa_Gun_Idle.gameObject.GetComponent<SpriteRenderer>();
        Santa_Gun_Walk_Rend = Santa_Gun_Walk.gameObject.GetComponent<SpriteRenderer>();
}

    // Update is called once per frame
    void Update()
    {
        Movement();
        Attacks();
        PickupXP();
        StatsAndStatusHandler();

        // indicate damage
        if (PlayerHitSwitch && PlayerHitTimer <= Time.timeSinceLevelLoad)
        {
            PlayerHitSwitch = false;
            /*Santa_Body_Idle_Rend.material.shader = shaderSpritesDefault;
            Santa_Body_Walk_Rend.material.shader = shaderSpritesDefault;
            Santa_Gun_Idle_Rend.material.shader = shaderSpritesDefault;
            Santa_Gun_Walk_Rend.material.shader = shaderSpritesDefault;*/
            Santa2WalkRenderer.material.shader = shaderSpritesDefault;
            Santa2IdleRenderer.material.shader = shaderSpritesDefault;
            Santa2WalkRenderer.color = Color.white;
            Santa2IdleRenderer.color = Color.white;
        }
    }

    private Vector2 MoveDirection;
    private Vector3 FinalMovement;
    // moves Player 
    void Movement()
    {
        FinalMovement = Stats.MoveSpeed * Stats.MoveSpeedModifier * Time.deltaTime * MoveDirection.ConvertTo<Vector3>();
        // raycast check for walls in move direction
        foreach (RaycastHit2D Hit in Physics2D.LinecastAll(transform.position,transform.position + FinalMovement))
        {
            if (Hit.collider.CompareTag("Walls"))
            {
                FinalMovement *= Hit.distance;
                break;
            }
        }
        transform.position += FinalMovement;
    }

    private Vector2 AimDirection = new(1,0);
    void Attacks()
    {
        for (int i = 0; i < WeaponsArray.Length; i++) 
        {
            if (WeaponsArray[i].LastShot < Time.time )
            {
                WeaponsArray[i].LastShot = Time.time + WeaponsArray[i].Weapon.GetAttackSpeed(WeaponsArray[i].Level) * Stats.CooldownModifier;
                WeaponsArray[i].Weapon.Attack(WeaponsArray[i].Level, transform, AimDirection, Stats);
            
            }
        }
    }
    void PickupXP()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, 5, Vector3.forward))
        {
            if (hit.transform.CompareTag("PickupAble"))
            {
                if (hit.transform.GetComponent<Coin>() != null)
                    hit.transform.GetComponent<Coin>().Pickup();
                if (hit.transform.GetComponent<ItemHandler>() != null)
                    hit.transform.GetComponent<ItemHandler>().Pickup();
            }
        }
    }
    void StatsAndStatusHandler()
    {
        // Time
        Stats.TimeUntilDeath -= Time.deltaTime;

        // Stats
        Stats.Health += Time.deltaTime * Stats.Recovery;
        if (Stats.Health > Stats.MaxHealth) Stats.Health = Stats.MaxHealth;

        // Statuses
        if (Stats.TimeUntilDeath <= 0) GameController.Instance.PlayerIsDead();
    }

    public void PlayerTakesDamage(float damage)
    {
        // play sound of player moaning/complaining?
        Stats.Health -= math.clamp( damage - Stats.Armor,0,999999999);

        // damage indicator
        /*Santa_Body_Idle_Rend.material.shader = shaderGUItext;
        Santa_Body_Walk_Rend.material.shader = shaderGUItext;
        Santa_Gun_Idle_Rend.material.shader = shaderGUItext;
        Santa_Gun_Walk_Rend.material.shader = shaderGUItext;*/
        Santa2WalkRenderer.material.shader = shaderGUItext;
        Santa2IdleRenderer.material.shader = shaderGUItext;
        Santa2WalkRenderer.color = Color.red;
        Santa2IdleRenderer.color = Color.red;
        PlayerHitTimer = Time.timeSinceLevelLoad + 0.35f;
        PlayerHitSwitch = true;

        // is dead?
        if (Stats.Health < 1) GameController.Instance.PlayerIsDead();
        else MyShake.shake();
    }
    public void AddPoints(int Points, float time)
    {
        Stats.Points += Points;
        //Stats.TimeUntilDeath += time;
        GameController.Instance.gamePoints = Stats.Points;
        GameController.Instance.myUS.AddStatPoints(Points);
    }

    private bool FirstSpin = true;
    public void OnSpin(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;
        if (FirstSpin)
        {
            Time.timeScale = 1f;
            FirstSpin = false;
        }
        UI_HUD.Instance.ShowSlotMachine_PullHandle();
    }

    // Event called by Player Input Component on press and release of move keybind
    public void OnMove(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>());
    }

    Vector2 TouchStart = Vector2.zero;
    public void OnTouch(InputAction.CallbackContext context)
    {
        TouchStart = context.ReadValue<Vector2>();

        if (TouchStart.x / Cam.pixelWidth > 0.6 && TouchStart.y / Cam.pixelHeight > 0.6)
        {
            if (FirstSpin)
            {
                Time.timeScale = 1f;
                FirstSpin = false;
            }
            UI_HUD.Instance.ShowSlotMachine_PullHandle();
        }
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        Move(context.ReadValue<Vector2>() - TouchStart);
    }

    public void OnRelease(InputAction.CallbackContext context)
    {
        Move(Vector2.zero);
    }
    
    void Move(Vector2 vector2)
    {
        MoveDirection = vector2.normalized;
        if (MoveDirection != Vector2.zero)
        { // is moving
            AimDirection = MoveDirection;
            foreach (WeaponStats weaponStats in WeaponsArray)
            {
                weaponStats.Weapon.SetAim(AimDirection);
            }

            // Flip player sprite to face movement
            if (MoveDirection.x < 0)
                transform.localScale = new Vector3(-.5f, .5f, 1);
            else if (MoveDirection.x > 0)
                transform.localScale = new Vector3(.5f, .5f, 1);

            /*
            Santa_Body_Idle.SetActive(false);
            Santa_Body_Walk.SetActive(true);
            Santa_Gun_Idle.SetActive(false);
            Santa_Gun_Walk.SetActive(true);
            */
            //Santa2Animator.speed = 1;
            Santa2WalkRenderer.gameObject.SetActive(true);
            Santa2IdleRenderer.gameObject.SetActive(false);
        }
        else
        { // not moving
            /*
            Santa_Body_Idle.SetActive(true);
            Santa_Body_Walk.SetActive(false);
            Santa_Gun_Idle.SetActive(true);
            Santa_Gun_Walk.SetActive(false);
            */
            //Santa2Animator.speed = 0;
            Santa2WalkRenderer.gameObject.SetActive(false);
            Santa2IdleRenderer.gameObject.SetActive(true);
        }
    }

    public void AddCoins(int amount)
    {
        Stats.Coins += amount;
    }
    public bool DoesPlayerHaveWeapon(WeaponBase checkWeapon)
    {
        foreach (WeaponStats lookForWeapon in WeaponsArray)
        {
            if (lookForWeapon.Weapon == checkWeapon) return true;
        }
        return false;
    }
    public void AddWeapon(WeaponBase addWeapon)
    {
        if (DoesPlayerHaveWeapon(addWeapon)) return;
        //adds new weapon to array
        WeaponsArray = WeaponsArray.Concat(new WeaponStats[] { new WeaponStats(addWeapon) }).ToArray();
    }

    public void AddWeapon(WeaponStats addWeapon)
    {
        if (DoesPlayerHaveWeapon(addWeapon.Weapon)) return;
        //adds new weapon to array
        WeaponsArray = WeaponsArray.Concat(new WeaponStats[] { addWeapon }).ToArray();
    }

    public void UpgradeWeapon(WeaponBase weapon)
    {
        if (WeaponUpgradeable(weapon))
            WeaponsArray[FindWeapon(weapon)].Level++;
    }

    int FindWeapon(WeaponBase weapon)
    {
        for (int i = 0; i < WeaponsArray.Length; i++)
        {
            if (WeaponsArray[i].Weapon == weapon)
                return i;
        }
        Debug.LogError("none found");
        return 99999999;
    }

    public bool WeaponUpgradeable(WeaponBase weapon)
    {
        return WeaponsArray[FindWeapon(weapon)].Level < weapon.GetMaxLevel();
    }

}
