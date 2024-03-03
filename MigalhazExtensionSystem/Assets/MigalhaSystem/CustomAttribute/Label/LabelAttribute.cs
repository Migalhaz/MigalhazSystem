using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem
{
    public class LabelAttribute : PropertyAttribute
    {
        public string m_Text = string.Empty;
        public string m_ToolTip = string.Empty;
        public int m_LabelSize = 22;
        public Color? m_LabelColor = null;

        public LabelAttribute(string text) { m_Text = text; }
        public LabelAttribute(string text, float r = 0, float g = 0, float b = 0, float a = 1) 
        {
            m_Text = text;
            m_LabelColor = new Color(r, g, b, a); 
        }

        public LabelAttribute(string text, string textColorHex)
        {
            m_Text = text;
            if (ColorUtility.TryParseHtmlString(textColorHex, out Color newColor))
            {
                m_LabelColor = newColor;
            }
        }
    }
}
