using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Utils;


public class CameraController : MonoBehaviour
{
  public GameObject Target;
  public float HoriThreshold = 2;
  public float VertThreshold = 5;
  public float Speed = 20;

  public float ZoomSpeed = 0.5f;

  private Camera cam;
  private float targetSize;
  private float normalSize;

  // Start is called before the first frame update
  void Start()
  {
    transform.position = Target.transform.position;
    cam = GetComponent<Camera>();
    normalSize = cam.orthographicSize;
  }

  void Update()
  {
    if (GameManager.Manager.IsPaused)
    {
      targetSize = normalSize * 1.1f;
    }
    else
    {
      targetSize = normalSize;
    }

    cam.orthographicSize = Anim(cam.orthographicSize, targetSize, ZoomSpeed);

    /* if (GameManager.Manager.IsPaused) { return; } */
  }

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

    Vector2 newPos = Anim(transform.position, new Vector3(x, y), Speed);
    transform.position = new Vector3(newPos.x, newPos.y, -100);
  }
}
