using MigalhaSystem.Extensions;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public class JsonSaveSystemDemo : MonoBehaviour
    {
        [SerializeField] DemoHero m_player;
        [SerializeField] bool m_encrypted;
        [SerializeField] string m_path;
        [ContextMenu("Save")]
        public void Save()
        {
            SaveManager.Instance.SaveData(m_player, m_path, m_encrypted);
        }

        [ContextMenu("Load")]
        public void Load()
        {
            try
            {
                m_player = SaveManager.Instance.LoadData<DemoHero>(m_path, m_encrypted);
            }
            catch 
            {
                Save();
                Load();
            }
        }
    }
    [Serializable]
    public class DemoHero
    {
        public string m_heroName;
        public float m_heroHealth;

        public DemoHero(string heroName, float heroHealth)
        {
            m_heroName = heroName;
            m_heroHealth = heroHealth;
        }
    }
}