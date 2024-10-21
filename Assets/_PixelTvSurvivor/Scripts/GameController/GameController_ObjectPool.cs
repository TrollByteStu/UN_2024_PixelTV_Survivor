using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class GameController_ObjectPool : MonoBehaviour
{
    [Header("Enemies")]
    public GameObject EnemyPrefab;
    public ObjectPool<GameObject> EnemyPool;
    public int EnemyStartPool = 500;
    public int EnemyMaxPool = 2000;

    // Start is called before the first frame update
    void Start()
    {

        EnemyPool = new ObjectPool<GameObject>(() => {
            // create object pool
            return Instantiate(EnemyPrefab);
        }, go => { // get object there is one available
            go.gameObject.SetActive(true);
        }, go => { // return object to pool, room in pool
            go.gameObject.SetActive(false);
        }, go => {  // return object, pool is full
            Destroy(go.gameObject);
        }, true, EnemyStartPool, EnemyMaxPool);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
