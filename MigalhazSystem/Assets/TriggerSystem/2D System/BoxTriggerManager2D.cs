using System.Collections.Generic;
using UnityEngine;

namespace Trigger.System2D.Manager
{
    public class BoxTriggerManager2D : MonoBehaviour
    {
        [SerializeField] Core.UpdateMethod m_updateMethod = Core.UpdateMethod.FixedUpdate;
        [SerializeField] List<BoxTrigger2D> m_triggers = new List<BoxTrigger2D>() { new BoxTrigger2D() };

        void Update()
        {
            CallTrigger(Core.UpdateMethod.Update);
        }

        void FixedUpdate()
        {
            CallTrigger(Core.UpdateMethod.FixedUpdate);
        }

        void LateUpdate()
        {
            CallTrigger(Core.UpdateMethod.LateUpdate);
        }

        void CallTrigger(Core.UpdateMethod callMethod)
        {
            if (callMethod != m_updateMethod) return;
            foreach (BoxTrigger2D trigger in m_triggers)
            {
                trigger.InTrigger(transform.localPosition);
            }
        }

        #region Gizmos
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (BoxTrigger2D trigger in m_triggers)
            {
                if (trigger.m_DrawSettings.DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmos) continue;
                trigger.DrawTrigger(transform.position);
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (BoxTrigger2D trigger in m_triggers)
            {
                if (trigger.m_DrawSettings.DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmosSelect) continue;
                trigger.DrawTrigger(transform.position);
            }
        }
#endif
        #endregion
    }
}
