using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Renderer>().enabled = false;
    }
}
