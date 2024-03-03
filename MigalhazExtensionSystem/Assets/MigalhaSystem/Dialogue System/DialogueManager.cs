using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MigalhaSystem.DialogueSystem
{
    public class DialogueManager : MonoBehaviour
    {
        [Header("Dialogue Settings")]
        [SerializeField] Languagekey m_languageKey;
        [SerializeField, Range(0, 1f)] float m_typeSpeed;
        float m_currentTypeSpeed => Mathf.Abs(1f - m_typeSpeed) * 0.1f;

        [Header("Dialogue UI")]
        [SerializeField] UnityEngine.UI.RawImage m_characterIcon;
        [SerializeField] TextMeshProUGUI m_dialogueTextBox;
        [SerializeField] TextMeshProUGUI m_characterNameTextBox;
        [SerializeField] List<UnityEngine.UI.Button> m_buttons;

        int m_currentIndex;
        DialogueScriptableObject m_currentDialogue;
        bool m_typing;

        bool HaveLines() => m_currentIndex < m_currentDialogue.m_Lines.Count;

        private void Awake()
        {
            m_currentIndex = 0;
            HideUI();
        }

        public void SetLanguage(Languagekey languageKey)
        {
            m_languageKey = languageKey;
        }
        
        void SetCurrentDialogue(DialogueScriptableObject dialogue)
        {
            m_currentDialogue = dialogue;
        }

        DialogueLine GetCurrentLine(bool debugErrors = true)
        {
            if (m_currentDialogue is null) return null;
            return GetCurrentDialogueLineScriptableObject().GetDialogueLine(m_languageKey, debugErrors);
        }
        DialogueLineScriptableObject GetCurrentDialogueLineScriptableObject()
        {
            return m_currentDialogue.m_Lines[m_currentIndex];
        }

        Texture GetCurrentCharacterIcon()
        {
            return GetCurrentDialogueLineScriptableObject().m_DialogueIcon.texture;
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
            HideUI();
            m_currentIndex = 0;
            SetCurrentDialogue(dialogue);
            m_currentDialogue.StartDialogueEvent();
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
            GetCurrentDialogueLineScriptableObject().InvokeLineEvent();
            IncreaseIndex();
            
            if (!HaveLines())
            {
                if (m_currentDialogue.m_Choice is not null)
                {
                    SetChoices();
                    return;
                }
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
            if (!HaveLines())
            {
                return;
            }
            do { m_currentIndex++; } 
            while (WhileLogic());

            bool WhileLogic()
            {
                if (!HaveLines()) return false;
                if (GetCurrentLine() == null) return true;
                return false;
            }
        }

        public void FinishDialogue()
        {
            m_currentDialogue.FinishDialogueEvent();
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

        void SetChoices()
        {
            for (int i = 0; i < m_buttons.Count; i++)
            {
                if (i >= m_currentDialogue.m_Choice.m_Choice.Count)
                {
                    break;
                }
                m_buttons[i].gameObject.SetActive(true);
                Choice currentChoice = m_currentDialogue.m_Choice.m_Choice[i];
                m_buttons[i].GetComponentInChildren<TextMeshProUGUI>().text = currentChoice.GetChoiceText(m_languageKey).m_ChoiceText;
                SetupButton(currentChoice, m_buttons[i]);
            }
        }

        void SetupButton(Choice currentChoice, UnityEngine.UI.Button button)
        {
            button.onClick.AddListener(FinishDialogue);

            UnityEngine.Events.UnityAction action = 
                () => currentChoice.m_CurrentChoiceEvent.Invoke();

            button.onClick.AddListener(action);

            button.onClick.AddListener(ClearButton);

            void ClearButton()
            {
                button.onClick.RemoveListener(FinishDialogue);
                button.onClick.RemoveListener(action);
            }
        }

        void HideUI()
        {
            m_dialogueTextBox.gameObject.SetActive(false);
            m_characterNameTextBox.gameObject.SetActive(false);
            m_characterIcon.gameObject.SetActive(false);

            m_buttons.ForEach(x => x.gameObject.SetActive(false));
        }

        void ShowUI()
        {
            m_dialogueTextBox.gameObject.SetActive(true);
            m_characterNameTextBox.gameObject.SetActive(true);
            m_characterIcon.gameObject.SetActive(true);
        }
    }
}
