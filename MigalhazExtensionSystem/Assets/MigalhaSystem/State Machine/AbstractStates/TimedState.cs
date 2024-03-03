using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.StateMachine
{
    [System.Serializable]
    public abstract class TimedState : AbstractState
    {
        [SerializeField] Extensions.Timer m_timer;
        public override void EnterState(StateMachineController stateMachineController)
        {
            base.EnterState(stateMachineController);
            m_timer.ActiveTimer(true);
        }
        public override void UpdateState(StateMachineController stateMachineController)
        {
            base.UpdateState(stateMachineController);
            if (TimeElapsed()) OnCooldownEnd();
        }
        protected Extensions.Timer GetTimer() => m_timer;
        protected virtual bool TimeElapsed() => m_timer.TimerElapse(Time.deltaTime);
        protected abstract void OnCooldownEnd();
    }

    [System.Serializable]
    public abstract class TimedState<T> : AbstractState<T> where T : StateMachineController
    {
        [SerializeField] Extensions.Timer m_timer;
        public override void EnterState(StateMachineController stateMachineController)
        {
            base.EnterState(stateMachineController);
            m_timer.ActiveTimer(true);
        }
        public override void UpdateState(StateMachineController stateMachineController)
        {
            base.UpdateState(stateMachineController);
            if (TimeElapsed()) OnCooldownEnd();
        }
        protected Extensions.Timer GetTimer() => m_timer;
        protected virtual bool TimeElapsed() => m_timer.TimerElapse(Time.deltaTime);
        protected abstract void OnCooldownEnd();
    }
}
