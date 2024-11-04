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

    public bool Spawnb;

    private Transform Player;
    private Transform Holder;

    private readonly float ViewPortSize = 24;

    float LastSpawn;
    private GameObject lastSpawn;

    public int maxEnemeyCost = 1;

    [Serializable]
    public struct EnemyCost
    {
        public EnemyCharacter Enemy;
        public int Cost;
        public int ChanceSize;
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
    public void StartFromGameController(Transform player, Transform holder)
    {
        Player = player;
        Holder = holder;
        LastSpawn = 0f;
    }

    private void Update()
    {
        if (Player == null) return;

        if (LastSpawn + 1 < Time.timeSinceLevelLoad)
        {
            maxEnemeyCost = (int)math.floor(Time.timeSinceLevelLoad / waveSize.keys[^1].time + 1);
            RandomSpawns();
            SpawnWave((int)(waveSize.Evaluate(Time.timeSinceLevelLoad) * math.floor(Time.timeSinceLevelLoad / waveSize.keys[^1].time + 1)));
            LastSpawn = Time.timeSinceLevelLoad;

        }
    }

    void RandomSpawns()
    {
        foreach (var enemy in RandomEnemies)
        {
            if (Random.Range(1 , enemy.chance) == 1)
            {
                Spawn(enemy.enemy);
            }
        }
    }
    async void SpawnWave(int amount)
    {
        // computer is dying, skip wave
        if (!GameController.Instance.FPS_isWithinLimit(50)) return;
        int cost=1;
        // spawn wave
        for (int i = 0; i < amount; i += cost)
        {
            int[] availableEnemies = GetEnemyOfCost(maxEnemeyCost);
            int enemy = Random.Range(0 , availableEnemies.Length);

            if (SpawnableEnemiesArray[enemy].Cost > 0)
                cost = SpawnableEnemiesArray[enemy].Cost;
            else
                cost = 1;

            Spawn(SpawnableEnemiesArray[enemy].Enemy);

            await Awaitable.WaitForSecondsAsync(2/amount);
        }
    }

    int[] GetEnemyOfCost(int cost)
    {
        int[] array = new int[0];
        for (int i = 0; i < SpawnableEnemiesArray.Length; i++)
        {
            if (SpawnableEnemiesArray[i].Cost > cost)
                continue;
            Array.Resize(ref array, array.Length +1);
            array[^1] = i;
        }
        return array;
    }
    int GetEnemyFromChance(int[] enemyArray)
    {
        int randomMax = 0;
        foreach (int enemy in enemyArray)
        {
            randomMax += SpawnableEnemiesArray[enemy].ChanceSize;
        }
        int randomOutput = Random.Range(1, randomMax);
        foreach (int enemy in enemyArray)
        {
            randomOutput -= SpawnableEnemiesArray[enemy].ChanceSize;
            if (randomOutput <= 0)
                return enemy;
        }
        return enemyArray[^1];
    }
    void Spawn(EnemyCharacter enemy)
    {
        float angle = Random.Range(-Mathf.PI, Mathf.PI);
        lastSpawn = GameController.Instance.EnemyPool_Get();
        lastSpawn.transform.position = new Vector3(Mathf.Sin(angle) * (ViewPortSize + Random.Range(-3, 3)), Mathf.Cos(angle) * (ViewPortSize + Random.Range(-3, 3)), 0) + Player.position;
        lastSpawn.transform.SetParent(Holder);
        lastSpawn.GetComponent<Enemy_Main>().Setup(enemy);
    }
}
