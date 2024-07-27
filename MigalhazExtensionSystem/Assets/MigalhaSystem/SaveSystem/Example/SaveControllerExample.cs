using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.SaveSystem.Example
{
	public class SaveControllerExample : MonoBehaviour
	{
		[Header("Settings")]
		[SerializeField] int m_currentSaveSlot;
		[SerializeField] bool m_saveOnLoadError = true;

		[Header("Data")]
		[SerializeField] SaveObject<Hero> m_heroSave;
		[SerializeField] SaveObject<Foo> m_foo;
		public int GetCurrentSaveSlot() => m_currentSaveSlot;
		
		void SaveHero()
		{
            DirectoryManagement.CreateDirectory(GetCurrentSaveSlot());
            m_heroSave.Save(GetCurrentSaveSlot());
        }

        void LoadHero()
        {
            if (!CanLoad())
            {
				if (m_saveOnLoadError) SaveHero();
                return;
            }

            m_heroSave.Load(GetCurrentSaveSlot());
            return;

            bool CanLoad()
            {
                if (!DirectoryManagement.CheckDirectory(GetCurrentSaveSlot())) return false;
                if (!FileManagement.CheckFile(m_heroSave.GetPath(GetCurrentSaveSlot()))) return false;
                return true;
            }
        }

		void SaveA()
		{
            DirectoryManagement.CreateDirectory(GetCurrentSaveSlot());
			m_foo.Save(GetCurrentSaveSlot());
        }

		void LoadA()
		{
            if (!CanLoad())
            {
				if (m_saveOnLoadError) SaveA();
                return;
            }

            m_foo.Load(GetCurrentSaveSlot());
            return;

            bool CanLoad()
            {
                if (!DirectoryManagement.CheckDirectory(GetCurrentSaveSlot())) return false;
                if (!FileManagement.CheckFile(m_foo.GetPath(GetCurrentSaveSlot()))) return false;
                return true;
            }
        }

        public void SaveAllData()
		{
            SaveHero();
			SaveA();
		}

        public void LoadAllData()
		{
			LoadHero();
			LoadA();
		}
	}

	[System.Serializable]
	public class Foo
	{
		public string a;
		public int b;
		public float c;
		public bool d;
		public char e;
	}

	[System.Serializable]
	public class Hero
	{
		public string name;
		public int id;
		public List<int> powersIds = new();
		public float xp;

		public Attribute speed = new();
		public Attribute health = new();
		public Attribute power = new();
	}

	[System.Serializable]
	public class Attribute
	{
		public int attributeIndex;
		public int attributeLevel;
	}
}