using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem
{
    [CustomPropertyDrawer(typeof(NoteAttribute))]
    public class NoteDrawer : DecoratorDrawer
    {
        float m_height = 0f;
        const float m_PADDING = 20f;
        public override float GetHeight()
        {
            NoteAttribute noteAttribute = attribute as NoteAttribute;
            GUIStyle style = EditorStyles.helpBox;
            style.alignment = TextAnchor.MiddleLeft;
            style.wordWrap = true;
            style.padding = new RectOffset(10, 10, 10, 10);
            style.fontSize = 12;
            m_height = style.CalcHeight(new GUIContent(noteAttribute.m_Text), Screen.width);
            return m_height + m_PADDING;
        }

        public override void OnGUI(Rect position)
        {
            NoteAttribute noteAttribute = attribute as NoteAttribute;
            position.height = m_height;
            position.y += m_PADDING * .5f;
            EditorGUI.HelpBox(position, noteAttribute.m_Text, (UnityEditor.MessageType) noteAttribute.m_MessageType);
        }
    }
}