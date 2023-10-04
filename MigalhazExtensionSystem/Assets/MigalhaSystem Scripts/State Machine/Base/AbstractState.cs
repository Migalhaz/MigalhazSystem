using UnityEngine;
namespace MigalhaSystem.StateMachine
{
    [System.Serializable]
    public abstract class AbstractState
    {
        [SerializeField] protected bool m_canBeInterrupt = true;
        public bool m_CanBeInterrupt => m_canBeInterrupt;
        protected AbstractStateMachineController m_abstractStateMachineController;

        public virtual void EnterState(AbstractStateMachineController stateMachineController) 
        {
            m_abstractStateMachineController = stateMachineController;
        }
        public virtual void UpdateState(AbstractStateMachineController stateMachineController) { }
        public virtual void FixedUpdateState(AbstractStateMachineController stateMachineController) { }
        public virtual void ExitState(AbstractStateMachineController stateMachineController) { }
    }
}