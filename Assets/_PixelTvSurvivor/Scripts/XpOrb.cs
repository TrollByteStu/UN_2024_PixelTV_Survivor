using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XpOrb : MonoBehaviour
{
    public float xp;
    public SpriteRenderer SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
