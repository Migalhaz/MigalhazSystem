using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public interface IDataService
    {
        bool SaveData<T>(string _relativePath, T _data, bool _encrypted);

        T LoadData<T>(string _relativePath, bool _encrypted);
    }
}