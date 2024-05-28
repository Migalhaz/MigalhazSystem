using UnityEngine;
namespace MigalhaSystem.Move
{
	public interface IMovable
	{
		void Move();

		Vector3 MoveDirection();
	}

	public interface IJumpable
	{
		void Jump();
	}

	public interface IDashable
	{
		void Dash();
		Vector3 DashSirection();
	}
}