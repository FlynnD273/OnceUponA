using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
  public float minX;
  public float maxX;

  void LateUpdate()
  {
    float camPercent = (transform.parent.transform.position.x - minX) / (maxX - minX);
    float x = camPercent * 17 * 2 - 16;
    transform.localPosition = new Vector3(-x, 0, transform.localPosition.z);
  }
}
