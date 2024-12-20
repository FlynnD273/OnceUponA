using UnityEngine;

public abstract class ButtonController : MonoBehaviour
{
    private Renderer text;

    public void Start()
    {
        text = GetComponent<Renderer>();
    }

    public void OnMouseOver()
    {
        text.material.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnMouseExit()
    {
        text.material.color = new Color(1, 1, 1);
    }

    public void OnMouseDown()
    {
        OnClick();
    }

    public abstract void OnClick();
}
