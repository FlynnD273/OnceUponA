using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Utils.Utils;

public class ParallaxController : MonoBehaviour
{
    public float offset;
    public float Parallax;
    private float width;
    private Vector3 scale;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        float height = 2;
        float width = height * Screen.width / Screen.height;
        transform.localScale = new Vector3(width, height, 1);
        /* width = GetComponent<Renderer>().bounds.size.x; */
        /* twin = new GameObject("Background 2"); */
        /* twin.transform.SetParent(transform); */
        /* twin.transform.localPosition = new Vector3(width / transform.localScale.x, 0, 0); */
        /* twin.transform.localScale = Vector3.one; */
        /* var rend = twin.AddComponent<SpriteRenderer>(); */
        /* var thisrend = GetComponent<SpriteRenderer>(); */
        /* rend.sprite = thisrend.sprite; */
        /* rend.sortingOrder = thisrend.sortingOrder; */
        /* scale = transform.localScale; */
    }

    void LateUpdate()
    {
        float camScale = Camera.main.orthographicSize / 15;
        float camWidth = width * camScale;
        /* transform.localScale = scale * camScale; */
        offset = (transform.parent.transform.position.x / Parallax); // - transform.parent.transform.position.x;
        /* offset *= camScale; */
        /* offset /= 100; */
        rend.material.mainTextureOffset = new Vector2(offset, 0);
        /* offset = (((offset % camWidth) + camWidth) % camWidth) - camWidth; */
        /* transform.localPosition = new Vector3(offset, 0, transform.localPosition.z); */
    }
}
