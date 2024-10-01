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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ( transform.childCount < amountToSpawn ) 
        {
            if (Time.time > lastSpawn )
            {
                var enemy = Instantiate( genericEnemyPrefab );
                enemy.transform.SetParent(transform);
                enemy.transform.localPosition = Vector3.zero;
                lastSpawn = Time.time + spawnDelay;
            }
        }
    }
}
