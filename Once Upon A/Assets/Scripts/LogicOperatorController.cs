using System.Linq;

public partial class LogicOperatorController : TriggerLogic
{
    public TriggerLogic[] Slots;
    public LogicType Operator;

    public override void Init()
    {
        base.Init();
        foreach (TriggerLogic slot in Slots)
        {
            slot.StateChanged += UpdateState;
        }
        InvolvedSlots = Slots;
        UpdateState();
    }

    private void UpdateState()
    {
        bool cond = false;
        cond = Operator switch
        {
            LogicType.Or => Slots.Any(static x => x.State),
            LogicType.And => Slots.All(static x => x.State),
            _ => false,
        };
        State = cond;
    }
}
