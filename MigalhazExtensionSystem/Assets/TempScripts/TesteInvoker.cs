using MigalhaSystem.ScriptableEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteInvoker : MonoBehaviour
{
    [SerializeField] ScriptableEvent evento;
    [SerializeField] Teste t1;
    [SerializeField] Teste t2;

    void Start()
    {
        evento.Invoke("Testando o debug 1", this);
        evento.Invoke("Testando o debug 2");
    }
}

[System.Serializable]
public class Teste
{
    public int a;
}