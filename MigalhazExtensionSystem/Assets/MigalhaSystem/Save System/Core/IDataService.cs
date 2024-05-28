using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public interface IDataService
    {
        bool SaveData<T>(string _relativePath, T _data, bool _encrypted);

        T LoadData<T>(string _relativePath, bool _encrypted);

        static IDataService CreateDefault() => new BinaryFormatterDataService();
    }

    public abstract class DataServiceFactory : ScriptableObject
    {
        protected IDataService m_dataService;
        public abstract IDataService ProvideDataService();
    }

    public static class SaveUtils
    {
        static PersistentDataPathFactory m_pathFactory = null;
        public static string GetPersistentDataPath() => (m_pathFactory ??= new DefaultPersistentDataPath()).ProvideDataPath();

        //Factories
        public abstract class PersistentDataPathFactory
        {
            protected string m_persistentDataPath = null;
            public abstract string ProvideDataPath();
        }
        public class DefaultPersistentDataPath : PersistentDataPathFactory
        {
            public DefaultPersistentDataPath()
            {
                m_persistentDataPath = null;
            }

            public override string ProvideDataPath() => DataPathGenerationCheck() ? (m_persistentDataPath = NewPersistentDataPath()) : m_persistentDataPath;
            protected virtual bool DataPathGenerationCheck() => string.IsNullOrWhiteSpace(m_persistentDataPath);
            protected virtual string NewPersistentDataPath() => Application.persistentDataPath;
        }
    }
}