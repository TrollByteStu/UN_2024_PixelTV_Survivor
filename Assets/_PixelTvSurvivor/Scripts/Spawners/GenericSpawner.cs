using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSpawner : MonoBehaviour
{
    public EnemyCharacter[] listOfSpawnableEnemies;
    public GameObject genericEnemyPrefab;

    public int amountToSpawn;
    public float spawnDelay;

    private float lastSpawn;
    private int spawntype;

    private Vector3 myLocation;

    // Start is called before the first frame update
    void Start()
    {
        myLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ( transform.childCount < amountToSpawn ) 
        {
            if (Time.time > lastSpawn )
            {
                spawntype = Random.Range(0, listOfSpawnableEnemies.Length);
                var enemy = GameController.Instance.EnemyPool_Get();
                enemy.GetComponent<Enemy_Main>().Setup(listOfSpawnableEnemies[spawntype]);
                enemy.transform.position = transform.position;
                enemy.transform.SetParent(transform);
                lastSpawn = Time.time + spawnDelay;
            }
        }
    }
}
