using MigalhaSystem.Extensions;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MigalhaSystem.SceneManagement
{
	public class SceneManagement : Singleton.Singleton<SceneManagement>
	{
		[SerializeField] SceneReference m_startScene;
		[SerializeField] SceneReference m_loadScene;
		protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
			SceneManager.LoadScene(m_startScene.buildIndex);
        }
    }
}