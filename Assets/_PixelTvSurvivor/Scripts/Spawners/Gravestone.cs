using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravestone : MonoBehaviour
{
    public EnemyCharacter SpawnableEnemy;

    public GameObject genericEnemyPrefab;
    public Sprite[] PossibleGraveStoneSprites;

    private int spawnsLeft = 2;
    private float spawnTimer;

    private Vector3 myLocation;

    // Start is called before the first frame update
    void Start()
    {
        spawnTimer = Time.time + 10;
        GetComponent<SpriteRenderer>().sprite = PossibleGraveStoneSprites[Random.Range(0, PossibleGraveStoneSprites.Length - 1)];
        myLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ( Time.time > spawnTimer)
        {
            var enemy = GameController.Instance.EnemyPool_Get();
            enemy.GetComponent<Enemy_Main>().Setup(SpawnableEnemy);
            enemy.transform.position = myLocation;
            if ( spawnsLeft > 1)
            { // more to spawn
                spawnsLeft--;
                spawnTimer = Time.time + 10;
            } else
            {// last one
                Destroy(gameObject);
            }
        }
    }
}
