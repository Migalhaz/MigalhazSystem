using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.StateMachine
{
    [DisallowMultipleComponent]
    public abstract class StateMachineController : MonoBehaviour
    {
        protected AbstractState m_currentState;
        protected AbstractState m_lastState;

        #region Getters
        public AbstractState m_CurrentState => m_currentState;
        public AbstractState m_LastState => m_lastState;
        #endregion

        protected virtual void StartController()
        {
            m_currentState = null;
            m_lastState = null;
            ForceSwitchState(FirstState());
        }

        protected abstract AbstractState FirstState();

        protected virtual void Update()
        {
            m_currentState?.UpdateStateLogic(this);
        }

        protected virtual void FixedUpdate()
        {
            m_currentState?.FixedUpdateState(this);
        }

        protected virtual void LateUpdate()
        {
            m_currentState?.LateUpdateState(this);
        }

        protected virtual void OnDestroy()
        {
            m_currentState?.ExitStateLogic(this);
            m_currentState = null;
            m_lastState = null;
        }

        public virtual void SwitchState(AbstractState newState, bool restartState = false)
        {
            if (m_currentState == newState && !restartState) return;
            if (m_currentState is not null)
            {
                if (!m_currentState.m_CanBeInterrupt) return;
                m_currentState.ExitStateLogic(this);
            }
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterStateLogic(this);
        }

        public virtual void ForceSwitchState(AbstractState newState, bool restartState = false)
        {
            if (m_currentState == newState && !restartState) return;
            if (m_currentState is not null) m_currentState.ExitStateLogic(this);
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterStateLogic(this);
        }

        public bool CheckCurrentState<T>() where T : AbstractState => m_currentState is T;
        public bool CheckLastState<T>() where T : AbstractState => m_lastState is T;
        public static bool CheckState<T>(AbstractState state) where T : AbstractState => state is T;
    }
}
