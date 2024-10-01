using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullets : MonoBehaviour
{

    public float Speed;
    public float Damage;
    public Vector3 Direction  = new (1,0,0);

    private float StartTime = 0;
    private float LifeTime = 10;

    private void Start()
    {
        StartTime = Time.time;
    }

    public void Setup(float speed, float damage, Vector2 direction)
    {
        Speed = speed;
        Damage = damage;
        Direction = direction;
    }

    private void Update()
    {
        transform.position += Direction * Speed * Time.deltaTime;

        if (Time.time > StartTime + LifeTime)
            Destroy(gameObject);
    }
}
