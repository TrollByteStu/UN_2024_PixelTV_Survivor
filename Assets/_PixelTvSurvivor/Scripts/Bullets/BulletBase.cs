using Unity.Mathematics;
using UnityEngine;


public class BulletBase : MonoBehaviour
{

    public float Speed;
    public float Damage;
    public Vector3 Direction  = new (1,0,0);
    public GameObject Target;
    public AnimationCurve AnimationCurve;
    public bool FlipCurve;

    protected float StartTime = 0;
    protected float LifeTime = 5;

    protected void Start()
    {
        StartTime = Time.time;
    }

    public void Setup(float speed, float damage, Vector2 direction ,AnimationCurve animationCurve,bool flipCurve,Sprite texture)
    {
        Speed = speed;
        Damage = damage;
        Direction = direction;
        if (animationCurve != null)
            AnimationCurve = animationCurve;
        else
            AnimationCurve = new AnimationCurve();
        GetComponentInChildren<SpriteRenderer>().sprite = texture;
        FlipCurve = flipCurve;
        transform.rotation = Lookat(direction);
    }

    public void Setup(float speed, float damage, GameObject target, AnimationCurve animationCurve, bool flipCurve, Sprite texture)
    {
        Speed = speed;
        Damage = damage;
        Target = target;
        if (animationCurve != null)
            AnimationCurve = animationCurve;
        else
            AnimationCurve = new AnimationCurve();
        GetComponentInChildren<SpriteRenderer>().sprite = texture;
        FlipCurve = flipCurve;
        transform.rotation = Lookat(Target.transform);

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
