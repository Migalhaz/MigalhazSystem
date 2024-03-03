using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem
{
    public class NoteAttribute : PropertyAttribute
    {
        public string m_Text = string.Empty;
        public MessageType m_MessageType = MessageType.None;
        public NoteAttribute(string text)
        {
            m_Text = text;
        }
    }

    public enum MessageType
    {
        None, Info, Warning, Error
    }
}