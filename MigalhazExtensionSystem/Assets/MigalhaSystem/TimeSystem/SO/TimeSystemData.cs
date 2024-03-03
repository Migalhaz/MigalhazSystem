using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.TimeSystem
{
	[CreateAssetMenu(fileName = "NewTimeSettings", menuName = "Scriptable Object/Time System/Settings", order = 1)]
	public class TimeSystemData : ScriptableObject
	{
		[Label("Day Settings")]
		[SerializeField] DaySettingsData m_daySettingsData;
		[Label("Month Settings")]
		[SerializeField] List<MonthSettingsData> m_months;

		public DaySettingsData m_DaySettingsData => m_daySettingsData;
        public List<MonthSettingsData> m_Months => m_months;
    }
}