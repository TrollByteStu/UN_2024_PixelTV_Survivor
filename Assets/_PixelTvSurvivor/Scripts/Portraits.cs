using Unity.VisualScripting;
using UnityEngine;

public class Portraits : MonoBehaviour
{
    private GameObject Player;

    private int Id;

    public bool MoveToPlayer = true;
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
            Destroy(gameObject);

        if (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position, (Vector3.Distance(transform.position, Player.transform.position) + 5) * Time.deltaTime * 0.5f);
            if (Vector3.Distance(transform.position, Player.transform.position) < 0.1f)
            {
                GameController.Instance.PlayerReference.AddPoints(100,0);
                GameController.Instance.PlayerReference.AddCoins(100);
                GameController.Instance.myPP.PickUp(Id);
                Destroy(gameObject);
            }
        }
    }

    public void Setup(int id)
    {
        GetComponent<SpriteRenderer>().sprite = GameController.Instance.myPP.Portraits[id];
        Id = id;
    }
    public void Pickup()
    {
        MoveToPlayer = true;
        transform.tag = "PickedUp";
        Destroy(GetComponent<BoxCollider2D>());
    }
}
