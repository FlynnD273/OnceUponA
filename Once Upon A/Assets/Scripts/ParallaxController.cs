using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour
{
  public float Parallax;
  private float width;
  private GameObject twin;
  new private Camera camera;
  private Vector3 scale;

  void Awake()
  {
    width = GetComponent<Renderer>().bounds.size.x;
    twin = new GameObject("Background 2");
    twin.transform.SetParent(transform);
    twin.transform.localPosition = new Vector3(width / transform.localScale.x, 0, 0);
    twin.transform.localScale = Vector3.one;
    var rend = twin.AddComponent<SpriteRenderer>();
    var thisrend = GetComponent<SpriteRenderer>();
    rend.sprite = thisrend.sprite;
    rend.sortingOrder = thisrend.sortingOrder;
    scale = transform.localScale;
    camera = transform.parent.GetComponent<Camera>();
  }

  void LateUpdate()
  {
    float camScale = camera.orthographicSize / 15;
    float camWidth = width * camScale;
    transform.localScale = scale * camScale;
    float offset = (transform.parent.transform.position.x / Parallax) - transform.parent.transform.position.x;
    offset *= camScale;
    offset = (((offset % camWidth) + camWidth) % camWidth) - camWidth;
    transform.localPosition = new Vector3(offset, 0, transform.localPosition.z);
  }
}
