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
		[SerializeField] bool m_validScene;

		public int buildIndex => m_buildIndex;
		public string path => m_path;
		public string name => m_name;
		public bool validScene => m_validScene;

		public static implicit operator int(SceneReference sceneReference) => sceneReference.m_buildIndex;
		public static implicit operator string(SceneReference sceneReference) => sceneReference.m_name;
    }
}