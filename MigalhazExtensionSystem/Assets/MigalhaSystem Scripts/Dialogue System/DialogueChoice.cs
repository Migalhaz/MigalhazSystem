using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem.ScriptableEvents;

namespace MigalhaSystem.DialogueSystem
{
    [CreateAssetMenu(fileName = "NewChoice", menuName = "Scriptable Object/Dialogue System/Dialogue Choice")]
    public class DialogueChoice : ScriptableObject
    {
        [SerializeField] List<Choice> m_choices;
        public List<Choice> m_Choice => m_choices;
    }

    [System.Serializable]
    public class Choice
    {
        [SerializeField] List<ChoiceText> m_choiceText;
        [SerializeField] ScriptableEvent m_choiceEvent;
        public ScriptableEvent m_CurrentChoiceEvent => m_choiceEvent;

        public ChoiceText GetChoiceText(Languagekey currentLanguage)
        {
            List<ChoiceText> choice = m_choiceText.FindAll(x => x.CurrentText(currentLanguage));
            if (choice.Count > 1)
            {
                Debug.LogError($"More than 1 dialogue line with the {currentLanguage} language key found");
                return null;
            }
            if (choice.Count < 1)
            {
                Debug.LogError($"No dialogue line with the {currentLanguage} language key found");
                return null;
            }
            return choice[0];
        }
    }

    [System.Serializable]
    public class ChoiceText 
    {
        [SerializeField] string m_choiceText;
        [SerializeField] Languagekey m_languageKey;
        public string m_ChoiceText => m_choiceText;

        public bool CurrentText(Languagekey currentLanguage)
        {
            return m_languageKey == currentLanguage;
        }
    }
}