using MigalhaSystem.ScriptableEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesteListener : MonoBehaviour
{
    [SerializeField] ScriptableEvent evento;
    void Awake()
    {
        evento += (object data, Object invoker) => { Debug.Log($"{data}, {invoker}"); };
    }
}
