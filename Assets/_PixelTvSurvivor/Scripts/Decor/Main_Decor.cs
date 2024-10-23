using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Main_Decor : MonoBehaviour
{
    private Tilemap myTM;
    
    public TileBase[] Decortiles;

    Vector3 playerPos;
    Vector3Int testPos;
    bool decorationFound;

    // Start is called before the first frame update
    void Start()
    {
        myTM = GetComponent<Tilemap>();
        playerPos = GameController.Instance.PlayerReference.transform.position;


        for (int i = 1; i < 100; i++)
        {
            playerPos = GameController.Instance.PlayerReference.transform.position;
            int random = Random.Range(0, 360);
            testPos = Vector3Int.RoundToInt(playerPos + new Vector3(Mathf.Cos(random) * (int)(i*0.2), Mathf.Sin(random) * (int)(i * 0.2)));

            decorationFound = false;
            for (int x = -5; x < 5; x++)
            {
                for (int y = -5; y < 5; y++)
                {
                    if (myTM.GetTile(testPos + new Vector3Int(x, y, 0)) != null)
                    {
                        decorationFound = true;
                    }
                }
            }

            if (!decorationFound)
            {
                myTM.SetTile(testPos, Decortiles[Random.Range(0,Decortiles.Length)]);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameController.Instance.PlayerReference.transform.position;
        int random = Random.Range(0, 360);
        testPos = Vector3Int.RoundToInt(playerPos + new Vector3(Mathf.Cos(random) * 20, Mathf.Sin(random) * 20));

        decorationFound = false;
        for (int x = -5; x < 5; x++)
        {
            for (int y = -5; y < 5; y++)
            {
                if ( myTM.GetTile(testPos+ new Vector3Int(x,y,0)) != null)
                {
                    decorationFound = true;
                }
            }
        }

        if ( !decorationFound )
        {
            myTM.SetTile(testPos, Decortiles[Random.Range(0, Decortiles.Length)]);
        }
        
    }
    private Vector3Int testpos(Vector3Int testPost, int size)
    {
        return new Vector3Int(testPos.x + size, testPos.y + size, 0);
    }
}
