using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Main_Decor : MonoBehaviour
{
    private Tilemap myTM;

    public TileBase[] Decortiles;



    // Start is called before the first frame update
    void Start()
    {
        myTM = GetComponent<Tilemap>();
        myTM.SetTile(new Vector3Int(5, 5, 0), Decortiles[0]);
        myTM.SetTile(new Vector3Int(15, 15, 0), Decortiles[0]);
        int random = Random.Range(0, Decortiles.Length - 1);
        Vector3 playerPos = GameController.Instance.PlayerReference.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
