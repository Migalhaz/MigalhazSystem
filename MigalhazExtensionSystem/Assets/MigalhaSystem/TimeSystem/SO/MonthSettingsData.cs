using UnityEngine;

namespace MigalhaSystem.TimeSystem
{
	[CreateAssetMenu(fileName = "NewMonthSettings", menuName = "Scriptable Object/Time System/Month Settings", order = 3)]
	public class MonthSettingsData : ScriptableObject
	{
        [Label("Month Settings")]
        [SerializeField] string m_monthName;
		[SerializeField, Min(0)] int m_dayCount;
		[SerializeField] SeasonData m_season;

		public string m_MonthName => m_monthName;
		public int m_DayCount => m_dayCount;
		public SeasonData m_Season => m_season;
	}
}