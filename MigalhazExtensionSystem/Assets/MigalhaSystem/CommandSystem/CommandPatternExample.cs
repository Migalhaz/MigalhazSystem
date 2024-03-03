using MigalhaSystem.Command;
using MigalhaSystem.Pool;
using UnityEngine;

public class CommandPatternExample : MonoBehaviour
{
    [SerializeField] CommandInvoker commandInvoker;
    //[SerializeField] Switcher switcher;
    [SerializeField] PoolData poolData;

    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Adding command");
            commandInvoker.AddCommand(new Spawner() { m_pool = poolData });
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            commandInvoker.ExecuteCommand();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            commandInvoker.UndoCommand();
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            commandInvoker.ExecuteAllCommands();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            commandInvoker.UndoAllCommands();
        }
    }
}

[System.Serializable]
public class Switcher : ICommand
{
    [SerializeField] int index = 0;
    public void Execute()
    {
        index++;
        Debug.Log($"Execute {index}");
    }

    public bool Finished()
    {
        return true;
    }

    public void Undo()
    {
        Debug.Log($"Undo {index}");
        index--;
    }
}

[System.Serializable]
public class Spawner : ICommand
{
    public PoolData m_pool;
    GameObject a;
    public void Execute()
    {
        a = PoolManager.Instance.PullObject(m_pool, Vector3.zero, Quaternion.identity);
    }

    public bool Finished()
    {
        return true;
    }

    public void Undo()
    {
        PoolManager.Instance.PushObject(m_pool, a);
    }
}
