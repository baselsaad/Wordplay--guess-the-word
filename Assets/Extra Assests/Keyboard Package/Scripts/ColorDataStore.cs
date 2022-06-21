using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ColorDataStore : MonoBehaviour
{
    [SerializeField] Color borderColor;
    [SerializeField] Color fillColor;
    [SerializeField] Color textColor;
    [SerializeField] Color actionTextColor;

    static Color s_borderColor;
    static Color s_fillColor;
    static Color s_textColor;
    static Color s_actionTextColor;

    private void Awake()
    {
        s_borderColor = borderColor;
        s_fillColor = fillColor;
        s_textColor = textColor;
        s_actionTextColor = actionTextColor;
    }

    public static Color GetKeyboardBorderColor() { return s_borderColor; }
    public static Color GetKeyboardFillColor() { return s_fillColor; }
    public static Color GetKeyboardTextColor() { return s_textColor; }
    public static Color GetKeyboardActionTextColor() { return s_actionTextColor; }
}
