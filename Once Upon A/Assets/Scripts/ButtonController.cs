using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class ButtonController : MonoBehaviour
{
    private Renderer text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Renderer>();
    }

    void OnMouseOver()
    {
        text.material.color = new Color(1, 1, 1, 0.5f);
    }

    void OnMouseExit()
    {
        text.material.color = new Color(1, 1, 1);
    }

    void OnMouseDown()
    {
        OnClick();
    }

    public abstract void OnClick();
}
