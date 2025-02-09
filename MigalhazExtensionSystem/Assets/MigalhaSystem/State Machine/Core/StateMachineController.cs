using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MigalhaSystem.StateMachine
{
    public abstract class StateMachineController : MonoBehaviour
    {
        protected AbstractState m_currentState;
        protected AbstractState m_lastState;
        protected bool m_isPlaying;

        [Header("Controller Events")]
        [SerializeField] UnityEvent<StateMachineController> m_onStartController;
        [SerializeField] UnityEvent<StateMachineController> m_onStopController;

        #region Getters
        public AbstractState m_CurrentState => m_currentState;
        public AbstractState m_LastState => m_lastState;

        public UnityEvent<StateMachineController> OnStartController => m_onStartController;
        public UnityEvent<StateMachineController> OnStopController => m_onStopController;
        #endregion

        protected virtual void StartController()
        {
            m_isPlaying = true;
            m_currentState = null;
            m_lastState = null;
            ForceSwitchState(FirstState());
            m_onStartController?.Invoke(this);
        }
        protected virtual void StopController()
        {
            m_isPlaying = false;
            m_currentState?.ExitStateLogic(this);
            m_currentState = null;
            m_lastState = null;
            m_onStopController?.Invoke(this);
        }
        protected abstract AbstractState FirstState();

        protected virtual void Update()
        {
            if (IsPlaying())
            {
                m_currentState?.UpdateStateLogic(this);
            }
        }
        protected virtual void FixedUpdate()
        {
            if (IsPlaying())
            {
                m_currentState?.FixedUpdateState(this);
            }
        }
        protected virtual void LateUpdate()
        {
            if (IsPlaying())
            {
                m_currentState?.LateUpdateState(this);
            }
        }
        protected virtual void OnDestroy()
        {
            StopController();
        }

        public virtual void SwitchState(AbstractState newState, bool restartState = false)
        {
            if (!m_isPlaying)
            {
#if DEBUG
                Debug.LogWarning
                    ($"You can't switch a state in an inactive controller!");
#endif
                return;
            }

            if (newState == null)
            {
#if DEBUG
                Debug.LogWarning
                    ($"You can't switch {name} to a null state! \nIf you want to stop the controller, use the {nameof(StopController)} method instead.");
#endif
                return;
            }

            if (m_currentState == newState && !restartState)
            {
#if DEBUG
                Debug.LogWarning
                    ($"There was an attempt to re-enter the current state. If you want to restart the state, set the boolean '{nameof(restartState)}' to true.");
#endif
                return;
            }

            if (m_currentState is not null)
            {
                if (!m_currentState.m_CanBeInterrupt)
                {
#if DEBUG
                    Debug.LogWarning
                        ($"It was not possible to change the current state {nameof(m_currentState)} because the boolean 'CanBeInterrupt' is false.\nTo change the current state, set this boolean to true or use the {nameof(ForceSwitchState)} method instead.");
#endif
                    return;
                }
                m_currentState.ExitStateLogic(this);
            }
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterStateLogic(this);
        }

        public virtual void ForceSwitchState(AbstractState newState, bool restartState = false)
        {
            if (!m_isPlaying)
            {
#if DEBUG
                Debug.LogWarning
                    ($"You can't switch a state in an inactive controller!");
#endif
                return;
            }

            if (newState == null)
            {
#if DEBUG
                Debug.LogWarning
                    ($"You can't switch {name} to a null state! \nIf you want to stop the controller, use the {nameof(StopController)} method instead.");
#endif
                return;
            }

            if (m_currentState == newState && !restartState)
            {
#if DEBUG
                Debug.LogWarning
                    ($"There was an attempt to re-enter the current state. If you want to restart the state, set the boolean '{nameof(restartState)}' to true.");
#endif
                return;
            }

            m_currentState?.ExitStateLogic(this);
            m_lastState = m_currentState;
            m_currentState = newState;
            m_currentState.EnterStateLogic(this);
        }

        public virtual bool IsPlaying() => m_isPlaying && m_currentState != null;
        public bool CheckCurrentState<T>() where T : AbstractState => m_currentState is T;
        public bool CheckLastState<T>() where T : AbstractState => m_lastState is T;
        public static bool CheckState<T>(AbstractState state) where T : AbstractState => state is T;
    }
}