using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem
{
    [CustomPropertyDrawer(typeof(HorizontalLineAttribute))]
    public class HorizontalLineDrawer : DecoratorDrawer
    {
        public override float GetHeight()
        {
            HorizontalLineAttribute horizontalLineAttribute = attribute as HorizontalLineAttribute;
            return Mathf.Max(horizontalLineAttribute.m_Padding, horizontalLineAttribute.m_Thickness);
        }

        public override void OnGUI(Rect position)
        {
            HorizontalLineAttribute horizontalLineAttribute = attribute as HorizontalLineAttribute;
            position.height = horizontalLineAttribute.m_Thickness;
            position.y += horizontalLineAttribute.m_Padding * .5f;

            Color GetCurrentColor()
            {
                if (horizontalLineAttribute.m_LineColor != null) return (Color) horizontalLineAttribute.m_LineColor;
                Color defaultDarkColor = new(.3f, .3f, .3f, 1f);
                Color defaultLightColor = new(.7f, .7f, .7f, 1f);
                return EditorGUIUtility.isProSkin ? defaultDarkColor : defaultLightColor;
            }

            EditorGUI.DrawRect(position, GetCurrentColor());
        }
    }
}