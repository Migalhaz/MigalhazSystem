using System.Collections.Generic;
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

        protected TransitionController m_transitionController;

        public virtual void EnterStateLogic(StateMachineController stateMachineController)
        {
            m_transitionController ??= new();
            EnterState(stateMachineController);
            m_stateEvents.m_OnEnterState?.Invoke();
        }

        public void UpdateStateLogic(StateMachineController stateMachineController)
        {
            m_transitionController.CheckTransitions();
            UpdateState(stateMachineController);
            m_stateEvents.m_OnUpdateState?.Invoke();
        }

        public void ExitStateLogic(StateMachineController stateMachineController)
        {
            ClearTransitions();
            ExitState(stateMachineController);
            m_stateEvents.m_OnExitState?.Invoke();
        }

        public void AddTransition(StateTransition newStateTransition)
        {
            m_transitionController ??= new();
            m_transitionController.AddTransition(newStateTransition);
        }

        public void RemoveTransition(StateTransition stateTransitionToRemove)
        {
            m_transitionController ??= new();
            m_transitionController.RemoveTransition(stateTransitionToRemove);
        }

        public virtual void ClearTransitions()
        {
            if (m_transitionController == null) return;
            m_transitionController.ClearTransitions();
        }

        public virtual void EnterState(StateMachineController stateMachineController) {  }
        public virtual void UpdateState(StateMachineController stateMachineController) { }
        public virtual void FixedUpdateState(StateMachineController stateMachineController) { }
        public virtual void LateUpdateState(StateMachineController stateMachineController) { }
        public virtual void ExitState(StateMachineController stateMachineController) {  }
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
        public sealed override void EnterStateLogic(StateMachineController stateMachineController)
        {
            base.EnterStateLogic(stateMachineController);
            T setController = stateMachineController as T;
            if (setController == null)
            {
                Debug.LogError($"You can't convert {typeof(StateMachineController)} to {typeof(T)}");
                return;
            }
            m_controller = setController;
        }
    }

    public class StateTransition
    {
        System.Func<bool> m_evaluate;
        AbstractState m_newState;
        StateMachineController m_controller;
        public StateTransition(System.Func<bool> condition, AbstractState newState, StateMachineController controller)
        {
            m_evaluate = condition;
            m_newState = newState;
            m_controller = controller;
        }

        public void Transition()
        {
            if (m_controller == null || m_newState == null) return;
            if (m_evaluate.Invoke()) m_controller.SwitchState(m_newState);
        }
    }

    public class TransitionController
    {
        List<StateTransition> m_transitions = new List<StateTransition>();

        public TransitionController()
        {
            m_transitions = new List<StateTransition>();
        }

        public void CheckTransitions()
        {
            m_transitions ??= new();
            for (int i = 0; i < m_transitions.Count; i++)
            {
                m_transitions[i].Transition();
            }
        }

        public void AddTransition(StateTransition newStateTransition)
        {
            m_transitions ??= new();
            m_transitions.Add(newStateTransition);
        }

        public void RemoveTransition(StateTransition stateTransitionToRemove)
        {
            m_transitions ??= new();
            m_transitions.Remove(stateTransitionToRemove);
        }

        public void ClearTransitions()
        {
            m_transitions ??= new();
            m_transitions.Clear();
        }
    }
}