using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Constants;

public class PlayerTriggerVisibilityController : MonoBehaviour
{
    public GameObject[] Visible;
    public GameObject[] Invisible;

    void Start()
    {
        foreach (var go in Visible)
        {
            go.SetActive(true);
        }
        foreach (var go in Invisible)
        {
            go.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == (int)Layers.Player)
        {
            foreach (var go in Visible)
            {
                go.SetActive(false);
            }
            foreach (var go in Invisible)
            {
                go.SetActive(true);
            }
        }
    }
}
