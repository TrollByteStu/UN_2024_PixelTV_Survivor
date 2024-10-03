using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullets : MonoBehaviour
{

    public float Speed;
    public float Damage;
    public Vector3 Direction  = new (1,0,0);
    public GameObject Target;
    public AnimationCurve AnimationCurve;
    public Weapons.WeaponType WeaponType;

    private float StartTime = 0;
    private float LifeTime = 5;

    private void Start()
    {
        StartTime = Time.time;
    }

    public void Setup(float speed, float damage, Vector2 direction ,AnimationCurve animationCurve,Sprite texture , Weapons.WeaponType type)
    {
        Speed = speed;
        Damage = damage;
        Direction = direction;
        AnimationCurve = animationCurve;
        GetComponentInChildren<SpriteRenderer>().sprite = texture;
        WeaponType = type;
    }

    public void Setup(float speed, float damage, GameObject target, AnimationCurve animationCurve, Sprite texture, Weapons.WeaponType type)
    {
        Speed = speed;
        Damage = damage;
        Target = target;
        AnimationCurve = animationCurve;
        GetComponentInChildren<SpriteRenderer>().sprite = texture;
        WeaponType = type;
    }

    private void Update()
    {
        if (WeaponType == Weapons.WeaponType.Bullet)
            transform.position += (Direction + AnimationCurve.Evaluate(Time.time - StartTime) * transform.right) * Speed * Time.deltaTime;
        else
        {
            //Quaternion.LookRotation(Target.transform.position);
            transform.position = Vector3.MoveTowards(transform.position - 0.05f * AnimationCurve.Evaluate(Time.time - StartTime) * transform.right, Target.transform.position, Speed *Time.deltaTime);

        }

        if (Time.time > StartTime + LifeTime)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Bullet hit something with tag: " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("BulletHits!!!");
            collision.GetComponent<Enemy_Main>().Damage(Damage);
            Destroy(gameObject);
        }
    }

}
