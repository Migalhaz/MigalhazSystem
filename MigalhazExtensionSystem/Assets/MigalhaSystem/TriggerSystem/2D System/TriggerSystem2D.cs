using UnityEngine;
using System.Linq;
using Trigger.Core;
using System.Collections.Generic;

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

        /// <summary>
        /// Checks if there's anything in trigger.
        /// </summary>
        /// <param name="collider2D">Trigger's center by collider2D. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns>Returns true if there's anything in trigger. Returns false otherwise.</returns>
        public abstract bool InTrigger(Collider2D collider2D, int resultCount = 1, bool callbacks = true);

        /// <summary>
        /// Checks if there's anything in trigger and get all colliders2D in It.
        /// </summary>
        /// <param name="position">Trigger's center. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="collider2Ds">List of colliders2D in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(Vector3 position, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true);
        /// <summary>
        /// Checks if there's anything in trigger and get all colliders2D in It.
        /// </summary>
        /// <param name="transform">Trigger's center by transform. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="collider2Ds">List of colliders2D in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(Transform transform, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true);
        /// <summary>
        /// Checks if there's anything in trigger and get all colliders2D in It.
        /// </summary>
        /// <param name="gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="collider2Ds">List of colliders2D in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(GameObject gameObject, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true);
        /// <summary>
        /// Checks if there's anything in trigger and get all colliders2D in It.
        /// </summary>
        /// <param name="gameObject">Trigger's center by game object. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="collider2Ds">List of colliders2D in trigger</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <returns></returns>
        public abstract bool InTrigger(Collider2D collider2D, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true);


        /// <summary>
        /// Checks if there's something in trigger and gets a component from it.
        /// </summary>
        /// <typeparam name="T">A game object component.</typeparam>
        /// <param name="collider2D">Trigger's center by collider2D. (This parameter will be ignored if trigger has a Center Object defined)</param>
        /// <param name="callbacks">Invoke trigger's callbacks when it's true. (True by default)</param>
        /// <param name="debugError">Debug failed tries to get <typeparamref name="T"/> when it's true. (True by default)</param>
        /// <returns>Returns the component got by trigger.</returns>
        public abstract T InTrigger<T>(Collider2D collider2D, int resultCount = 1, bool callbacks = true, bool debugError = true) where T : Component;
        public abstract void DrawTrigger(Collider2D collider2D);
        #endregion
    }

    [System.Serializable]
    public class BoxTrigger2D : System2D
    {
        #region Variables
        [SerializeField, Min(0)] Vector2 m_triggerSize = Vector2.one;
        [SerializeField] float m_triggerAngle = 0f;
        Collider2D[] m_results;
        #region Getters
        public Vector2 m_TriggerSize => m_triggerSize;
        public float m_TriggerAngle => m_triggerAngle;
        public Collider2D[] m_Results => m_results;
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
            if (!m_DrawSettings.m_Draw) return;
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            Gizmos.matrix = Matrix4x4.TRS(position + m_TriggerOffset.ToVector3(), Quaternion.Euler(0f, 0f, m_TriggerAngle), Vector3.one);
            Gizmos.color = InTrigger(position, callbacks: false) ? m_DrawSettings.m_InColor : m_DrawSettings.m_OutColor;

            if (m_DrawSettings.m_DrawSolid)
            {
                Gizmos.DrawCube(Vector2.zero, m_triggerSize);
            }
            else
            {
                Gizmos.DrawWireCube(Vector2.zero, m_triggerSize);
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
        public override bool InTrigger(Vector3 position, int resultCount = 1, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }

            if (m_results == null || m_results.Length != resultCount)
            {
                m_results = new Collider2D[resultCount];
            }

            bool isIn = Physics2D.OverlapBoxNonAlloc(position + m_TriggerOffset.ToVector3(), m_TriggerSize, m_triggerAngle, m_results, m_TriggerLayerMask) > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            
            return isIn;
        }

        public override bool InTrigger(Transform transform, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(transform.position, resultCount, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, resultCount, callbacks);
        }

        public override bool InTrigger(Collider2D collider2D, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(collider2D.transform.position, resultCount, callbacks);
        }

        public override T InTrigger<T>(Vector3 position, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            if (!InTrigger(position, resultCount, callbacks)) return null;
            foreach (Collider2D r in m_results)
            {
                if (r.TryGetComponent(out T component)) return component;
            }
            
            if (debugErrors)
            {
                Debug.LogWarning($"<color=yellow>Warning:</color> None objects in results have: <<color=#DC143C>{typeof(T).Name}</color>> as a component");
            }
            return null;
        }

        public override T InTrigger<T>(Transform transform, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            return InTrigger<T>(transform.position, resultCount, callbacks, debugErrors);
        }

        public override T InTrigger<T>(GameObject gameObject, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            return InTrigger<T>(gameObject.transform.position, resultCount, callbacks, debugErrors);
        }

        public override T InTrigger<T>(Collider2D collider2D, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            return InTrigger<T>(collider2D.transform.position, resultCount, callbacks, debugErrors);
        }

        public override bool InTrigger(Vector3 position, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            bool result = InTrigger(position, resultCount, callbacks);
            collider2Ds = m_results?.ToList();
            return result;
        }

        public override bool InTrigger(Transform transform, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(transform.position, out collider2Ds, resultCount, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, out collider2Ds, resultCount, callbacks);
        }

        public override bool InTrigger(Collider2D collider2D, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(collider2D.transform.position, out collider2Ds, resultCount, callbacks);
        }
        #endregion

        #endregion
    }


    [System.Serializable]
    public class CircleTrigger2D : System2D
    {
        #region Variables
        [SerializeField, Min(0)] float m_triggerRadius = 0.5f;
        Collider2D[] m_results;
        #region Getters
        public float m_TriggerRadius => m_triggerRadius;
        public Collider2D[] m_Results => m_results;
        #endregion

        #endregion

        #region Methods

        public void SetTriggerRadius(float _newRadius)
        {
            m_triggerRadius = _newRadius;
        }

        #region InTrigger
        public override bool InTrigger(Vector3 position, int resultCount = 1, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            if (m_results == null || m_results.Length != resultCount)
            {
                m_results = new Collider2D[resultCount];
            }
            bool isIn = Physics2D.OverlapCircleNonAlloc(position + m_TriggerOffset.ToVector3(), m_TriggerRadius, m_results, m_TriggerLayerMask) > 0;
            if (callbacks)
            {
                InvokeCallbacks(isIn);
            }
            return isIn;
        }

        public override bool InTrigger(Transform transform, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(transform.position,  resultCount, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, resultCount, callbacks);
        }

        public override bool InTrigger(Collider2D collider2D, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(collider2D.transform.position, resultCount, callbacks);
        }

        public override T InTrigger<T>(Vector3 position, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }
            if (!InTrigger(position, resultCount, callbacks)) return null;

            foreach (Collider2D r in m_results)
            {
                if (r.TryGetComponent(out T component)) return component;
            }

            if (debugErrors)
            {
                Debug.LogWarning($"<color=yellow>Warning:</color> None objects in results have: <<color=#DC143C>{typeof(T).Name}</color>> as a component");
            }

            return null;
        }

        public override T InTrigger<T>(Transform transform, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            return InTrigger<T>(transform.position, resultCount, callbacks, debugErrors);
        }

        public override T InTrigger<T>(GameObject gameObject, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            return InTrigger<T>(gameObject.transform.position, resultCount, callbacks, debugErrors);
        }

        public override T InTrigger<T>(Collider2D collider2D, int resultCount = 1, bool callbacks = true, bool debugErrors = true)
        {
            return InTrigger<T>(collider2D.transform.position, resultCount, callbacks, debugErrors);
        }

        public override bool InTrigger(Vector3 position, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            if (m_CenterObject != null)
            {
                position = m_CenterObject.position;
            }

            bool result = InTrigger(position, resultCount, callbacks);
            collider2Ds = m_results?.ToList();
            return result;
        }

        public override bool InTrigger(Transform transform, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(transform.position, out collider2Ds, resultCount, callbacks);
        }

        public override bool InTrigger(GameObject gameObject, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(gameObject.transform.position, out collider2Ds, resultCount, callbacks);
        }

        public override bool InTrigger(Collider2D collider2D, out List<Collider2D> collider2Ds, int resultCount = 1, bool callbacks = true)
        {
            return InTrigger(collider2D.transform.position, out collider2Ds, resultCount, callbacks);
        }
        #endregion


        #region Draw
        public override void DrawTrigger(Vector3 _position)
        {
            if (!m_DrawSettings.m_Draw) return;
            if (m_CenterObject != null)
            {
                _position = m_CenterObject.position;
            }
            Gizmos.color = InTrigger(_position, callbacks: false) ? m_DrawSettings.m_InColor : m_DrawSettings.m_OutColor;

            if (m_DrawSettings.m_DrawSolid)
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