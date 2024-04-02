using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilitySlotController : WordSlotController
{
    public string triggerWord;
    public GameObject[] Visible;
    public GameObject[] Invisible;

    public override void Init()
    {
        foreach (var go in Visible)
        {
            go.SetActive(true);
        }
        foreach (var go in Invisible)
        {
            go.SetActive(false);
        }
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

        if (cond)
        {
            foreach (var go in Visible)
            {
                go.SetActive(false);
            }
            foreach (var go in Invisible)
            {
                go.SetActive(true);
            }
        }
    }

    internal override void UnTriggered()
    {
        foreach (var go in Visible)
        {
            go.SetActive(true);
        }
        foreach (var go in Invisible)
        {
            go.SetActive(false);
        }
    }
}
