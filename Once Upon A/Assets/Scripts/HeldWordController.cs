using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Constants;

public class HeldWordController : MonoBehaviour
{
    public GameObject Target;
    public float Speed = 0.1f;
    private Vector2 offset = new(0, 1.8f);

    private TextMesh heldWordMesh;
    private Word heldWord;
    public Word HeldWord
    {
        get
        {
            return heldWord;
        }
        set
        {
            heldWord = value;
            if (value == null)
            {
                heldWordMesh.text = "";
            }
            else
            {
                heldWordMesh.text = value.Text;
                heldWordMesh.color = WordToColor[value.Type];
                var xOffset = 1f * Mathf.Sign(Target.transform.localScale.x);
                transform.position = Target.transform.position + new Vector3(xOffset, -1);
            }
        }
    }

    private Word savedWord;

    void Start()
    {
        heldWordMesh = GetComponent<TextMesh>();
        transform.position = Target.transform.position;
        GameManager.Manager.ResetOccurred += Reset;
        GameManager.Manager.SaveStateOccurred += SaveState;
    }

    private void Reset()
    {
        HeldWord = savedWord;
    }

    private void SaveState()
    {
        savedWord = HeldWord;
    }

    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, (Vector2)Target.transform.position + offset, Speed);
    }
}
