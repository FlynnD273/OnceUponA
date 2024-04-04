using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
    private bool didInit;

    public event Action StateChanged;
    private bool state;
    public bool State
    {
        get
        {
            return state;
        }
        internal set
        {
            if (state != value)
            {
                state = value;
                StateChanged?.Invoke();
            }
        }
    }

    public void Start()
    {
        if (!didInit)
        {
            Init();
            didInit = true;
        }
    }

    public virtual void Init()
    {
        State = true;
        State = false;
    }
}
