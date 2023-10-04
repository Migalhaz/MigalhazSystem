using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem.ScriptableEvents
{
    [CreateAssetMenu(fileName = "New Event", menuName = "Scriptable Object/Scriptable Event/New Event")]
    public class ScriptableEvent : ScriptableObject
    {
        Action action = delegate { };

        public void Invoke() => action?.Invoke();

        public void ResetEvents() => action = delegate { };

        public static ScriptableEvent operator +(ScriptableEvent _event, Action _action)
        {
            _event.action += _action;
            return _event;
        }

        public static ScriptableEvent operator -(ScriptableEvent _event, Action _action)
        {
            _event.action -= _action;
            return _event;
        }

        public static ScriptableEvent operator --(ScriptableEvent _event)
        {
            _event.ResetEvents();
            return _event;
        }

        public void AddListener(Action _action){ action += _action; }

        public void RemoveListener(Action _action) {  action -= _action; }
        public void Clear() { ResetEvents(); }
    }

    public abstract class Observer : MonoBehaviour
    {
        protected virtual void InvokeEvent(ScriptableEvent scriptableEvent)
        {
            scriptableEvent.Invoke();
        }
    }

    public abstract class Listener : MonoBehaviour
    {
        Dictionary<ScriptableEvent, List<Action>> m_scriptableEvents;
        protected void AddEvent(ScriptableEvent scriptableEvent, Action action)
        {
            scriptableEvent += action;
            if (m_scriptableEvents.ContainsKey(scriptableEvent))
            {
                m_scriptableEvents[scriptableEvent].Add(action);
            }
            else
            {

                m_scriptableEvents.Add(scriptableEvent, new List<Action>() { action });
            }
        }
        
        protected void ClearEvents()
        {
            foreach (var e in m_scriptableEvents)
            {
                e.Value.ForEach(x => e.Key.RemoveListener(x));
            }
        }

        protected void OnDisable()
        {
            ClearEvents();
        }
    }
}