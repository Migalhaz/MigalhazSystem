//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Trigger.System3D.Manager
//{
//    public class TriggersManager3D : MonoBehaviour
//    {
//        [SerializeField] Core.UpdateMethod updateMethod = Core.UpdateMethod.FixedUpdate;
//        [SerializeField] List<BoxTrigger3D> boxes = new List<BoxTrigger3D>() { new BoxTrigger3D() };
//        [SerializeField] List<SphereTrigger3D> circles = new List<SphereTrigger3D>() { new SphereTrigger3D() };

//        void Update()
//        {
//            if (!updateMethod.Equals(Core.UpdateMethod.Update)) return;
//            boxes.ForEach(x => x.InTrigger(transform.position));
//            circles.ForEach(x => x.InTrigger(transform.position));
//        }

//        void FixedUpdate()
//        {
//            if (!updateMethod.Equals(Core.UpdateMethod.FixedUpdate)) return;
//            boxes.ForEach(x => x.InTrigger(transform.position));
//            circles.ForEach(x => x.InTrigger(transform.position));
//        }

//        void LateUpdate()
//        {
//            if (!updateMethod.Equals(Core.UpdateMethod.LateUpdate)) return;
//            boxes.ForEach(x => x.InTrigger(transform.position));
//            circles.ForEach(x => x.InTrigger(transform.position));
//        }

//        #region Gizmos
//#if UNITY_EDITOR
//        private void OnDrawGizmos()
//        {
//            foreach (BoxTrigger3D trigger in boxes)
//            {
//                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmos) continue;
//                trigger.DrawTrigger(transform.position);
//            }

//            foreach (SphereTrigger3D trigger in circles)
//            {
//                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmos) continue;
//                trigger.DrawTrigger(transform.position);
//            }
//        }

//        private void OnDrawGizmosSelected()
//        {
//            foreach (BoxTrigger3D trigger in boxes)
//            {
//                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmosSelect) continue;
//                trigger.DrawTrigger(transform.position);
//            }

//            foreach (SphereTrigger3D trigger in circles)
//            {
//                if (trigger.m_DrawSettings.m_DrawMethod != Core.DrawTrigger.DrawMode.OnDrawGizmosSelect) continue;
//                trigger.DrawTrigger(transform.position);
//            }
//        }
//#endif
//        #endregion
//    }
//}