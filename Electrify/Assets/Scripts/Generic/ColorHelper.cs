using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorHelper : MonoBehaviour
{
    public static float[] ConvertRgbToCmyk(Color color)
    {

        float k = ClampCmyk(1 - Mathf.Max(Mathf.Max(color.r, color.g), color.b));
        float c = ClampCmyk((1 - color.r - k) / (1 - k));
        float m = ClampCmyk((1 - color.g - k) / (1 - k));
        float y = ClampCmyk((1 - color.b - k) / (1 - k));

        return new float[]{c, m, y, k};
    }

    public static Color ConvertCmykToRgb(float[] color)
    {
        float r = (1 - color[0]) * (1 - color[3]);
        float g = (1 - color[1]) * (1 - color[3]);
        float b = (1 - color[2]) * (1 - color[3]);

        return new Color(r, g, b, 1.0f);
    }

    public static Color AdditiveBlending(Color color1, Color color2)
    {
        float[] cmyk1 = ConvertRgbToCmyk(color1);
        float[] cmyk2 = ConvertRgbToCmyk(color2);
        
        for(int i = 0; i < cmyk1.Length; i++)
        {
            cmyk1[i] = cmyk1[i] + cmyk2[i];
        }
        return ConvertCmykToRgb(cmyk1);
    }

    private static float ClampCmyk(float value)
    {
        if (value < 0 || float.IsNaN(value))
        {
            value = 0;
        }

        return value;
    }
}
