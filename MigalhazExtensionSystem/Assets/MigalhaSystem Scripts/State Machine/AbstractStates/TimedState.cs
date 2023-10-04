using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.StateMachine
{
    [System.Serializable]
    public abstract class TimedState : AbstractState
    {
        [SerializeField, Min(0)] float m_timerToExitState;
        float m_currentTime;

        public override void EnterState(AbstractStateMachineController stateMachineController)
        {
            m_currentTime = m_timerToExitState;
        }

        public override void UpdateState(AbstractStateMachineController stateMachineController)
        {
            if (TimeElapsed())
            {
                OnCooldownEnd();
            }
        }

        public override void FixedUpdateState(AbstractStateMachineController stateMachineController){}

        public override void ExitState(AbstractStateMachineController stateMachineController){}

        protected virtual bool TimeElapsed()
        {
            m_currentTime -= Time.deltaTime;
            return m_currentTime <= 0;
        }

        protected abstract void OnCooldownEnd();
    }
}
