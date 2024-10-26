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

    [Header("BloodSplats")]
    public GameObject BloodSplatPrefab;
    public ObjectPool<GameObject> BloodPool;
    public int BloodStartPool = 500;
    public int BloodMaxPool = 2000;

    private GameController myGC;

    // Start is called before the first frame update
    public void Start()
    {
        myGC = GetComponent<GameController>();

        // run the inital configure of the object pools
        ConfigureEnemyPool();
        ConfigureBloodPool();

    }

    void ConfigureEnemyPool()
    {
        EnemyPool = new ObjectPool<GameObject>(() => {
            // create object pool
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
        // Still trying to access old pool 2nd game
        EnemyPool.Clear();
    }

    void ConfigureBloodPool()
    {
        BloodPool = new ObjectPool<GameObject>(() => {
            // create object pool
            return Instantiate(BloodSplatPrefab);
        }, go => { // get object there is one available
            myGC.currentBloodSplats++;
            go.gameObject.SetActive(true);
        }, go => { // return object to pool, room in pool
            myGC.currentBloodSplats--;
            go.gameObject.SetActive(false);
        }, go => {  // return object, pool is full
            myGC.currentBloodSplats--;
            Destroy(go.gameObject);
        }, false, BloodStartPool, BloodMaxPool);
        // Still trying to access old pool 2nd game
        BloodPool.Clear();
    }


}
