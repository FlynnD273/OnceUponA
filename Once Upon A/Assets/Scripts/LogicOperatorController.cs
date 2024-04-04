using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogicOperatorController : TriggerLogic
{
    public enum LogicType { And, Or }

    public TriggerLogic[] Slots;
    public LogicType Operator;

    public override void Init()
    {
        base.Init();
        foreach (var slot in Slots)
        {
            slot.StateChanged += UpdateState;
        }
        UpdateState();
    }

    private void UpdateState()
    {
        bool cond = false;
        switch (Operator)
        {
            case LogicType.Or:
                cond = Slots.Any(x => x.State);
                break;
            case LogicType.And:
                cond = Slots.All(x => x.State);
                break;
            default:
                cond = false;
                break;
        }

				State = cond;
    }
}
