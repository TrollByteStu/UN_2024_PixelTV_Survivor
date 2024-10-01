using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Main : MonoBehaviour
{
    public EnemyCharacter enemytype;
    public EnemyStats myStats;


    public Transform playerRef;

    // Start is called before the first frame update
    void Start()
    {
        playerRef = GameObject.Find("PlayerWannaBeThatZombiesChase").transform;
        myStats = enemytype.Stats;
        transform.name = enemytype.EnemyName;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, playerRef.position, myStats.MoveSpeed * Time.deltaTime);
    }
}
