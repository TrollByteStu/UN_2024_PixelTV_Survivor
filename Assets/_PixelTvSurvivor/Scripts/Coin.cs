using UnityEngine;

public class Coin : MonoBehaviour
{
    private GameObject Player;

    public int Value;
    public bool MoveToPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        Player = GameController.Instance.PlayerReference.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        // kill enemy if to far from player
        if (Vector3.Distance(Player.transform.position, transform.position) > 30)
            GameController.Instance.CoinPool_Release(gameObject);

        if (MoveToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, (Vector3.Distance(transform.position, Player.transform.position) + 5) * Time.deltaTime);
            if (Vector3.Distance(transform.position, Player.transform.position) < 0.1f)
            {
                GameController.Instance.PlayerReference.AddCoins(Value);
                //Destroy(gameObject);
                GameController.Instance.CoinPool_Release(gameObject);
            }
        }
    }

    public void Pickup()
    {
        MoveToPlayer = true;
        transform.tag = "PickedUp";
        Destroy(GetComponent<BoxCollider2D>());
    }
}
