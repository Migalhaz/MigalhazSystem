using MigalhaSystem.Extensions;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public class BinaryFormatterDataService : IDataService
    {
        public bool SaveData<T>(string _relativePath, T _data, bool _encrypted)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string relativePath = NormalizeRelativePath(_relativePath);
            string path = SaveUtils.GetPersistentDataPath() + relativePath;
            try
            {
                if (File.Exists(path))
                {
#if DEBUG
                    Debug.LogWarning("Data exists. Deleting old file and writing a new one.");
#endif
                    File.Delete(path);
                }
                else
                {
#if DEBUG
                    Debug.Log("Writing new file!");
#endif
                }
                using FileStream stream = File.Create(path);
                formatter.Serialize(stream, _data);
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
        public T LoadData<T>(string _relativePath, bool _encrypted)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string relativePath = NormalizeRelativePath(_relativePath);

            string path = SaveUtils.GetPersistentDataPath() + relativePath;

            if (!File.Exists(path))
            {
#if DEBUG
                Debug.LogError($"Cannot load file at {path}! File does not exist!".Error());
#endif
                throw new FileNotFoundException($"{path} does not exist!");
            }

            try
            {
                FileStream stream = File.Open(path, FileMode.Open);
                T data = (T) formatter.Deserialize(stream);
                stream.Close();
                return data;
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.LogError($"Failed to load data due to: {e.Message} {e.StackTrace}");
#endif
                throw e;
            }
        }

        string NormalizeRelativePath(string _relativePath)
        {
            string saveExtension = ".save";
            string normalizePath = _relativePath;

            if (!normalizePath.StartsWith('/'))
            {
                normalizePath = $"/{normalizePath}";
            }

            if (!normalizePath.EndsWith(saveExtension))
            {
                normalizePath = $"{normalizePath}{saveExtension}";
            }

            return normalizePath;
        }
    }
}