using UnityEngine;

public class GameController_Steamrollers : MonoBehaviour
{

    public GameObject SteamrollerPrefab;

    public bool spawnNow = false;


    private GameController myGC;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myGC = GameController.Instance;

    }

    // Update is called once per frame
    void Update()
    {

        if (spawnNow)
        {
            spawnNow = false;
            DoSpawnNow();
        }
    }

    void DoSpawnNow()
    {
        var direction = new Vector3(10, 0, 0);
        direction.x += Random.Range(-5, 5);
        direction.y += Random.Range(-5, 5);
        float distance = Random.Range(0f, 5f)+5;
        for (int i = 0; i < 5; i++)
        {
            var steam = Instantiate(SteamrollerPrefab).GetComponent<Steamroller>();
            steam.direction = direction;
            steam.transform.position = new Vector3(-20, (i*distance)-10, 0)+myGC.PlayerReference.transform.position;
        }
    }


}
