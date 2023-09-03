using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.DialogueSystem
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Object/Dialogue System/Dialogue")]
    public class DialogueScriptableObject : ScriptableObject
    {
        [SerializeField] List<DialogueLineScriptableObject> m_lines = new List<DialogueLineScriptableObject>();
        public List<DialogueLineScriptableObject> m_Lines => m_lines;
    }
}
