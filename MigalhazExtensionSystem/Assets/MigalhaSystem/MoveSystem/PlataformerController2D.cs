using MigalhaSystem.StateMachine;
using UnityEngine;

namespace MigalhaSystem.Move
{
	[RequireComponent(typeof(Rigidbody2D))]
    public class PlataformerController2D : StateMachineController
	{
        [Header("Components")]
		[SerializeField] Rigidbody2D m_rig;
        public Rigidbody2D m_Rig => m_rig;

        [Header("States")]
        [SerializeField] PlataformerController2DIdleState m_idleState;
        [SerializeField] PlataformerController2DMoveState m_moveState;
        [SerializeField] PlataformerController2DJumpState m_jumpState;
        [SerializeField] PlataformerController2DDashState m_dashState;

        public Vector2 GetAxisInput() => new(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        protected override AbstractState FirstState()
        {
            throw new System.NotImplementedException();
        }

        private void Reset()
        {
            TryGetComponent(out m_rig);
        }
    }

    [System.Serializable]
    public class PlataformerController2DIdleState : AbstractState<PlataformerController2D>
    {
        public override void EnterState(StateMachineController stateMachineController)
        {
            base.EnterState(stateMachineController);
            m_controller.m_Rig.velocity = Vector3.zero;
        }

        public override void UpdateState(StateMachineController stateMachineController)
        {
            base.UpdateState(stateMachineController);
            if (m_controller.GetAxisInput().x != 0)
            {

            }
        }
    }

    [System.Serializable]
    public class PlataformerController2DMoveState : AbstractState<PlataformerController2D>, IMovable
    {
        public void Move()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 MoveDirection()
        {
            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    public class PlataformerController2DJumpState : AbstractState<PlataformerController2D>, IJumpable
    {
        public void Jump()
        {
            throw new System.NotImplementedException();
        }
    }

    [System.Serializable]
    public class PlataformerController2DDashState : AbstractState<PlataformerController2D>, IDashable
    {
        public void Dash()
        {
            throw new System.NotImplementedException();
        }

        public Vector3 DashSirection()
        {
            throw new System.NotImplementedException();
        }
    }
}