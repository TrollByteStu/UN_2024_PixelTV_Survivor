using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Bullets : MonoBehaviour
{

    public float Speed;
    public float Damage;
    public Vector3 Direction  = new (1,0,0);
    public GameObject Target;
    public AnimationCurve AnimationCurve;
    public Weapons.WeaponType WeaponType;
    public bool FlipCurve;

    private float StartTime = 0;
    private float LifeTime = 5;
    private Vector3 MoveDirection;
    private Rigidbody2D Rigidbody;

    private void Start()
    {
        StartTime = Time.time;
        Rigidbody = GetComponentInChildren<Rigidbody2D>();
    }

    public void Setup(float speed, float damage, Vector2 direction ,AnimationCurve animationCurve,bool flipCurve,Sprite texture , Weapons.WeaponType type)
    {
        Speed = speed;
        Damage = damage;
        Direction = direction;
        AnimationCurve = animationCurve;
        GetComponentInChildren<SpriteRenderer>().sprite = texture;
        WeaponType = type;
        FlipCurve = flipCurve;
        transform.rotation = Quaternion.LookRotation(transform.position - Target.transform.position, Vector3.forward);
    }

    public void Setup(float speed, float damage, GameObject target, AnimationCurve animationCurve, bool flipCurve, Sprite texture, Weapons.WeaponType type)
    {
        Speed = speed;
        Damage = damage;
        Target = target;
        AnimationCurve = animationCurve;
        GetComponentInChildren<SpriteRenderer>().sprite = texture;
        WeaponType = type;
        FlipCurve = flipCurve;
        transform.rotation = Quaternion.LookRotation(transform.position - Target.transform.position, Vector3.forward);
    }

    private void Update()
    {
        if (WeaponType == Weapons.WeaponType.Bullet)
            transform.position += (Direction + AnimationCurve.Evaluate(Time.time - StartTime) * transform.right * math.pow(-1, Convert.ToInt32(FlipCurve))) * Speed * Time.deltaTime;
        else
        {
            //transform.rotation = Quaternion.LookRotation(Target.transform.position - transform.position,Vector3.forward);
            transform.position = Vector3.MoveTowards(transform.position - 0.05f * AnimationCurve.Evaluate(Time.time - StartTime) * transform.right * math.pow(-1,Convert.ToInt32(FlipCurve)), Target.transform.position, Speed *Time.deltaTime);
        }

        if (Time.time > StartTime + LifeTime)
            Destroy(gameObject);
    }


    public void OnHit(Collider2D collision)
    {
        //Debug.Log("Bullet hit something with tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            //Debug.Log("BulletHits!!!");
            if (collision.GetComponent<Enemy_Main>() != null)
                collision.GetComponent<Enemy_Main>().Damage(Damage);
            Destroy(gameObject);
        }
    }

}
