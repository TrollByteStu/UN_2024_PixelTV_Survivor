using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Main : MonoBehaviour
{
    public EnemyCharacter enemytype;
    public GameObject XpOrb;
    public EnemyStats myStats;

    // status effects
    public float statusStunTimerLeft;

    private Transform playerRef;

    // enemy attack
    private float myLastAttackTime;

    // bumping into walls the ugly way
    private Vector3 thisPosition;
    private Vector3 lastPosition;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameController.Instance.PlayerReference.transform;
        myStats = enemytype.Stats;
        transform.name = enemytype.EnemyName;
    }

    // Update is called once per frame
    void Update()
    {
        // janky wall blocker
        lastPosition = thisPosition;
        thisPosition = transform.position;

        // ai
        EnemyMoveByAIType();

        // stats & status
        if ( statusStunTimerLeft > 0 )
        {
            statusStunTimerLeft -= Time.deltaTime;
            if ( statusStunTimerLeft < 0 ) statusStunTimerLeft = 0;
        }
    }
    private void EnemyMoveByAIType()
    {
        switch (myStats.AiType)
        {
            default:
                EnemyMove_ZombieType();
            break;
        }
    }
    private void EnemyMove_ZombieType()
    {
        if (statusStunTimerLeft <= 0)
            transform.position = Vector3.MoveTowards(transform.position, playerRef.position, myStats.MoveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, playerRef.position) < myStats.AttackRange) EnemyAttacksPlayer();
    }

    public void EnemyTakesDamage(float damage)
    {
        myStats.Health -= damage;
        if (myStats.Health <= 0) EnemyDies();
        else EnemyDropsBlood();
    }

    public void EnemyDropsBlood()
    {
        var blood = Instantiate(GameController.Instance.BloodSplatPrefabs[0], transform.position,Quaternion.identity,GameController.Instance.BloodHolder);
        Destroy(blood, 2);
        blood.GetComponent<SpriteRenderer>().color = enemytype.bloodColor;
    }

    public void EnemyDies()
    {
        // add effects and sounds
        GameController.Instance.PlayerReference.AddPoints(myStats.PointValue,myStats.TimeSecondsValue);
        Instantiate(XpOrb,transform.position,Quaternion.identity,GameController.Instance.XpHolder).GetComponent<XpOrb>().xp = myStats.XpValue;
        EnemyDies_DropLoot();
        Destroy(gameObject);
    }
    private void EnemyDies_DropLoot()
    {
        if (enemytype.LootTable.Length < 1) return; // no loot
        foreach (EnemyCharacter.LootTableStructure possibleLoot in enemytype.LootTable)
        { // try them all, roll for each(simpler to explain to the designers)
            if (possibleLoot.LootChancePercent < Random.Range(0f, 100f) && possibleLoot.LootType != null)
            {
                Instantiate(GameController.Instance.GenericItemPrefab, transform.position, Quaternion.identity).GetComponent<ItemHandler>().ItemType = possibleLoot.LootType;
                return;
            }
        }  
    }

    public void EnemyAttacksPlayer()
    {
        // Still not recovered from last attack on player
        if (myLastAttackTime + myStats.AttackSpeed > Time.time) return;
        // play sound of zombie attacking?!?
        GameController.Instance.PlayerReference.PlayerTakesDamage(myStats.AttackDamage);
        // possibly blood particles?!?
        myLastAttackTime = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Zombie hit something with tag: " + collision.gameObject.tag);
        if ( collision.gameObject.tag == "Walls")
        {
            transform.position -= ((transform.position - lastPosition) * 50);
        }
    }
}
