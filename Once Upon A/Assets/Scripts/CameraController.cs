using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Target;
    public float HoriThreshold = 2;
    public float VertThreshold = 5;
    public float Speed = 20;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Target.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 deltaPos = Target.transform.position - transform.position;
        float x = transform.position.x, y = transform.position.y;

        if (deltaPos.x > HoriThreshold)
        {
            x = Target.transform.position.x - HoriThreshold;
        }
        else if (deltaPos.x < -HoriThreshold)
        {
            x = Target.transform.position.x + HoriThreshold;
        }

        if (deltaPos.y > VertThreshold)
        {
            y = Target.transform.position.y - VertThreshold;
        }
        else if (deltaPos.y < -VertThreshold)
        {
            y = Target.transform.position.y + VertThreshold;
        }

        Vector2 newPos = Vector2.Lerp(transform.position, new Vector2(x, y), Math.Min(1, Speed * Time.fixedDeltaTime));
        transform.position = new Vector3(newPos.x, newPos.y, -100);
    }
}
