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
        Action<object, UnityEngine.Object> action = delegate { };

        public void Invoke(object data, UnityEngine.Object invoker = null) => action?.Invoke(data, invoker);

        public void ResetEvents() => action = delegate { };

        public static ScriptableEvent operator +(ScriptableEvent _event, Action<object, UnityEngine.Object> _action)
        {
            _event.action += _action;
            return _event;
        }

        public static ScriptableEvent operator -(ScriptableEvent _event, Action<object, UnityEngine.Object> _action)
        {
            _event.action -= _action;
            return _event;
        }

        public static ScriptableEvent operator --(ScriptableEvent _event)
        {
            _event.ResetEvents();
            return _event;
        }

        public void AddListener(Action<object, UnityEngine.Object> _action){ action += _action; }

        public void RemoveListener(Action<object, UnityEngine.Object> _action) {  action -= _action; }
        public void Clear() { ResetEvents(); }
    }
}