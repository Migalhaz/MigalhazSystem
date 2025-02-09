using UnityEngine;

namespace MigalhaSystem.SceneManagement
{
	[System.Serializable]
	public class SceneReference
	{
		[SerializeField] Object m_scene;
		[SerializeField] string m_path; 
		[SerializeField] string m_name;
		[SerializeField] int m_buildIndex;
		[SerializeField] string m_guid;

		public int BuildIndex => m_buildIndex;
		public string Path => m_path;
		public string Name => m_name;
		public string Guid => m_guid;

		public bool ValidScene
		{
			get
			{
				if (m_scene == null) return false;
				if (string.IsNullOrWhiteSpace(m_path)) return false;
				if (string.IsNullOrWhiteSpace(m_name)) return false;
				if (string.IsNullOrWhiteSpace(m_guid)) return false;
				if (m_buildIndex <= -1) return false;
				return true;
			}
		}

		public static explicit operator int(SceneReference sceneReference)
		{
			if (!sceneReference.ValidScene) return -1;
			return sceneReference.BuildIndex;
		}

		public static explicit operator string(SceneReference sceneReference)
		{
            if (!sceneReference.ValidScene) return string.Empty;
            return sceneReference.Name;
        }
    }
}