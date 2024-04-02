using System.Collections;
using System.Collections.Generic;
using static Utils.Constants;
using UnityEngine;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using System;
using System.Linq;

public class WordSlotController : MonoBehaviour
{
    private Word currentWord;
    public Word CurrentWord
    {
        get
        {
            return currentWord;
        }
        set
        {
            currentWord = value;

            if (value == null)
            {
                text.text = new string('_', (int)(trigger.bounds.size.x / 0.05 / 25));
                text.color = WordToColor[WordType.Normal];
            }
            else
            {
                text.text = value.Text;
                text.color = WordToColor[value.Type];
            }
        }
    }

    public WordType StartingWordType;

    private Collider2D trigger;
    private TextMesh text;

    private int startLength;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMesh>();
        startLength = text.text.Length;
        trigger = GetComponent<Collider2D>();
        if (text.text.Trim('_').Length == 0)
        {
            CurrentWord = null;
        }
        else
        {
            CurrentWord = new(StartingWordType, text.text);
        }
    }

    public Word Swap(Word newWord)
    {
        var temp = CurrentWord;
        UnTriggered();
        CurrentWord = newWord;
        Triggered(temp);
        return temp;
    }

    internal virtual void Triggered(Word oldWord)
    {
        Debug.Log("Triggered! " + CurrentWord);
    }

    internal virtual void UnTriggered()
    {
        Debug.Log("Emptied!");
    }

}
