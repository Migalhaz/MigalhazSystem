using UnityEngine;

namespace MigalhaSystem
{
	[CreateAssetMenu(fileName = "NewSeason", menuName = "Scriptable Object/Time System/SeasonData", order = 4)]
	public class SeasonData : ScriptableObject
	{
		[SerializeField] string m_seasonName;
		public string m_SeasonName => m_seasonName;
	}
}