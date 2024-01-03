using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem.ScriptableEvents;

namespace MigalhaSystem.DialogueSystem
{
    [CreateAssetMenu(fileName = "NewLine", menuName = "Scriptable Object/Dialogue System/Dialogue Line")]
    public class DialogueLineScriptableObject : ScriptableObject
    {
        [SerializeField] Sprite m_dialogueIcon;
        [SerializeField] List<DialogueLine> m_dialogueLine = new() { new() };
        [SerializeField] ScriptableEvent m_lineEvent;
        public Sprite m_DialogueIcon => m_dialogueIcon;

        public DialogueLine GetDialogueLine(Languagekey currentLanguage, bool debugError = true)
        {
            List<DialogueLine> lines = m_dialogueLine.FindAll(x => x.GetDialogueLanguage(currentLanguage));
            if (lines.Count > 1)
            {
                if (debugError)
                {
                    Debug.LogError($"More than 1 dialogue line with the {currentLanguage} language key found");
                }
                return null;
            }
            if (lines.Count < 1)
            {
                if (debugError)
                {
                    Debug.LogError($"No dialogue line with the {currentLanguage} language key found");
                }
                return null;
            }
            return lines[0];
        }

        public void InvokeLineEvent()
        {
            if (m_lineEvent is null) return;
            
            m_lineEvent.Invoke(this, null);
        }
    }

    [System.Serializable]
    public class DialogueLine
    {
        [Space, SerializeField] Languagekey m_languagekey = Languagekey.PTBR;
        [SerializeField] string m_characterName;
        [SerializeField, TextArea(5, 10)] string m_textLine;
        [SerializeField] List<GetVariablesForDialogue> m_getVariablesForDialogues = new();

        public string m_CharacterName => m_characterName;
        public string m_TextLine => CurrentText();

        public bool GetDialogueLanguage(Languagekey currentLanguage)
        {
            return m_languagekey == currentLanguage;
        }

        public string CurrentText()
        {
            string currentText = m_textLine;
            foreach (var i in m_getVariablesForDialogues)
            {
                string name, value;
                (name, value) = i.GetVariable();
                currentText = currentText.Replace($"%{name}%", value);
            }
            return currentText;
        }
    }

    public enum Languagekey
    {
        PTBR,
        EN,
        ES,
        FR
    }

    public abstract class GetVariablesForDialogue : ScriptableObject
    {
        public abstract (string, string) GetVariable();
    }
}
