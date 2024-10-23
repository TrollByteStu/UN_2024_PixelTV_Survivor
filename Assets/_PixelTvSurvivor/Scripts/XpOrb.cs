using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpOrb : MonoBehaviour
{
    private SpriteRenderer SpriteRenderer;
    private GameObject Player;

    public float xp;
    public bool MoveToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameController.Instance.PlayerReference.gameObject;
        SpriteRenderer = GetComponent<SpriteRenderer>();
        switch (xp)
        {
            case < 10:
                SpriteRenderer.color = new Color(0, 0, 1);
                break;
            case > 10 and < 25:
                SpriteRenderer.color = new Color(0, 1, 0);
                break;
            case > 25 and < 50:
                SpriteRenderer.color = new Color(1, 0, 0);
                break;
            case > 50 and < 100:
                SpriteRenderer.color = new Color(1, 1, 0);
                break;
        }
        Destroy(gameObject, 20);
    }

    // Update is called once per frame
    void Update()
    {
        if (MoveToPlayer)
        {
            transform.position = Vector3.MoveTowards(transform.position, Player.transform.position , (Vector3.Distance(transform.position, Player.transform.position) + 5)  * Time.deltaTime);
            if (Vector3.Distance(transform.position, Player.transform.position) < 0.1f)
            {
                GameController.Instance.PlayerReference.AddXp(xp);
                Destroy(gameObject);
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
