using UnityEngine;

public class GameController_Steamrollers : MonoBehaviour
{

    public AudioSource AnnouncerASRef;
    public AudioClip[] AnnouncementList;

    public GameObject SteamrollerPrefab;
    public float spawnInterval = 60f;
    public bool spawning = false;
    public bool spawnNow = false;
    public bool playNow = false;


    private GameController myGC;
    private float nextSpawn = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myGC = GameController.Instance;
        nextSpawn = Time.timeSinceLevelLoad + spawnInterval;
        spawning = false;
    }
    public void StartFromGameController()
    {
        spawning = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!spawning) return;
        if (Time.timeSinceLevelLoad > nextSpawn)
        {
            if (!playNow) DoPlayNow();
        }
        if (playNow && !AnnouncerASRef.isPlaying)
        { DoSpawnNow(); }
    }
    void DoPlayNow()
    {
        playNow = true;
        AnnouncerASRef.clip = AnnouncementList[Random.Range(0,AnnouncementList.Length)];
        AnnouncerASRef.Play();
    }
    void DoSpawnNow()
    {
        nextSpawn = Time.timeSinceLevelLoad + spawnInterval;
        spawnNow = false;
        playNow = false;
        bool flip = false;
        if (Random.value > 0.5f) flip = true;
        var direction = new Vector3(10, 0, 0);
        direction.x += Random.Range(-5, 5);
        direction.y += Random.Range(-5, 5);
        float distance = Random.Range(0f, 5f)+5;
        for (int i = 0; i < 5; i++)
        {
            var steam = Instantiate(SteamrollerPrefab).GetComponent<Steamroller>();
            steam.direction = direction;
            if (flip)
            { // heads
                steam.direction.x *= -1f;
                steam.transform.position = new Vector3(20, (i * distance) - 10, 0) + myGC.PlayerReference.transform.position;
                steam.transform.localScale = new Vector3(-1,1,0);
            } else { // tails
                steam.transform.position = new Vector3(-20, (i * distance) - 10, 0) + myGC.PlayerReference.transform.position;
            }
        }
    }


}
