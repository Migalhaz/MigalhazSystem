using System;
using UnityEngine;
using Trigger.Core;
//using log4net.Util;

namespace Trigger.System2D
{
    public abstract class System2D : BasicTriggerSystem
    {
        [SerializeField] Vector2 m_triggerOffset = Vector2.zero;
        public Vector2 m_TriggerOffset => m_triggerOffset;

        #region Methods
        public System2D() : base()
        {
            SetTriggerOffset(Vector2.zero);
        }

        public void SetTriggerOffset(Vector2 _newOffset)
        {
            m_triggerOffset = _newOffset;
        }

        public void FlipOffset()
        {
            SetTriggerOffset(-m_triggerOffset);
        }

        public void FlipXOffset()
        {
            SetTriggerOffset(new Vector2(-m_triggerOffset.x, m_triggerOffset.y));
        }

        public void FlipYOffset()
        {
            SetTriggerOffset(new Vector2(m_triggerOffset.x, -m_triggerOffset.y));
        }
        #endregion
    }

    [Serializable]
    public class BoxTrigger2D : System2D
    {
        #region Variables
        [SerializeField, Min(0)] Vector2 m_triggerSize = Vector2.one;
        [SerializeField] float m_triggerAngle = 0f;

        #region Getters
        public Vector2 m_TriggerSize => m_triggerSize;
        public float m_TriggerAngle => m_triggerAngle;
        #endregion

        #endregion

        #region Methods

        #region Constructor

        public BoxTrigger2D() : base()
        {
            m_triggerSize = Vector2.one;
        }

        #endregion

        #region Draw

        public void SetTriggerSize(Vector2 newSize)
        {
            m_triggerSize = newSize;
        }

        public void SetTriggerRotation(float newAngle)
        {
            m_triggerAngle = newAngle;
        }

        public override void DrawTrigger(Vector3 position)
        {
            if (!m_DrawSettings.Draw) return;
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            Matrix4x4 rotationMatrix = Matrix4x4.TRS(position, Quaternion.Euler(0f, 0f, m_TriggerAngle), Vector3.one);
            Gizmos.matrix = rotationMatrix;
            Gizmos.color = InTrigger(position, false) ? m_DrawSettings.InColor : m_DrawSettings.OutColor;
            if (m_DrawSettings.DrawSolid)
            {
                Gizmos.DrawCube(m_TriggerOffset.ToVector3(), m_triggerSize);
            }
            else
            {
                Gizmos.DrawWireCube(m_TriggerOffset.ToVector3(), m_triggerSize);
            }
        }

        public override void DrawTrigger(Transform transform)
        {
            DrawTrigger(transform.position);
        }

        public override void DrawTrigger(GameObject gameObject)
        {
            DrawTrigger(gameObject.transform.position);
        }

        public override void DrawTrigger(Collider2D collider2D)
        {
            DrawTrigger(collider2D.transform.position);
        }
        #endregion

        #region InTrigger
        public override bool InTrigger(Vector3 position, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            bool isIn = Physics2D.OverlapBox(position + m_TriggerOffset.ToVector3(), m_TriggerSize, m_triggerAngle, m_TriggerLayerMask);
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            
            return isIn;
        }

        public override bool InTrigger(Transform transform, bool callbacks = true)
        {
            return InTrigger(transform.position, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, callbacks);
        }

        public override bool InTrigger(Collider2D collider2D, bool callbacks = true)
        {
            return InTrigger(collider2D.transform.position, callbacks);
        }

        public override bool InTrigger<T>(T _component, bool callbacks = true)
        {
            return InTrigger(_component.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Vector3 _position, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_position);
            return InTrigger(_position, callbacks);
        }

        public override bool InTrigger<T>(Transform _transform, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_transform.position);
            return InTrigger(_transform.position, callbacks);
        }

        public override bool InTrigger<T>(GameObject _gameObject, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_gameObject.transform.position);
            return InTrigger(_gameObject.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Collider2D _collider2D, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_collider2D.transform.position);
            return InTrigger(_collider2D.transform.position, callbacks);
        }

        public override T InTrigger<T>(Vector3 _position, bool callbacks = true, bool _debugErrors = true)
        {
            if (m_CenterObject != null)
            {
                _position = m_CenterObject.position;
            }
            if (!InTrigger(_position, callbacks)) return null;

            GameObject g = Physics2D.OverlapBox(_position + m_TriggerOffset.ToVector3(), m_TriggerSize, m_triggerAngle, m_TriggerLayerMask).gameObject;
            if (g.TryGetComponent(out T _component))
            {
                return _component;
            }
            if (_debugErrors)
            {
                Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{g.name}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
            }
            return null;
        }

        public override T InTrigger<T>(Transform _transform, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(GameObject _gameObject, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_gameObject.transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(Collider2D _collider2D, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_collider2D.transform.position, callbacks, _debugErrors);
        }
        #endregion

        #region GetComponent
        public override T[] GetComponent<T>(Vector3 _position)
        {
            if (m_CenterObject != null)
            {
                _position = m_CenterObject.position;
            }
            Collider2D[] colliders = Physics2D.OverlapBoxAll(_position + m_TriggerOffset.ToVector3(), m_TriggerSize, m_triggerAngle, m_TriggerLayerMask);
            int lenght = colliders.Length;
            T[] _ts = new T[lenght];
            for (int i = 0; i < lenght; i++)
            {
                if (colliders[i].TryGetComponent(out T _component))
                {
                    _ts[i] = _component;
                }
                else
                {
                    Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{colliders[i].gameObject}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
                }
            }
            return _ts;
        }

        public override T[] GetComponent<T>(Transform _transform) { return GetComponent<T>(_transform.position); }
        public override T[] GetComponent<T>(GameObject _gameObject) { return GetComponent<T>(_gameObject.transform.position); }
        public override T[] GetComponent<T>(Collider2D _collider2D) { return GetComponent<T>(_collider2D.transform.position); }
        public override T[] GetComponent<T>(T _component) { return GetComponent<T>(_component.transform.position); }
        #endregion
        #endregion
    }


    [Serializable]
    public class CircleTrigger2D : System2D
    {
        #region Variables
        [SerializeField] float m_triggerRadius = 0.5f;

        #region Getters
        public float m_TriggerRadius => m_triggerRadius;
        #endregion

        #endregion

        #region Methods

        public void SetTriggerRadius(float _newRadius)
        {
            m_triggerRadius = _newRadius;
        }

        #region InTrigger
        public override bool InTrigger(Vector3 _position, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                _position = m_CenterObject.position;
            }
            bool isIn = Physics2D.OverlapCircle(_position + m_TriggerOffset.ToVector3(), m_TriggerRadius, m_TriggerLayerMask);
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            return isIn;
        }

        public override bool InTrigger(Transform _transform, bool callbacks = true)
        {
            return InTrigger(_transform.position, callbacks);
        }

        public override bool InTrigger(GameObject _gameObject, bool callbacks = true)
        {
            return InTrigger(_gameObject.transform.position, callbacks);
        }

        public override bool InTrigger(Collider2D _collider2D, bool callbacks = true)
        {
            return InTrigger(_collider2D.transform.position, callbacks);
        }

        public override bool InTrigger<T>(T _component, bool callbacks = true)
        {
            return InTrigger(_component.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Vector3 _position, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_position);
            return InTrigger(_position, callbacks);
        }

        public override bool InTrigger<T>(Transform _transform, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_transform.position);
            return InTrigger(_transform.position, callbacks);
        }

        public override bool InTrigger<T>(GameObject _gameObject, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_gameObject.transform.position);
            return InTrigger(_gameObject.transform.position, callbacks);
        }

        public override bool InTrigger<T>(Collider2D _collider2D, out T[] _ts, bool callbacks = true)
        {
            _ts = GetComponent<T>(_collider2D.transform.position);
            return InTrigger(_collider2D.transform.position, callbacks);
        }

        public override T InTrigger<T>(Vector3 _position, bool callbacks = true, bool _debugErrors = true)
        {
            if (m_CenterObject != null)
            {
                _position = m_CenterObject.position;
            }
            if (!InTrigger(_position, callbacks)) return null;

            GameObject g = Physics2D.OverlapCircle(_position + m_TriggerOffset.ToVector3(), m_TriggerRadius, m_TriggerLayerMask).gameObject;
            if (g.TryGetComponent(out T _component))
            {
                return _component;
            }
            if (_debugErrors)
            {
                Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{g.name}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
            }
            return null;
        }

        public override T InTrigger<T>(Transform _transform, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(GameObject _gameObject, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_gameObject.transform.position, callbacks, _debugErrors);
        }

        public override T InTrigger<T>(Collider2D _collider2D, bool callbacks = true, bool _debugErrors = true)
        {
            return InTrigger<T>(_collider2D.transform.position, callbacks, _debugErrors);
        }
        #endregion

        #region GetComponent

        public override T[] GetComponent<T>(Vector3 _position)
        {
            if (m_CenterObject != null)
            {
                _position = m_CenterObject.position;
            }
            Collider2D[] colliders = Physics2D.OverlapCircleAll(_position + m_TriggerOffset.ToVector3(), m_TriggerRadius, m_TriggerLayerMask);
            int lenght = colliders.Length;
            T[] _ts = new T[lenght];
            for (int i = 0; i < lenght; i++)
            {
                if (colliders[i].TryGetComponent(out T _component))
                {
                    _ts[i] = _component;
                }
                else
                {
                    Debug.LogWarning($"<color=yellow>Warning:</color> The Game Object: <<color=#DC143C>{colliders[i].gameObject}</color>> doesn't have: <<color=#DC143C>{typeof(T).Name}</color>> as component");
                }
            }
            return _ts;
        }

        public override T[] GetComponent<T>(Transform _transform)
        {
            return GetComponent<T>(_transform.position);
        }

        public override T[] GetComponent<T>(GameObject _gameObject)
        {
            return GetComponent<T>(_gameObject.transform.position);
        }

        public override T[] GetComponent<T>(Collider2D _collider2D)
        {
            return GetComponent<T>(_collider2D.transform.position);
        }

        public override T[] GetComponent<T>(T _component)
        {
            return GetComponent<T>(_component.transform.position);
        }
        #endregion

        #region Draw
        public override void DrawTrigger(Vector3 _position)
        {
            if (!m_DrawSettings.Draw) return;
            if (m_CenterObject != null)
            {
                _position = m_CenterObject.position;
            }
            Gizmos.color = InTrigger(_position, false) ? m_DrawSettings.InColor : m_DrawSettings.OutColor;

            if (m_DrawSettings.DrawSolid)
            {
                Gizmos.DrawSphere(_position + m_TriggerOffset.ToVector3(), m_TriggerRadius);
            }
            else
            {
                Gizmos.DrawWireSphere(_position + m_TriggerOffset.ToVector3(), m_TriggerRadius);
            }
            
        }

        public override void DrawTrigger(Transform _transform)
        {
            DrawTrigger(_transform.position);
        }

        public override void DrawTrigger(GameObject _gameObject)
        {
            DrawTrigger(_gameObject.transform.position);
        }

        public override void DrawTrigger(Collider2D _collider2D)
        {
            DrawTrigger(_collider2D.transform.position);
        }
        #endregion

        #endregion
    }
}