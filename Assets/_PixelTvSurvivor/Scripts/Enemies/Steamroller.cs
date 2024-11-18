using UnityEngine;

public class Steamroller : MonoBehaviour
{

    public Vector3 direction;

    private PlayerController playerRef;
    private AudioSource myAS;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRef = GameController.Instance.PlayerReference;
        myAS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * Time.deltaTime;

        myAS.volume = (15 - Vector3.Distance(playerRef.transform.position, transform.position))*0.1f;

        // kill enemy if to far from player
        if (Vector3.Distance(playerRef.transform.position, transform.position) > 30)
            Destroy(gameObject);

        // kill this noise of player is dead
        if (playerRef.Stats.Health < 1f)
            Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            GameController.Instance.EnemyPool_Release(collision.gameObject);
        }
        if (collision.transform.tag == "Player")
        {
            playerRef.PlayerTakesDamage(50);
        }
    }

}
