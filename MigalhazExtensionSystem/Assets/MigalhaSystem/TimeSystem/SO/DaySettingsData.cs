using UnityEngine;

namespace MigalhaSystem.TimeSystem
{
	[CreateAssetMenu(fileName = "NewDaySettingsData", menuName = "Scriptable Object/Time System/Day Settings", order = 2)]
	public class DaySettingsData : ScriptableObject
	{
        [Label("Day Settings", m_ToolTip = "How much last the game's day as normal time")]
        [SerializeField, Min(0)] int m_seconds;
        [SerializeField, Min(0)] int m_minutes;
        [SerializeField, Min(0)] int m_hours;

        public int m_Seconds => m_seconds;
        public int m_Minutes => m_minutes;
        public int m_Hours => m_hours;

        public int GetFullTimeInSeconds()
        {
            int total = m_Seconds + (m_Minutes * 60) + (m_Hours * 3600);
            return total;
        }
    }
}