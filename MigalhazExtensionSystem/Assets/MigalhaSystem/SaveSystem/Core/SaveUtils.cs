using MigalhaSystem.Extensions;
using Newtonsoft.Json;
using System;
using System.IO;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public abstract class AbstractSaveStrategy : ScriptableObject
    {
        public abstract bool WriteData(string data, string path);
        public abstract string ReadData(string path);
        public abstract string GetFileExtension();

        protected virtual bool CheckDataFile(string path)
        {
            if (!File.Exists(path))
            {
#if DEBUG
                Debug.LogError($"Cannot load file at {path}! File does not exist!".Error());
#endif
                return false;
                throw new FileNotFoundException($"{path} does not exist!");
            }
            return true;
        }
    }

    public static class SaveJsonConverter
    {
        public static string ToJson(object obj) => JsonConvert.SerializeObject(obj);
        public static T FromJson<T>(string json) => JsonConvert.DeserializeObject<T>(json);
    }

    public static class SaveUtils
    {
        public static string NormalizeRelativePath(string _relativePath, string saveExtension)
        {
            string normalizePath = $"{_relativePath}";
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

    public static class FileManagement
    {
        public static FileStream CreateNewFile(string path)
        {
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
                return File.Create(path);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.LogError($"Unable to save data due to: {e.Message} {e.StackTrace}");
#endif
                throw e;
            }
        }

        public static bool CheckFile(string path) => File.Exists(path);
    }
    public static class DirectoryManagement
    {
        public static bool CheckDirectory(string path) => Directory.Exists(path);
        public static bool CheckDirectory(int saveSlot) => CheckDirectory(GetPathOfSaveSlot(saveSlot));

        public static void CreateDirectory(string path)
        {
            if (CheckDirectory(path)) return;
            Directory.CreateDirectory(path);
        }

        public static void CreateDirectory(int saveSlot) => CreateDirectory(GetPathOfSaveSlot(saveSlot));

        public static void DeleteDirectory(string path)
        {
            if (!CheckDirectory(path)) return;
            Directory.Delete(path, true);
        }
        public static void DeleteDirectory(int saveSlot) => DeleteDirectory(GetPathOfSaveSlot(saveSlot));


        public static void OverrideDirectory(string path)
        {
            DeleteDirectory(path);
            CreateDirectory(path);
        }
        public static void OverrideDirectory(int saveSlot) => OverrideDirectory(GetPathOfSaveSlot(saveSlot));

        public static string GetPathOfSaveSlot(int saveSlot) => GetPersistentDataPath() + GetSlotPath(saveSlot);
        public static string GetPersistentDataPath() => Application.persistentDataPath;
        public static string GetSlotPath(int saveSlot) => $"/SaveSlot{saveSlot}";
    }
}
