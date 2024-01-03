using MigalhaSystem.ScriptableEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MigalhaSystem.ScriptableEvents 
{
    [CreateAssetMenu(fileName = "New Event", menuName = "Scriptable Object/New Scriptable Event/New Event")]
    public class NewScriptableEvent : ScriptableObject
    {
        Action<IInvoker, object> action = delegate { };

        public void Invoke(IInvoker invoker, object data) => action?.Invoke(invoker, data);

        public void ResetEvents() => action = delegate { };

        public static NewScriptableEvent operator +(NewScriptableEvent _event, Action<IInvoker, object> _action)
        {
            _event.action += _action;
            return _event;
        }

        public static NewScriptableEvent operator -(NewScriptableEvent _event, Action<IInvoker, object> _action)
        {
            _event.action -= _action;
            return _event;
        }

        public static NewScriptableEvent operator --(NewScriptableEvent _event)
        {
            _event.ResetEvents();
            return _event;
        }

        public void AddListener(Action<IInvoker, object> _action) { action += _action; }

        public void RemoveListener(Action<IInvoker, object> _action) { action -= _action; }
        public void Clear() { ResetEvents(); }
    }

    public interface IInvoker
    {
        T Convert<T>() where T : class;
    }
}
