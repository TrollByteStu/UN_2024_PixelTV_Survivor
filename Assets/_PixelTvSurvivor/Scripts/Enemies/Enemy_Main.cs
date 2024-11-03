using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Enemy_Main : MonoBehaviour
{
    [Header("Internal references")]
    public SpriteRenderer mySpriteRenderer;
    public Animator myAnimator;

    [Header("Prefabs")]
    public GameObject XpOrb;

    [Header("Data")]
    public EnemyCharacter enemytype;
    public EnemyStats myStats;

    // status effects
    public float StunTime;

    // external references
    private Transform playerRef;
    private PlayerController playerControllerRef;

    // enemy attack
    private float myLastAttackTime;

    // bumping into walls the ugly way
    private Vector3 thisPosition;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    public void Start()
    {
        playerRef = GameController.Instance.PlayerReference.transform;
        playerControllerRef = GameController.Instance.PlayerReference;
        myStats = enemytype.Stats;
        transform.name = enemytype.EnemyName;
    }

    // Update is called once per frame
    void Update()
    {
 
        // kill enemy if to far from player
        if (Vector3.Distance(playerRef.position, transform.position) > 30)
            GameController.Instance.EnemyPool_Release(gameObject);

        // ai
        Movement();
        // attack
        if (Vector3.Distance(transform.position, playerRef.position) < myStats.AttackRange) EnemyAttacksPlayer();
        //EnemyMoveByAIType();

    }

    private void EnemyMoveByAIType()
    {
        switch (myStats.AiType)
        {
            case EnemyStats.EnemyAiType.LootGoblin or EnemyStats.EnemyAiType.Zombie or EnemyStats.EnemyAiType.Shooter:
                Movement();
            break;
            default:
            break;
        }
    }
    //private void EnemyMove_ZombieType()
    //{
    //    if (statusStunTimerLeft <= 0)
    //        transform.position = Vector3.MoveTowards(transform.position, playerRef.position, myStats.MoveSpeed * Time.deltaTime);
    //    if (Vector3.Distance(transform.position, playerRef.position) < myStats.AttackRange) EnemyAttacksPlayer();
    //}
    //private void EnemyMove_LootGoblinType()
    //{
    //    if (statusStunTimerLeft <= 0)
    //        transform.position = Vector3.MoveTowards(transform.position, transform.position+((transform.position-playerRef.position)*2), myStats.MoveSpeed * Time.deltaTime);
    //}

    void Movement()
    {
        if (StunTime > Time.time) return;

        Vector3 direction = (playerRef.position - transform.position ).normalized * myStats.MoveSpeed * Time.deltaTime;

        foreach (RaycastHit2D Hit in Physics2D.LinecastAll(transform.position, direction + transform.position ))
        {
            if (Hit.collider.CompareTag("Walls") || Hit.collider.CompareTag("Player"))
            {
                direction *= Hit.distance;
                break;
            }
        }
        //Debug.DrawRay(transform.position, direction,Color.red,1);

        transform.position += direction;

        if (direction.x < 0)
            transform.localScale = new Vector3(-enemytype.spriteScale, enemytype.spriteScale, enemytype.spriteScale);
        else
            transform.localScale = Vector3.one * enemytype.spriteScale;

    }

    public void Stun(float seconds)
    {
        StunTime = seconds + Time.time;
    }

    public void EnemyTakesDamage(float damage)
    {
        myStats.Health -= damage;
        if (myStats.Health <= 0) EnemyDies();
        Knockback();

    }
    void Knockback()
    {
        if (transform.tag != "Enemy") return;
        transform.position += (transform.position - playerRef.position).normalized * myStats.TakenKnockback;

    }

    public void EnemyDropsBlood()
    {
        var blood = GameController.Instance.BloodPool_Get().GetComponent<BloodSplatHandler>();
        blood.Start();
        blood.transform.position = transform.position;
        blood.transform.SetParent(GameController.Instance.BloodHolder);
        blood.colorRange(enemytype.bloodColorMin,enemytype.bloodColorMax);
        blood.audioRange(enemytype.enemyAudioDeath);
    }

    public void EnemyDies()
    {
        if (transform.tag != "Enemy") return;
        // add effects and sounds
        EnemyDropsBlood();
        GameController.Instance.PlayerReference.AddPoints(myStats.PointValue,myStats.TimeSecondsValue);
        GameController.Instance.PlayerReference.AddXp(myStats.XpValue);
        
        //Instantiate(XpOrb,transform.position,Quaternion.identity,GameController.Instance.XpHolder).GetComponent<XpOrb>().xp = myStats.XpValue;
        EnemyDies_DropLoot();
        //if ( enemytype.SpawnsGravestoneUponDeath && GameController.Instance.FPS_isWithinLimit( 50 ) )
        //{ // spawn a gravestone
        //    Instantiate(GameController.Instance.GraveStonePrefab, transform.position, Quaternion.identity).GetComponent<Gravestone>().SpawnableEnemy = enemytype;
        //}
        //if ( gameObject.activeInHierarchy ) GameController.Instance.EnemyPool_Release(gameObject);
        transform.tag = "Untagged";
        if (myStats.MoveSpeed > 0f)
        { // moves towards player
            myStats.MoveSpeed = myStats.MoveSpeed * -10f;
        } else { // moves away from player
            myStats.MoveSpeed = myStats.MoveSpeed * 10f;
        }
    }
    private void EnemyDies_DropLoot()
    {
        if (enemytype.LootTable.Length < 1) return; // no loot
        foreach (EnemyCharacter.LootTableStructure possibleLoot in enemytype.LootTable)
        { // try them all, roll for each(simpler to explain to the designers)
            if (possibleLoot.LootChancePercent > Random.Range(0f, 100f) && EnemyDies_DropLoot_CheckLootPossible(possibleLoot))
            {
                Instantiate(GameController.Instance.GenericItemPrefab, transform.position, Quaternion.identity).GetComponent<ItemHandler>().ItemType = possibleLoot.LootType;
                return;
            }
        }  
    }
    private bool EnemyDies_DropLoot_CheckLootPossible(EnemyCharacter.LootTableStructure checkLoot)
    { // check if this player can get this loot at this time
        // all the ways it can fail
        if (checkLoot.LootType == null) return false;
        if ( checkLoot.LootType.givenWeapon.Level > 0 )
        { // do not spawn a weapon player already has
            if (playerControllerRef.DoesPlayerHaveWeapon(checkLoot.LootType.givenWeapon.Weapon)) return false;
        }
        // passed all tests, player can get this loot
        return true;
    }

    public void EnemyAttacksPlayer()
    {
        // Still not recovered from last attack on player
        if (myLastAttackTime + myStats.AttackSpeed > Time.time ) return;
        // no longer an enemy
        if (transform.tag != "Enemy") return;
        // play sound of zombie attacking?!?
        GameController.Instance.PlayerReference.PlayerTakesDamage(myStats.AttackDamage);
        // possibly blood particles?!?
        myLastAttackTime = Time.time;
    }

    public void Setup(EnemyCharacter enemy)
    {
        enemytype = enemy;
        transform.localScale = new Vector3(enemytype.spriteScale, enemytype.spriteScale, 1);
        mySpriteRenderer.color = Color.Lerp(enemytype.spriteColorMin, enemytype.spriteColorMax, Random.value);
        mySpriteRenderer.sprite = enemytype.enemySprite;
        myAnimator.runtimeAnimatorController = enemytype.enemyAnimController;
        myStats = enemytype.Stats;
        transform.name = enemytype.EnemyName;
        transform.tag = "Enemy";
        myStats.AttackDamage += (Time.time * 0.001f);
        myStats.TakenKnockback -= (Time.time * 0.001f);
        if (myStats.TakenKnockback < 0f) myStats.TakenKnockback = 0f;
        myStats.MaxHealth *= 1f + (Time.time * 0.01f);
        myStats.Health = myStats.MaxHealth;
        myStats.MoveSpeed += (Time.time * 0.001f);
    }
}
