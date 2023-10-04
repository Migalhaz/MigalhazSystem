using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem.ScriptableEvents;
using JetBrains.Annotations;

namespace MigalhaSystem.DialogueSystem
{
    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Scriptable Object/Dialogue System/Dialogue")]
    public class DialogueScriptableObject : ScriptableObject
    {
        [SerializeField] List<DialogueLineScriptableObject> m_lines = new List<DialogueLineScriptableObject>();
        [SerializeField] DialogueChoice m_choice;
        [SerializeField] ScriptableEvent m_startDialogueEvent;
        [SerializeField] ScriptableEvent m_finishDialogueEvent;
        public List<DialogueLineScriptableObject> m_Lines => m_lines;
        public DialogueChoice m_Choice => m_choice;

        public void StartDialogueEvent()
        {
            if (m_startDialogueEvent is null) return;
            m_startDialogueEvent?.Invoke();
        }

        public void FinishDialogueEvent()
        {
            if (m_finishDialogueEvent is null) return;
            m_finishDialogueEvent?.Invoke();
        }
    }
}
