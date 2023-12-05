using MigalhaSystem.Extensions;
using Newtonsoft.Json;
using System;
using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
    public class SaveSystemDemo : MonoBehaviour
    {
        [Header("Demo")]
        public DemoHero m_player = new("Miguel", 100);

        [Header("Save System")]
        [SerializeField] bool m_encrypted;
        const string m_DEMOPATH = "demo";
        IDataService m_dataService = new JsonDataService();
        long m_saveTime;
        long m_loadTime;
        public void SaveData()
        {
            long startTime = DateTime.Now.Ticks;

            if (m_dataService.SaveData(m_DEMOPATH, m_player, m_encrypted))
            {
                m_saveTime = DateTime.Now.Ticks - startTime;
                Debug.Log($"The save operation was carried out successfully!".Color(Color.green) + $" Save Time: {(m_saveTime / TimeSpan.TicksPerMillisecond):N4}ms".Bold());
            }
            else
            {
                Debug.Log("The save operation was failed!".Error());
            }
        }

        public void LoadData()
        {
            long startTime = DateTime.Now.Ticks;
            try
            {
                m_loadTime = DateTime.Now.Ticks - startTime;
                DemoHero demoHero = m_dataService.LoadData<DemoHero>(m_DEMOPATH, m_encrypted);
                m_player = demoHero;
                Debug.Log($"The load operation was carried out successfully!".Color(Color.green) + $" Load Time: {(m_loadTime / TimeSpan.TicksPerMillisecond):N4}ms".Bold());
            }
            catch
            {
                Debug.Log("The load operation was failed!".Error());
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