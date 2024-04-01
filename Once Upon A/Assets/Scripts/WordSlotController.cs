using System.Collections;
using System.Collections.Generic;
using static Constants.Constants;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using System.Linq;

public class WordSlotController : MonoBehaviour
{
    public string CurrentWord
    {
        get
        {
            if (string.IsNullOrEmpty(text.text)) return null;
            return text.text;
        }
        set
        {
            text.text = value;
        }
    }

    private Collider2D coll;
    private Collider2D trigger;
    private TextMesh text;

    private float fullPos;
    private float emptyPos;
    private float fullTriggerPos;
    private float emptyTriggerPos;
    private Stopwatch transition;
    private int transTime = 100;

    // Start is called before the first frame update
    void Start()
    {
        transition = new();
        text = GetComponent<TextMesh>();
        var colliders = GetComponents<Collider2D>().ToList();
        coll = colliders.First(x => !x.isTrigger);
        trigger = colliders.First(x => x.isTrigger);

        fullPos = coll.offset.y;
        emptyPos = coll.offset.y - coll.bounds.size.y / transform.localScale.x;

        fullTriggerPos = trigger.offset.y;
        emptyTriggerPos = trigger.offset.y - coll.bounds.size.y / transform.localScale.x;

        if (CurrentWord == null)
        {
            coll.offset = new Vector2(coll.offset.x, emptyPos);
            trigger.offset = new Vector2(trigger.offset.x, emptyTriggerPos);
            coll.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (transition.IsRunning)
        {
            if (transition.ElapsedMilliseconds > transTime)
            {
                if (CurrentWord == null)
                {
                    coll.offset = new Vector2(coll.offset.x, emptyPos);
                    trigger.offset = new Vector2(trigger.offset.x, emptyTriggerPos);
                    coll.enabled = false;
                }
                else
                {
                    coll.offset = new Vector2(coll.offset.x, fullPos);
                    trigger.offset = new Vector2(trigger.offset.x, fullTriggerPos);
                }
                transition.Stop();
            }
            else
            {
                if (CurrentWord == null)
                {
                    coll.offset = new Vector2(coll.offset.x, Mathf.Lerp(fullPos, emptyPos, transition.ElapsedMilliseconds / (float)transTime));
                    trigger.offset = new Vector2(trigger.offset.x, Mathf.Lerp(fullTriggerPos, emptyTriggerPos, transition.ElapsedMilliseconds / (float)transTime));
                }
                else
                {
                    coll.offset = new Vector2(coll.offset.x, Mathf.Lerp(emptyPos, fullPos, transition.ElapsedMilliseconds / (float)transTime));
                    trigger.offset = new Vector2(trigger.offset.x, Mathf.Lerp(emptyTriggerPos, fullTriggerPos, transition.ElapsedMilliseconds / (float)transTime));
                }
            }

        }

    }

    public string Swap(string newWord)
    {
        transition.Restart();
        if (newWord == null)
        {
            var temp = CurrentWord;
            CurrentWord = null;
            UnTriggered();
            return temp;
        }

        coll.enabled = true;
        CurrentWord = newWord;
        Triggered();

        return null;
    }

    internal virtual void Triggered()
    {
        Debug.Log("Triggered! " + CurrentWord);
    }

    internal virtual void UnTriggered()
    {
        Debug.Log("Emptied!");
    }

}
