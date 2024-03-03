using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem
{
    public class HorizontalLineAttribute : PropertyAttribute
    {
        public int m_Thickness = 2;
        public float m_Padding = 10f;
        public Color? m_LineColor = null;
        public HorizontalLineAttribute() { }
        public HorizontalLineAttribute(float r = 0, float g = 0, float b = 0, float a = 1) { m_LineColor = new Color(r, g, b, a); }
        public HorizontalLineAttribute(string textColorHex) 
        {
            if (ColorUtility.TryParseHtmlString(textColorHex, out Color newColor))
            {
                m_LineColor = newColor;
            }
        }
    }
}