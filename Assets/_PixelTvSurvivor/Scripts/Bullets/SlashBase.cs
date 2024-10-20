using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashBase : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject,1);
    }

    public void Setup(Sprite sprite, Vector3 scale)
    {

        if (GetComponent<SpriteRenderer>() != null) 
            GetComponent<SpriteRenderer>().sprite = sprite;
        else
            GetComponentInChildren<SpriteRenderer>().sprite = sprite;
        transform.localScale = scale;

    }
}
