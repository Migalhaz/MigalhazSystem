using UnityEngine;

namespace MigalhaSystem.SaveSystem
{
	[System.Serializable]
	public class SaveObject<T>
	{
		[SerializeField] string m_fileName;
		public T m_SaveObject;
		[SerializeField] AbstractSaveStrategy m_strategy;

		public bool Save(int saveSlot) => m_strategy.WriteData(JsonData(), GetPath(saveSlot));

        public void Load(int saveSlot)
		{
            m_SaveObject = SaveJsonConverter.FromJson<T>(Reader(saveSlot));
		}

		public string GetPath(int saveSlot) => 
			DirectoryManagement.GetPathOfSaveSlot(saveSlot) 
			+ SaveUtils.NormalizeRelativePath(m_fileName, m_strategy.GetFileExtension());

		string JsonData() => SaveJsonConverter.ToJson(m_SaveObject);
        string Reader(int saveSlot) => m_strategy.ReadData(GetPath(saveSlot));
	}
}