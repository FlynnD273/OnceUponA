using UnityEngine;

public class ParallaxController : MonoBehaviour
{
    public float offset;
    public float Parallax;
    private Renderer rend;

    public void Start()
    {
        rend = GetComponent<Renderer>();
    }

    public void LateUpdate()
    {
        float height = 2;
        float width = height * Screen.width / Screen.height;
        transform.localScale = new Vector3(width, height, 1);
        offset = transform.parent.transform.position.x / Parallax;
        rend.material.mainTextureOffset = new Vector2(offset, 0);
    }
}
