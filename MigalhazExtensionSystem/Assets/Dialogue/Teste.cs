using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem.DialogueSystem;
using MigalhaSystem.ScriptableEvents;

public class Teste : MonoBehaviour
{
    [SerializeField] DialogueManager m_DialogueManager;
    [SerializeField] DialogueScriptableObject m_scriptableObject;
    [SerializeField] ScriptableEvent event1;
    [SerializeField] ScriptableEvent event2;
    [SerializeField] ScriptableEvent event3;
    [SerializeField] ScriptableEvent event4;
    void Start()
    {
        event1 += Teste1;
        event2 += Teste2;
        event3 += Teste3;
        event4 += Teste4;
    }

    public void Teste1()
    {
        m_DialogueManager.SetLanguage(Languagekey.PTBR);
        m_DialogueManager.StartDialogue(m_scriptableObject);
    }

    public void Teste2()
    {
        m_DialogueManager.SetLanguage(Languagekey.EN);
        m_DialogueManager.StartDialogue(m_scriptableObject);
    }

    public void Teste3()
    {
        m_DialogueManager.SetLanguage(Languagekey.ES);
        m_DialogueManager.StartDialogue(m_scriptableObject);
    }

    public void Teste4()
    {
        m_DialogueManager.SetLanguage(Languagekey.FR);
        m_DialogueManager.StartDialogue(m_scriptableObject);
    }
}
