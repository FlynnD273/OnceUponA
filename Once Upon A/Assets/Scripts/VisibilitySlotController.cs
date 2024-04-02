using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilitySlotController : WordSlotController
{
    public GameObject Target;
    public string triggerWord;

    internal override void Triggered(Word oldWord)
    {
        if (string.IsNullOrWhiteSpace(triggerWord))
        {
					if (CurrentWord != null) {
                Target.SetActive(true);
					}
        }
        else
        {
            if (CurrentWord.Text == triggerWord)
            {
                Target.SetActive(false);
            }
        }
    }
    internal override void UnTriggered()
    {
        Target.SetActive(!string.IsNullOrWhiteSpace(triggerWord));
    }
}
