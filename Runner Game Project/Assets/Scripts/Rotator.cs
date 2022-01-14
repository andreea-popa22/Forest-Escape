using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour

{
    float maxAngle = 15;
    float speed = 100;

    private void FixedUpdate()
    {
        float z = transform.rotation.eulerAngles.z;
        if (z > 360 - maxAngle)
            z -= 360;
        if (Mathf.Abs(z) > maxAngle)
        {
            speed = -speed;
            transform.rotation.eulerAngles.Set(0, 0, maxAngle - 1);
        }

        transform.Rotate(new Vector3(0, 0, speed * Time.fixedDeltaTime));
    }
}
