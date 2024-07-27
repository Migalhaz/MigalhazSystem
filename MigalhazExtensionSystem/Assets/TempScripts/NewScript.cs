using MigalhaSystem.Analysis;
using MigalhaSystem.Extensions;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    List<Command> commands = new List<Command>();
    private void Update()
    {
        Vector2 direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (direction.magnitude != 0)
        {
            MoveCommand moveCommand = new MoveCommand(transform, direction);
            commands.Add(moveCommand);
            moveCommand.Do();
        }

        if (Input.GetKeyDown(KeyCode.U) && commands.Count > 0)
        {
            int index = commands.Count - 1;
            commands[index].Undo();
            commands.RemoveAt(index);
        }
    }
}
public abstract class Command
{
    
    public abstract void Do();
    public abstract void Undo();
}

public class MoveCommand : Command
{
    Transform transform;
    Vector3 direcao;
    public MoveCommand(Transform transform, Vector3 direcao)
    {
        this.transform = transform;
        this.direcao = direcao;
    }

    public override void Do()
    {
        transform.position += direcao;
    }

    public override void Undo()
    {
        transform.position -= direcao;
    }
}