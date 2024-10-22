using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SurroundSpawner : MonoBehaviour
{

    public AnimationCurve waveSize;

    public EnemyCharacter[] SpawnableEnemiesArray;
    public GameObject Enemyprefab;

    private Transform Player;
    private Transform Holder;

    private readonly float ViewPortSize = 24;

    float LastSpawn;
    private GameObject lastSpawn;

    private void Start()
    {
        Player = GameController.Instance.PlayerReference.transform;
        Holder = GameController.Instance.EnemyHolder;

    }

    private void Update()
    {
        if (LastSpawn + 1 < Time.time)
        {
            
            SpawnWave((int)(waveSize.Evaluate(Time.timeSinceLevelLoad) * math.floor(Time.timeSinceLevelLoad / waveSize.length +1)));
            LastSpawn = Time.time;
        }
    }

    //void Spawn()
    //{
    //    int attempts = 0;
    //    for (int i = 0; i < 10; i++)
    //    {
    //        attempts++;
    //        Spawnpoint = GenerateSpawnpoint();
    //        if (!CanSpawn(Spawnpoint))
    //        {
    //            print("failed spawn" + Spawnpoint);
    //            return;
    //        }
    //        break;
    //    }
    //
    //    Instantiate(Enemyprefab, Spawnpoint + Player.position, Quaternion.identity, Holder).GetComponent<Enemy_Main>().Setup(SpawnableEnemiesArray[0]);
    //}
    //Vector3 GenerateSpawnpoint()
    //{
    //    int attempts = 0;
    //    Vector3 spawnpoint = Vector3.zero;
    //    for ( int i = 0; i < 10; i++)
    //    {
    //        attempts++;
    //        spawnpoint = new Vector3(Random.Range(-MaxDistance,MaxDistance), Random.Range(-MaxDistance, MaxDistance),0);
    //        if (spawnpoint.magnitude < MinDistance || spawnpoint.magnitude > MaxDistance)
    //            break;
    //    }
    //    if (attempts >= 10)
    //    {
    //        return Vector3.up * MinDistance;
    //    }
    //    return spawnpoint;
    //}
    //bool CanSpawn(Vector3 spawnpoint)
    //{
    //    RaycastHit2D[] hits = Physics2D.RaycastAll(Player.position + spawnpoint, Player.position,spawnpoint.magnitude);
    //    foreach (RaycastHit2D hit in hits)
    //    {
    //        if (hit.collider.CompareTag("Walls"))
    //        {
    //            Debug.DrawLine(Player.position + spawnpoint, Player.position , Color.red, 1);
    //            print(hits[0].collider.gameObject.name);
    //            return false;
    //        }
    //    }
    //            Debug.DrawLine(Player.position + spawnpoint , Player.position , Color.green, 1);
    //    return true;
    //}

    void SpawnWave(int amount)
    {
        // computer is dying, skip wave
        if (!GameController.Instance.FPS_isWithinLimit(60)) return;

        // spawn wave
        for (int i = 0; i < amount; i++)
        {
            float angle = Random.Range(-Mathf.PI, Mathf.PI);
            lastSpawn = GameController.Instance.myOP.EnemyPool.Get();
            lastSpawn.transform.position = new Vector3(Mathf.Sin(angle) * (ViewPortSize + Random.Range(-3, 3)), Mathf.Cos(angle) * (ViewPortSize + Random.Range(-3, 3)), 0) + Player.position;
            lastSpawn.transform.SetParent(Holder);
            lastSpawn.GetComponent<Enemy_Main>().Setup(SpawnableEnemiesArray[0]);
            //Instantiate(Enemyprefab, new Vector3(Mathf.Sin(angle)* (ViewPortSize + Random.Range(-3,3)) ,Mathf.Cos(angle)* (ViewPortSize + Random.Range(-3, 3)), 0) + Player.position, Quaternion.identity, Holder).GetComponent<Enemy_Main>().Setup(SpawnableEnemiesArray[0]);
        }
    }
}
