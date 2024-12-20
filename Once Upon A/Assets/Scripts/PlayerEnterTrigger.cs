using UnityEngine;
using static Utils.Constants;

public class PlayerEnterTrigger : TriggerLogic
{
    private bool savedState;

    public override void Init()
    {
        base.Init();
        GameManager.Manager.ResetOccurred += Reset;
        GameManager.Manager.SaveStateOccurred += SaveState;
    }

    private void Reset()
    {
        State = savedState;
    }

    private void SaveState()
    {
        savedState = State;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.layer == (int)Layers.Player)
        {
            State = true;
        }
    }
}
