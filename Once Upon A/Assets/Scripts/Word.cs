using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Utils.Constants;

public class Word
{

		public WordType Type;
		public string Text;

    public Word(WordType type, string text)
    {
        Type = type;
        Text = text;
    }
}
