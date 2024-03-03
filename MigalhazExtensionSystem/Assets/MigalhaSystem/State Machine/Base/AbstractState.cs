using UnityEngine;
using UnityEngine.Events;
namespace MigalhaSystem.StateMachine
{
    [System.Serializable]
    public abstract class AbstractState
    {
        [Header("State Settings")]
        [SerializeField] protected bool m_canBeInterrupt = true;
        public bool m_CanBeInterrupt => m_canBeInterrupt;

        [Header("State Events")]
        [SerializeField] StateEvents m_stateEvents;
        public StateEvents m_StateEvents => m_stateEvents;

        public virtual void EnterState(StateMachineController stateMachineController) { m_stateEvents.m_OnEnterState?.Invoke(); }
        public virtual void UpdateState(StateMachineController stateMachineController) { m_stateEvents.m_OnUpdateState?.Invoke(); }
        public virtual void FixedUpdateState(StateMachineController stateMachineController) { }
        public virtual void LateUpdateState(StateMachineController stateMachineController) { }
        public virtual void ExitState(StateMachineController stateMachineController) { m_stateEvents.m_OnExitState?.Invoke(); }
    }

    [System.Serializable]
    public class StateEvents
    {
        [SerializeField] UnityEvent m_onEnterState;
        [SerializeField] UnityEvent m_onUpdateState;
        [SerializeField] UnityEvent m_onExitState;

        public UnityEvent m_OnEnterState => m_onEnterState;
        public UnityEvent m_OnUpdateState => m_onUpdateState;
        public UnityEvent m_OnExitState => m_onExitState;
    }

    [System.Serializable]
    public abstract class AbstractState<T> : AbstractState where T : StateMachineController
    {
        protected T m_controller;
        public override void EnterState(StateMachineController stateMachineController)
        {
            base.EnterState(stateMachineController);
            T setController = stateMachineController as T;

            if (setController == null)
            {
                Debug.LogError($"You can't convert {typeof(StateMachineController)} to {typeof(T)}");
                return;
            }
            m_controller = setController;
        }
    }
}