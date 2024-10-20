using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class BulletHoming : BulletBase
{


    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Lookat(Target.transform.position);


        if (Target != null) // homing bullet moving towards a target that had been destroyed by another bullet crashed the game
            transform.position = Vector3.MoveTowards(transform.position - 0.5f * AnimationCurve.Evaluate(Time.time - StartTime) * transform.right * math.pow(-1, Convert.ToInt32(FlipCurve)), Target.transform.position, Speed * Time.deltaTime);
        else
            Destroy(gameObject);


        //transform.localScale = new Vector3(1, math.pow(-1, Convert.ToInt32(transform.rotation.z < 90 || transform.rotation.z > 270)), 1);

    }
}
