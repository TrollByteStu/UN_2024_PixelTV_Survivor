using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletBasic : BulletBase
{
    protected override void Start()
    {
        if (Direction.x < 0)
            transform.localScale = new Vector3(1, -1, 1);
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += (Direction + AnimationCurve.Evaluate(Time.time - StartTime) * transform.up * math.pow(-1, Convert.ToInt32(FlipCurve))) * Speed * Time.deltaTime;

    }
}
