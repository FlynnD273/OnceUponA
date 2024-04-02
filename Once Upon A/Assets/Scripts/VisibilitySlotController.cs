using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilitySlotController : WordSlotController
{
    public GameObject Target;
    public string triggerWord;
    public bool IsVisibleDefault;

    internal override void Init()
    {
        Triggered(null);
    }

    internal override void Triggered(Word oldWord)
    {
        bool cond;
        if (string.IsNullOrWhiteSpace(triggerWord))
        {
            cond = !string.IsNullOrWhiteSpace(CurrentWord?.Text);
        }
        else
        {
            cond = CurrentWord?.Text == triggerWord;
        }
        Target.SetActive(cond ^ IsVisibleDefault);
    }
    internal override void UnTriggered()
    {
        Target.SetActive(IsVisibleDefault);
    }
}
