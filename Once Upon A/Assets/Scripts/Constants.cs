using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TMPro;
using UnityEngine;

namespace Utils
{
  public static class Constants
  {
    public static readonly float StandardAnim = 30;

    public enum Layers
    {
      Default = 0,
      TransparentFX = 1,
      IgnoreRaycast = 2,
      Checkpoint = 3,
      Water = 4,
      UI = 5,
      Player = 6,
    }
    public enum WordType
    {
      Normal,
      Bouncy,
      Danger,
      White,
      Corrupt,
    }

    public static readonly ReadOnlyDictionary<WordType, Color> WordToColor = new(new Dictionary<WordType, Color>()
    {
      [WordType.Normal] = Utils.FromHex("558dfc"),
      [WordType.Bouncy] = Utils.FromHex("66fc55"),
      [WordType.Danger] = Utils.FromHex("fc5577"),
      [WordType.Corrupt] = Utils.FromHex("fc53f1"),
      [WordType.White] = Color.white,
    });


    public static readonly FontWidth CharWidths = new();
    public class FontWidth
    {
      public Dictionary<char, float> fontAdvances = new();
      private TMP_FontAsset font;

      public float this[char c]
      {
        get
        {
          if (fontAdvances.TryGetValue(c, out float width))
          {
            return width;
          }
          if (font == null)
          {
            font = TMP_Settings.defaultFontAsset;
          }
          width = font.characterLookupTable[c].glyph.metrics.horizontalAdvance * 0.5f;
          fontAdvances.Add(c, width);
          return width;
        }
      }
    }
  }

  public static class Utils
  {
    public static Color FromHex(string hex)
    {
      if (hex.Length != 6) { throw new Exception($"String of length {hex.Length} is not valid! Must be of length 6."); }
      byte[] bytes = new byte[3];
      for (int i = 0; i < hex.Length - 1; i += 2)
      {
        string b = hex.Substring(i, 2);
        bytes[i / 2] = Convert.ToByte(b, 16);
      }
      return new Color(bytes[0] / 255f, bytes[1] / 255f, bytes[2] / 255f);
    }

    public static void Invoke(this MonoBehaviour mb, Action f, float delay)
    {
      _ = mb.StartCoroutine(InvokeRoutine(f, delay));
    }

    private static IEnumerator InvokeRoutine(Action f, float delay)
    {
      yield return new WaitForSeconds(delay);
      f();
    }
  }
}
