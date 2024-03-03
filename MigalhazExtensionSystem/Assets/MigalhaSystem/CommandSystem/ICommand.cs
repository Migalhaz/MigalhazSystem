using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.Command
{
    public interface ICommand
    {
        void Execute();
        void Undo();
        bool Finished();
    }
}