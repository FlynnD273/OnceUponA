using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTrigger : MonoBehaviour
{
    public TriggerLogic Trigger;
    public GameObject[] Visible;
    public GameObject[] Invisible;

    public void Start()
    {
        if (Trigger == null)
        {
            Trigger = GetComponent<TriggerLogic>();
        }
        Trigger.StateChanged += StateChanged;
    }

    public void StateChanged()
    {
        foreach (var go in Visible)
        {
            go.SetActive(!Trigger.State);
        }
        foreach (var go in Invisible)
        {
            go.SetActive(Trigger.State);
        }
    }

}
