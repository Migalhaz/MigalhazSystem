using UnityEngine;
using MigalhaSystem.DialogueSystem;
using MigalhaSystem.ScriptableEvents;

public class Teste : MonoBehaviour
{
    [SerializeField, AutoGetComponent(typeof(Teste))] Teste rig;

    private void Start()
    {

        Debug.Log(rig.gameObject.name);
    }

    void Pato()
    {

    }
}
