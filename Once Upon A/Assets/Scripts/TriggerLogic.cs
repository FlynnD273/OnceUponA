using System;
using UnityEngine;

public class TriggerLogic : MonoBehaviour
{
    public bool DebugState;
    public TriggerLogic[] InvolvedSlots = new TriggerLogic[0];
    private bool didInit;

    public event Action StateChanged;
    internal bool state = true;
    public bool State
    {
        get => state;
        internal set
        {
            if (state != value)
            {
                state = value;
                DebugState = state;
                StateChanged?.Invoke();
            }
        }
    }

    public void Update()
    {
        if (GameManager.Manager.IsPaused) { return; }

        if (!didInit)
        {
            Init();
            didInit = true;
        }

        InheritedUpdate();
    }

    internal virtual void InheritedUpdate() { }

    public virtual void Init()
    {
        State = false;
    }
}
