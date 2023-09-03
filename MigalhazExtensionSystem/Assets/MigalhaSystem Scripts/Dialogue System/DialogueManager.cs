using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] Languagekey m_languageKey;
        [SerializeField] UnityEngine.UI.RawImage m_characterIcon;
        [SerializeField] TMPro.TextMeshProUGUI m_dialogueTextBox;
        [SerializeField] TMPro.TextMeshProUGUI m_characterNameTextBox;
        [SerializeField, Range(0, 1f)] float m_typeSpeed;
        int m_currentIndex;
        DialogueScriptableObject m_currentDialogue;
        bool m_typing;

        private void Awake()
        {
            m_currentIndex = 0;
            HideUI();
        }

        float m_currentTypeSpeed => Mathf.Abs(1f - m_typeSpeed) * 0.1f;

        void SetCurrentDialogue(DialogueScriptableObject dialogue)
        {
            m_currentDialogue = dialogue;
        }

        DialogueLine GetCurrentLine(bool debugErrors = true)
        {
            if (m_currentDialogue is null) return null;
            return m_currentDialogue.m_Lines[m_currentIndex].GetDialogueLine(m_languageKey, debugErrors);
        }

        Texture GetCurrentCharacterIcon()
        {
            return m_currentDialogue.m_Lines[m_currentIndex].m_DialogueIcon.texture;
        }

        IEnumerator Type()
        {
            DialogueLine currentDialogueLine = GetCurrentLine();
            string currentText = currentDialogueLine.m_TextLine;
            m_characterNameTextBox.text = currentDialogueLine.m_CharacterName;
            m_dialogueTextBox.text = "";
            m_characterIcon.texture = GetCurrentCharacterIcon();
            m_typing = true;
            foreach (char c in currentText)
            {
                yield return new WaitForSeconds(m_currentTypeSpeed);
                m_dialogueTextBox.text += c;
            }
            m_typing = false;
        }

        public void StartDialogue(DialogueScriptableObject dialogue)
        {
            m_currentIndex = 0;
            SetCurrentDialogue(dialogue);
            TypeExec();
        }

        public void NextLine()
        {
            if (m_currentDialogue is null) return;
            if (m_typing)
            {
                BreakType();
                return;
            }
            IncreaseIndex();

            if (!HaveLines())
            {
                FinishDialogue();
                return;
            }
            TypeExec();
        }

        void BreakType()
        {
            StopAllCoroutines();
            m_dialogueTextBox.text = GetCurrentLine().m_TextLine;
            m_typing = false;
        }

        void IncreaseIndex()
        {
            do { m_currentIndex++; } 
            while (WhileLogic());

            bool WhileLogic()
            {
                if (!HaveLines()) return false;
                if (GetCurrentLine() == null) return true;
                return false;
            }
        }

        bool HaveLines() => m_currentIndex < m_currentDialogue.m_Lines.Count;

        public void FinishDialogue()
        {
            StopAllCoroutines();
            m_currentDialogue = null;
            m_currentIndex = 0;
            HideUI();
        }

        void TypeExec()
        {
            StopAllCoroutines();
            ShowUI();
            StartCoroutine(Type());
        }

        void HideUI()
        {
            m_dialogueTextBox.gameObject.SetActive(false);
            m_characterNameTextBox.gameObject.SetActive(false);
            m_characterIcon.gameObject.SetActive(false);
        }

        void ShowUI()
        {
            m_dialogueTextBox.gameObject.SetActive(true);
            m_characterNameTextBox.gameObject.SetActive(true);
            m_characterIcon.gameObject.SetActive(true);
        }
    }
}
