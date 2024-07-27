using MigalhaSystem.Extensions;
using MigalhaSystem.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MigalhaSystem
{
    [CreateAssetMenu(fileName = "BinaryFormatterSaveStrategy", menuName = "Scriptable Object/Save System/Save Strategies/Binary Formatter Save Strategy")]
    public class BinaryFormatterSaveStrategy : AbstractSaveStrategy
    {
        BinaryFormatter m_binaryFormatter;
        public override bool WriteData(string data, string path)
        {
            try
            {
                using FileStream stream = FileManagement.CreateNewFile(path);
                ProvideBinaryFormatter().Serialize(stream, data);
                stream.Close();
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
                FileStream stream = File.Open(path, FileMode.Open);
                string result = ProvideBinaryFormatter().Deserialize(stream) as string;
                stream.Close();
                return result;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
#endif
                throw e;
            }
        }

        public override string GetFileExtension() => ".save";
        BinaryFormatter ProvideBinaryFormatter() => m_binaryFormatter ??= new BinaryFormatter();
    }
}
