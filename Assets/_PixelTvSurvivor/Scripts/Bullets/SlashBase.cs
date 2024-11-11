using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SlashBase : MonoBehaviour
{
    float Damage;
    public void OnFinish()
    {
        Destroy(gameObject);
    }
    public void Setup(Sprite sprite, Vector3 scale, Vector3 direction,float damage)
    {

        if (GetComponent<SpriteRenderer>() != null) 
            GetComponent<SpriteRenderer>().sprite = sprite;
        else
            GetComponentInChildren<SpriteRenderer>().sprite = sprite;

        Damage = damage;

        transform.localScale = scale;
        transform.rotation = Lookat(direction);
        transform.SetParent(GameController.Instance.PlayerReference.transform);
    }

    quaternion Lookat(Vector3 Direction)
    {
        if (Direction.x < 0)
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
        float angle = Mathf.Atan2(Direction.y, Direction.x);
        return quaternion.AxisAngle(Vector3.forward, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
            collision.GetComponent<Enemy_Main>().EnemyTakesDamage(Damage);
    }
}
