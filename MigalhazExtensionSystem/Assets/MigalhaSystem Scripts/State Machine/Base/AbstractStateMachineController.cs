using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.StateMachine
{
    [DisallowMultipleComponent]
    public class AbstractStateMachineController : MonoBehaviour
    {
        [Header("Components")]
        protected AbstractState m_currentState;
        protected AbstractState m_lastState;
        
        #region Getters
        public AbstractState m_CurrentState => m_currentState;
        public AbstractState m_LastState => m_lastState;
        #endregion
        protected virtual void Update()
        {
            m_currentState?.UpdateState(this);
        }

        protected virtual void FixedUpdate()
        {
            m_currentState?.FixedUpdateState(this);
        }

        /// <summary>
        /// Muda o estado atual do inimigo.
        /// </summary>
        /// <param name="newState">Novo estado do inimigo.</param>
        public virtual void SwitchState(AbstractState newState)
        {
            if (m_currentState is not null)
            {
                if (!m_currentState.m_CanBeInterrupt)
                {
                    return;
                }
                m_currentState.ExitState(this);
            }
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterState(this);
        }

        public virtual void ForceSwitchState(AbstractState newState)
        {
            if (m_currentState is not null)
            {
                m_currentState.ExitState(this);
            }
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterState(this);
        }

        public bool CheckCurrentState<T>() where T : AbstractState
        {
            return m_currentState is T;
        }

        public bool CheckLastState<T>() where T : AbstractState
        {
            return m_lastState is T;
        }
    }
}
