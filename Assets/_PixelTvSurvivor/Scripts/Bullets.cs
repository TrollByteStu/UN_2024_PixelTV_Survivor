using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

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

    private void Start()
    {
        StartTime = Time.time;
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
        transform.rotation = Lookat(direction);
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
        transform.rotation = Lookat(Target.transform);

    }

    private void Update()
    {
        if (WeaponType == Weapons.WeaponType.Bullet)
            transform.position += (Direction + AnimationCurve.Evaluate(Time.time - StartTime) * transform.up * math.pow(-1, Convert.ToInt32(FlipCurve))) * Speed * Time.deltaTime;
        else
        {
            //transform.rotation = Lookat(Target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position - 0 * AnimationCurve.Evaluate(Time.time - StartTime) * transform.right * math.pow(-1,Convert.ToInt32(FlipCurve)), Target.transform.position, Speed *Time.deltaTime);
        }

        transform.localScale = new Vector3(1,math.pow(-1,Convert.ToInt32(transform.rotation.z < 0)),1);
        if (Time.time > StartTime + LifeTime)
            Destroy(gameObject);
    }

    public quaternion Lookat(Transform Target)
    {
        Vector3 Direction = Target.position - transform.position;
        float angle = Mathf.Atan2(Direction.y, Direction.x);
        return quaternion.AxisAngle( Vector3.forward,angle);
    }    
    public quaternion Lookat(Vector3 Direction)
    {
        float angle = Mathf.Atan2(Direction.y, Direction.x);
        return quaternion.AxisAngle( Vector3.forward,angle);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.GetComponent<Enemy_Main>() != null)
                collision.GetComponent<Enemy_Main>().EnemyTakesDamage(Damage);
            Destroy(gameObject);
        }
    }

}
