using MigalhaSystem.SaveSystem;
using System.IO;
using System;
using UnityEngine;
using MigalhaSystem.Extensions;

namespace MigalhaSystem
{
    [CreateAssetMenu(fileName = "BasicJsonSaveStrategy", menuName = "Scriptable Object/Save System/Save Strategies/Basic Json Save Strategy")]
    public class BasicJsonSaveStrategy : AbstractSaveStrategy
    {
        public override string GetFileExtension() => ".json";
        
        public override bool WriteData(string data, string path)
        {
            try
            {
                using FileStream stream = FileManagement.CreateNewFile(path);
                stream.Close();
                File.WriteAllText(path, data);
                return true;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
#endif
                return false;
            }
        }

        public override string ReadData(string path)
        {
            if (!CheckDataFile(path)) return null;
            try
            {
                return File.ReadAllText(path);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
#endif
                throw e;
            }
        }
    }
}