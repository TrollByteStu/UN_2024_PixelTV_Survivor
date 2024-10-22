using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameController_ObjectPool : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject EnemyPrefab;
    public ObjectPool<GameObject> EnemyPool;
    public int EnemyStartPool = 1500;
    public int EnemyMaxPool = 20000;

    private GameController myGC;

    // Start is called before the first frame update
    void Start()
    {
        myGC = GetComponent<GameController>();

        EnemyPool = new ObjectPool<GameObject>(() => {
            // create object pool
            myGC.currentEnemies++;
            return Instantiate(EnemyPrefab);
        }, go => { // get object there is one available
            myGC.currentEnemies++;
            go.gameObject.SetActive(true);
        }, go => { // return object to pool, room in pool
            myGC.currentEnemies--;
            go.gameObject.SetActive(false);
        }, go => {  // return object, pool is full
            myGC.currentEnemies--;
            Destroy(go.gameObject);
        }, true, EnemyStartPool, EnemyMaxPool);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
