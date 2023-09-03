using UnityEngine;
namespace MigalhaSystem.StateMachine
{
    [System.Serializable]
    public abstract class AbstractEnemyState
    {
        [SerializeField] protected bool m_canBeInterrupt = true;
        public bool m_CanBeInterrupt => m_canBeInterrupt;

        public abstract void EnterState(AbstractEnemyStateMachineController stateMachineController);
        public abstract void UpdateState(AbstractEnemyStateMachineController stateMachineController);
        public abstract void ExitState(AbstractEnemyStateMachineController stateMachineController);
    }
}