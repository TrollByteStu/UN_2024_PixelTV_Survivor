using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class SurroundSpawner : MonoBehaviour
{

    public AnimationCurve waveSize;

    public EnemyCost[] SpawnableEnemiesArray;
    public RandomEnemy[] RandomEnemies;
    public GameObject Enemyprefab;

    public bool Spawn;

    private Transform Player;
    private Transform Holder;

    private readonly float ViewPortSize = 24;

    float LastSpawn;
    private GameObject lastSpawn;


    [Serializable]
    public struct EnemyCost
    {
        public EnemyCharacter Enemy;
        public int Cost;
    }
    [Serializable]
    public struct RandomEnemy
    {
        public EnemyCharacter enemy;
        public int chance;
    }
    private void Start()
    {
        if (GameController.Instance.PlayerReference == null) return;
        Player = GameController.Instance.PlayerReference.transform;
        Holder = GameController.Instance.EnemyHolder;
    }

    private void Update()
    {
        if (Player == null) return;

        if (LastSpawn + 1 < Time.time)
        {
            RandomSpawns();
            SpawnWave((int)(waveSize.Evaluate(Time.timeSinceLevelLoad) * math.floor(Time.timeSinceLevelLoad / waveSize.length +1)));
            LastSpawn = Time.time;

        }
    }

    void RandomSpawns()
    {
        foreach (var enemy in RandomEnemies)
        {
            if (Random.Range(1 , enemy.chance) == 1)
            {
                spawn(enemy.enemy);
            }
        }
    }
    void SpawnWave(int amount)
    {
        // computer is dying, skip wave
        if (!GameController.Instance.FPS_isWithinLimit(50)) return;

        // spawn wave
        for (int i = 0; i < amount; i += 1)
        {
            spawn(SpawnableEnemiesArray[Random.Range(0, SpawnableEnemiesArray.Length)].Enemy);
        }
    }
    void spawn(EnemyCharacter enemy)
    {
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        lastSpawn = GameController.Instance.EnemyPool_Get();
        lastSpawn.transform.position = new Vector3(Mathf.Sin(angle) * (ViewPortSize + Random.Range(-3, 3)), Mathf.Cos(angle) * (ViewPortSize + Random.Range(-3, 3)), 0) + Player.position;
        lastSpawn.transform.SetParent(Holder);
        lastSpawn.GetComponent<Enemy_Main>().Setup(enemy);
    }
}
