using UnityEngine;
using System.IO;
using MigalhaSystem.Singleton;

namespace MigalhaSystem.SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField, Min(0)] int m_currentSaveSlot;
        public string m_CurrentPath { get; private set; }
        public void SetSaveSlot(int newSaveSlot)
        {
            m_currentSaveSlot = newSaveSlot;
            ChangeSaveSlot();
        }

        public int GetSaveSlot()
        {
            return m_currentSaveSlot;
        }

        public string GetCurrentPath()
        {
            ChangeSaveSlot();
            return m_CurrentPath;
        }

        [ContextMenu("Change Save Slot")]
        void ChangeSaveSlot()
        {
            m_CurrentPath = $"/SaveSlot{GetSaveSlot()}";

            string persistentDataPath = Application.persistentDataPath + m_CurrentPath;
            if (!Directory.Exists(persistentDataPath))
            {
                Directory.CreateDirectory(persistentDataPath);
            }
        }

        [ContextMenu("Delete Save Slot")]
        public void DeleteSaveSlot()
        {
            m_CurrentPath = $"/SaveSlot{GetSaveSlot()}";
            string persistentDataPath = Application.persistentDataPath + m_CurrentPath;
            if (!Directory.Exists(persistentDataPath)) return;
            Directory.Delete(persistentDataPath, true);
        }

        public void SaveData<T>(T data, string path, bool encrypted)
        {
            ChangeSaveSlot();
            Saver<T>.SaveData(data, $"{GetCurrentPath()}/{path}", encrypted);
        }

        public T LoadData<T>(string path, bool encrypted)
        {
            ChangeSaveSlot();
            return Saver<T>.LoadData($"{GetCurrentPath()}/{path}", encrypted);
        }
    }
}