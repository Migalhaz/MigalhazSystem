using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace MigalhaSystem.ScriptableEvents
{
    public abstract class AbstractScriptableEvent<T> : ScriptableObject
    {
        Action<T> action = delegate { };
        public void Invoke(T data) => action?.Invoke(data);

        public void ResetEvents() => action = delegate { };

        public static AbstractScriptableEvent<T> operator +(AbstractScriptableEvent<T> _event, Action<T> _action)
        {
            _event.action += _action;
            return _event;
        }

        public static AbstractScriptableEvent<T> operator -(AbstractScriptableEvent<T> _event, Action<T> _action)
        {
            _event.action -= _action;
            return _event;
        }

        public static AbstractScriptableEvent<T> operator --(AbstractScriptableEvent<T> _event)
        {
            _event.ResetEvents();
            return _event;
        }

        public void AddListener(Action<T> _action) { action += _action; }

        public void RemoveListener(Action<T> _action) { action -= _action; }
        public void Clear() { ResetEvents(); }
    }
    public abstract class AbstractScriptableEvent<T1, T2> : ScriptableObject
    {
        Action<T1, T2> action = delegate { };
        public void Invoke(T1 data1, T2 data2) => action?.Invoke(data1, data2);

        public void ResetEvents() => action = delegate { };

        public static AbstractScriptableEvent<T1, T2> operator +(AbstractScriptableEvent<T1, T2> _event, Action<T1, T2> _action)
        {
            _event.action += _action;
            return _event;
        }

        public static AbstractScriptableEvent<T1, T2> operator -(AbstractScriptableEvent<T1, T2> _event, Action<T1, T2> _action)
        {
            _event.action -= _action;
            return _event;
        }

        public static AbstractScriptableEvent<T1, T2> operator --(AbstractScriptableEvent<T1, T2> _event)
        {
            _event.ResetEvents();
            return _event;
        }

        public void AddListener(Action<T1, T2> _action) { action += _action; }

        public void RemoveListener(Action<T1, T2> _action) { action -= _action; }
        public void Clear() { ResetEvents(); }
    }
    public abstract class AbstractScriptableEvent<T1, T2, T3> : ScriptableObject
    {
        Action<T1, T2, T3> action = delegate { };
        public void Invoke(T1 data1, T2 data2, T3 data3) => action?.Invoke(data1, data2, data3);

        public void ResetEvents() => action = delegate { };

        public static AbstractScriptableEvent<T1, T2, T3> operator +(AbstractScriptableEvent<T1, T2, T3> _event, Action<T1, T2, T3> _action)
        {
            _event.action += _action;
            return _event;
        }

        public static AbstractScriptableEvent<T1, T2, T3> operator -(AbstractScriptableEvent<T1, T2, T3> _event, Action<T1, T2, T3> _action)
        {
            _event.action -= _action;
            return _event;
        }

        public static AbstractScriptableEvent<T1, T2, T3> operator --(AbstractScriptableEvent<T1, T2, T3> _event)
        {
            _event.ResetEvents();
            return _event;
        }

        public void AddListener(Action<T1, T2, T3> _action) { action += _action; }

        public void RemoveListener(Action<T1, T2, T3> _action) { action -= _action; }
        public void Clear() { ResetEvents(); }
    }
}