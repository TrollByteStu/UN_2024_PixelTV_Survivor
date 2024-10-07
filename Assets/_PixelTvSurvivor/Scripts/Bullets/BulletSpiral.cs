using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BulletSpiral : BulletBase
{
    float ClimbSpeed;
    float offset;
    public Vector3 StartPos;
    float SpinDirection;

    // Update is called once per frame
    void Update()
    {
        transform.position = StartPos - new Vector3(SpinDirection * math.sin((offset + Time.time - StartTime)* Speed)* (Time.time - StartTime) * ClimbSpeed, math.cos((offset + Time.time - StartTime) * Speed) * (Time.time - StartTime) * ClimbSpeed, 0);
    }
    

    public void Setup(float angle, float spinDirection, float climbSpeed,float speed, float damage,Vector3 position)
    {
        offset = angle;
        SpinDirection = spinDirection;
        ClimbSpeed = climbSpeed;
        Speed = speed;
        Damage = damage;
        StartPos = position;
    }
}
