using System;
using UnityEngine;

namespace Utils
{
    public static class ConvertUtil
    {

        #region RegEx Constants

        private const string RX_ISHEX = @"(?<sign>[-+]?)(?<flag>0x|#|&H)(?<num>[\dA-F]+)(?<fractional>(\.[\dA-F]+)?)$";

        #endregion

        #region Color

        public static int ToInt(Color color)
        {
            return (Mathf.RoundToInt(color.a * 255) << 24) +
                   (Mathf.RoundToInt(color.r * 255) << 16) +
                   (Mathf.RoundToInt(color.g * 255) << 8) +
                   Mathf.RoundToInt(color.b * 255);
        }

        public static Color ToColor(int value)
        {
            var a = (float)(value >> 24 & 0xFF) / 255f;
            var r = (float)(value >> 16 & 0xFF) / 255f;
            var g = (float)(value >> 8 & 0xFF) / 255f;
            var b = (float)(value & 0xFF) / 255f;
            return new Color(r, g, b, a);
        }
        
        public static Color ToColor(Color32 value)
        {
            return new Color((float)value.r / 255f,
                             (float)value.g / 255f,
                             (float)value.b / 255f,
                             (float)value.a / 255f);
        }

        public static Color ToColor(Vector3 value)
        {

            return new Color((float)value.x,
                             (float)value.y,
                             (float)value.z);
        }

        public static Color ToColor(Vector4 value)
        {
            return new Color((float)value.x,
                             (float)value.y,
                             (float)value.z,
                             (float)value.w);
        }
        
        public static int ToInt(Color32 color)
        {
            return (color.a << 24) +
                   (color.r << 16) +
                   (color.g << 8) +
                   color.b;
        }

        public static Color32 ToColor32(int value)
        {
            byte a = (byte)(value >> 24 & 0xFF);
            byte r = (byte)(value >> 16 & 0xFF);
            byte g = (byte)(value >> 8 & 0xFF);
            byte b = (byte)(value & 0xFF);
            return new Color32(r, g, b, a);
        }

        public static Color32 ToColor32(Color value)
        {
            return new Color32((byte)(value.r * 255f),
                               (byte)(value.g * 255f),
                               (byte)(value.b * 255f),
                               (byte)(value.a * 255f));
        }

        public static Color32 ToColor32(Vector3 value)
        {

            return new Color32((byte)(value.x * 255f),
                               (byte)(value.y * 255f),
                               (byte)(value.z * 255f), 255);
        }

        public static Color32 ToColor32(Vector4 value)
        {
            return new Color32((byte)(value.x * 255f),
                               (byte)(value.y * 255f),
                               (byte)(value.z * 255f),
                               (byte)(value.w * 255f));
        }

        #endregion
        
        #region String

        public static string ConvertFloat(float value)
        {
            string moneyString = Format(Math.Round(value, 2).ToString());

            if (value >= 1000)
                moneyString = Format($"{Math.Round(value / 1000f, 1)}K");
            if (value >= 1000000)
                moneyString = Format($"{Math.Round(value / 1000000f, 1)}M");

            return moneyString;
        }
        
        private static string Format(string str)
        {
            return str.Replace(',', '.');
        }

        #endregion
    }
}

