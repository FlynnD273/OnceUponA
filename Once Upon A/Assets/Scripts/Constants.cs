using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Constants
    {
        public enum Layers
        {
            Default = 0,
            TransparentFX = 1,
            IgnoreRaycast = 2,
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
        }
        public static Dictionary<WordType, Color> WordToColor;


        static Constants()
        {
            WordToColor = new();
            WordToColor.Add(WordType.Normal, Utils.FromHex("558dfc"));
            WordToColor.Add(WordType.Bouncy, Utils.FromHex("66fc55"));
            WordToColor.Add(WordType.Danger, Utils.FromHex("fc5577"));
            WordToColor.Add(WordType.White, Color.white);
        }
    }

    public static class Utils
    {
        public static Color FromHex(string hex)
        {
            if (hex.Length != 6) { throw new System.Exception($"String of length {hex.Length} is not valid! Must be of length 6."); }
            byte[] bytes = new byte[3];
            for (int i = 0; i < hex.Length - 1; i += 2)
            {
                string b = hex.Substring(i, 2);
                bytes[i / 2] = Convert.ToByte(b, 16);
            }
            return new Color(bytes[0] / 255f, bytes[1] / 255f, bytes[2] / 255f);
        }
    }
}
