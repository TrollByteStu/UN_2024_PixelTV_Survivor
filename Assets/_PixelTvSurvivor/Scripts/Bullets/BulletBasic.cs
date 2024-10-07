using System;
using Unity.Mathematics;
using UnityEngine;

public class BulletBasic : BulletBase
{

    // Update is called once per frame
    void Update()
    {
        transform.position += (Direction + AnimationCurve.Evaluate(Time.time - StartTime) * transform.up * math.pow(-1, Convert.ToInt32(FlipCurve))) * Speed * Time.deltaTime;

        transform.localScale = new Vector3(1, math.pow(-1, Convert.ToInt32(transform.rotation.z < 0)), 1);
    }
}
