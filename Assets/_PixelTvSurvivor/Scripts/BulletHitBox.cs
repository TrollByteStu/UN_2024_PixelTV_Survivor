using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHitBox : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GetComponentInParent<Bullets>().OnHit(collision);
    }
}
