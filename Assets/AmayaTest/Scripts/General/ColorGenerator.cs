using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AmayaTest.Scripts.General
{
    public static class ColorGenerator
    {
        private const int MINIMAL_COLORS_SET = 3;
        
        public static Color GetRandomColor(float hue = -1, float saturation = -1, float value = -1)
        {
            Color result = Color.HSVToRGB(GetRandomIfNegative(hue), GetRandomIfNegative(saturation), GetRandomIfNegative(value));
            result.a = 1;
            return result;
        }
        
        /// <param name="setLenght">Minimum 3</param>
        public static List<Color> GetColorSet(float saturation = -1, float value = -1, int setLenght = 10)
        {
            setLenght = Math.Max(MINIMAL_COLORS_SET, setLenght);
            List<Color> result = new List<Color>(setLenght);

            float colorDelta = 1f / (setLenght - 1);
            float initialHue = Random.Range(0f, 1f);
            saturation = GetRandomIfNegative(saturation);
            value = GetRandomIfNegative(value);

            for (int i = 0; i < setLenght; i++)
            {
                float hue = initialHue + colorDelta * i;
                hue -= (int)hue;
                result.Add(GetRandomColor(hue, saturation, value));
            }
            
            return result;
        }

        private static float GetRandomIfNegative(float val)
        {
            return val < 0 ? Random.Range(0f, 1f) : val;
        }
    }
}