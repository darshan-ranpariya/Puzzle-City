using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemColorCollection", menuName = "ItemColorCollection")]
public class ItemColorCollection : ScriptableObject
{
    public List<ItemColorData> colors = new List<ItemColorData>();
    public Color GetBtnColor(bool isDark)
    {
        try
        {
            return colors.Find((s) => { return s.isDarkMode == isDark; }).btnColor;
        }
        catch
        {
            return Color.clear;
        }
    }

    public Color GetShadowColor(bool isDark)
    {
        try
        {
            return colors.Find((s) => { return s.isDarkMode == isDark; }).shadowColor;
        }
        catch
        {
            return Color.clear;
        }
    }
}


[Serializable]
public class ItemColorData
{
    public bool isDarkMode;
    public Color btnColor;
    public Color shadowColor;
}

