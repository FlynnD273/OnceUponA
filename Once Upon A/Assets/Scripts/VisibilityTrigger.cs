using UnityEngine;

public class VisibilityTrigger : MonoBehaviour
{
    public bool Deactivated;
    public TriggerLogic Trigger;
    public DynamicText[] Visible;
    public DynamicText[] Invisible;

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
        if (Deactivated || Trigger == null)
        {
            return;
        }
        foreach (DynamicText go in Visible)
        {
            go.SetVisibility(!Trigger.State, this);
        }
        foreach (DynamicText go in Invisible)
        {
            go.SetVisibility(Trigger.State, this);
        }

        if (Trigger.State)
        {
            foreach (TriggerLogic go in Trigger.InvolvedSlots)
            {
                WordSlotController slot = go.GetComponent<WordSlotController>();
                if (slot != null)
                {
                    slot.IsSwappable = false;
                }
            }
            GameManager.Manager?.JustActivated();
        }
    }
}
