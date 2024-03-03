using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Command
{
    public class CommandInvoker : MonoBehaviour
    {
        Queue<ICommand> m_commands;
        Stack<ICommand> m_executedCommands;

        private void Awake()
        {
            m_commands = new Queue<ICommand>();
            m_executedCommands = new Stack<ICommand>();
        }

        public void AddCommand(ICommand newCommand)
        {
            m_commands.Enqueue(newCommand);
        }

        public void ExecuteAllCommands()
        {
            while (m_commands.Count > 0)
            {
                ExecuteCommand();
            }
        }

        public void UndoAllCommands()
        {
            while (m_executedCommands.Count > 0)
            {
                UndoCommand();
            }
        }

        public void ExecuteCommand()
        {
            if (m_commands.Count <= 0) return;
            ICommand command = m_commands.Dequeue();
            command.Execute();
            m_executedCommands.Push(command);
        }

        public void UndoCommand()
        {
            if (m_executedCommands.Count <= 0) return;
            ICommand command = m_executedCommands.Pop();
            command.Undo();
        }
    }
}