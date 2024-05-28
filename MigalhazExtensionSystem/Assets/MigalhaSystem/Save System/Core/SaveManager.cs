using UnityEngine;
using System.IO;
using MigalhaSystem.Singleton;

namespace MigalhaSystem.SaveSystem
{
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField] DataServiceFactory m_dataServiceFactory;
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
            return SlotPath();
        }

        [ContextMenu("Change Save Slot")]
        void ChangeSaveSlot()
        {
            if (!Directory.Exists(PersistentDataPath()))
            {
                Directory.CreateDirectory(PersistentDataPath());
            }
        }

        [ContextMenu("Delete Save Slot")]
        public void DeleteSaveSlot()
        {
            if (!Directory.Exists(PersistentDataPath())) return;
            Directory.Delete(PersistentDataPath(), true);
        }

        public void SaveData<T>(T data, string path, bool encrypted)
        {
            ChangeSaveSlot();
            Saver<T>.SaveData(ProvideDataService(), data, $"{GetCurrentPath()}/{path}", encrypted);
        }

        public T LoadData<T>(string path, bool encrypted)
        {
            ChangeSaveSlot();
            return Saver<T>.LoadData(ProvideDataService(), $"{GetCurrentPath()}/{path}", encrypted);
        }

        string PersistentDataPath() => SaveUtils.GetPersistentDataPath() + SlotPath();
        public string SlotPath() => $"/SaveSlot{GetSaveSlot()}";
        IDataService ProvideDataService()
        {
            if (m_dataServiceFactory == null) return IDataService.CreateDefault();
            return m_dataServiceFactory.ProvideDataService();
        }
    }
}