using UnityEngine;
using UnityEditor;
namespace MigalhaSystem
{
	[CustomPropertyDrawer(typeof(LabelAttribute), true)]
	public class LabelAttributePropertyDrawer : DecoratorDrawer
    {
        public override float GetHeight() => EditorGUIUtility.singleLineHeight * 2;
        public override void OnGUI(Rect position)
        {
            LabelAttribute labelAttribute = attribute as LabelAttribute;
            position.yMin += EditorGUIUtility.singleLineHeight * .5f;

            //int charCount = labelAttribute.m_Text.Length;
            //float size = labelAttribute.m_LabelSize;
            //position.x = position.xMax * 0.5f - (charCount * size * .5f);

            GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
            style.richText = true;

            string txt = labelAttribute.m_Text;

            Color GetCurrentColor()
            {
                if (labelAttribute.m_LabelColor != null) return (Color)labelAttribute.m_LabelColor;
                Color defaultDarkColor = new(.3f, .3f, .3f, 1f);
                Color defaultLightColor = new(.7f, .7f, .7f, 1f);
                return EditorGUIUtility.isProSkin ? defaultLightColor : defaultDarkColor;
            }
            txt = Extensions.StringExtend.Size(txt, labelAttribute.m_LabelSize);
            txt = Extensions.StringExtend.Color(txt, GetCurrentColor());

            

            GUIContent label = new GUIContent(txt);
            if (!string.IsNullOrEmpty(labelAttribute.m_ToolTip)) label.tooltip = labelAttribute.m_ToolTip;
            GUI.Label(position, label,style);
        }
    }
}