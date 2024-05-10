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
  private float normalSize;
  private ExpDamp zoom;
  private ExpDampVec3 position;

  void Awake()
  {
    cam = GetComponent<Camera>();
    normalSize = cam.orthographicSize;
    zoom = new(normalSize, normalSize, () => { cam.orthographicSize = zoom?.Value ?? 1; transform.localScale = Vector3.one * cam.orthographicSize; });
    position = new(Vector3.zero, Vector3.zero, () => transform.position = position?.Value ?? Vector3.zero);
  }

  void Start()
  {
    position.TargetValue = Target.transform.position;
    position.Value = Target.transform.position;
  }

  void Update()
  {
    if (GameManager.Manager.IsPaused)
    {
      zoom.TargetValue = normalSize * 1.1f;
    }
    else
    {
      zoom.TargetValue = normalSize;
    }

    zoom.Next(ZoomSpeed, Time.unscaledDeltaTime);

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

    position.TargetValue = new Vector3(x, y, -100);
    position.Next(Speed, Time.fixedDeltaTime);
  }
}
