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
        lastPosition = thisPosition;
        thisPosition = transform.position;
        if ( statusStunTimerLeft <= 0 )
            transform.position = Vector3.MoveTowards(transform.position, playerRef.position, myStats.MoveSpeed * Time.deltaTime);
        if (Vector3.Distance(transform.position, playerRef.position) < myStats.AttackRange) EnemyAttacksPlayer();
        if ( statusStunTimerLeft > 0 )
        {
            statusStunTimerLeft -= Time.deltaTime;
            if ( statusStunTimerLeft < 0 ) statusStunTimerLeft = 0;
        }
    }

    public void EnemyTakesDamage(float damage)
    {
        myStats.Health -= damage;
        if (myStats.Health <= 0) EnemyDies();
        else EnemyDropsBlood();
    }

    public void EnemyDropsBlood()
    {
        var blood = Instantiate(GameController.Instance.BloodSplatPrefabs[0], transform.position,Quaternion.identity);
        Destroy(blood, 2);
        blood.GetComponent<SpriteRenderer>().color = enemytype.bloodColor;
    }

    public void EnemyDies()
    {
        // add effects and sounds
        // spawn pickups?
        GameController.Instance.PlayerReference.AddPoints(myStats.PointValue,myStats.TimeSecondsValue);
        Instantiate(XpOrb,transform.position,Quaternion.identity).GetComponent<XpOrb>().xp = myStats.XpValue;
        //GameController.Instance.PlayerReference.AddXp(myStats.XpValue);
        Destroy(gameObject);
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
