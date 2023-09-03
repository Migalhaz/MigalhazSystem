using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.StateMachine
{
    [DisallowMultipleComponent]
    public class AbstractEnemyStateMachineController : MonoBehaviour
    {
        [Header("Components")]
        protected AbstractEnemyState m_currentState;
        protected AbstractEnemyState m_lastState;
        
        #region Getters
        public AbstractEnemyState m_CurrentState => m_currentState;
        public AbstractEnemyState m_LastState => m_lastState;
        #endregion
        protected virtual void Update()
        {
            m_currentState?.UpdateState(this);
        }

        /// <summary>
        /// Muda o estado atual do inimigo.
        /// </summary>
        /// <param name="newState">Novo estado do inimigo.</param>
        public virtual void SwitchState(AbstractEnemyState newState)
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

        public virtual void ForceSwitchState(AbstractEnemyState newState)
        {
            if (m_currentState is not null)
            {
                m_currentState.ExitState(this);
            }
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterState(this);
        }

        public virtual void SwitchStateByBool(bool verification, AbstractEnemyState trueState, AbstractEnemyState falseState)
        {
            if (verification)
            {
                if (m_currentState.GetType() != trueState.GetType())
                {
                    SwitchState(trueState);
                }
            }
            else
            {
                if (m_currentState.GetType() != falseState.GetType())
                {
                    SwitchState(falseState);
                }

            }
        }

        public bool CheckCurrentState<T>() where T : AbstractEnemyState
        {
            return m_currentState is T;
        }

        public bool CheckLastState<T>() where T : AbstractEnemyState
        {
            return m_lastState is T;
        }
    }
}
