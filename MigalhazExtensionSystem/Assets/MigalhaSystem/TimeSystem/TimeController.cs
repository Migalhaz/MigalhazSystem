using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MigalhaSystem.TimeSystem
{
    public class TimeController : MonoBehaviour
    {
        [SerializeField] TimeSystemData m_settings;
        [Label("Events")]
        [SerializeField] UnityEvent<SeasonData> m_onSeasonChanged;
        [SerializeField] UnityEvent m_onDayChanged;
        [SerializeField] UnityEvent<MonthSettingsData> m_onMonthChanged;
        [SerializeField] UnityEvent m_onYearChanged;
        public UnityEvent<SeasonData> m_OnSeasonChanged => m_onSeasonChanged;
        public UnityEvent m_OnDayChanged => m_onDayChanged;
        public UnityEvent<MonthSettingsData> m_OnMonthChanged => m_onMonthChanged;
        public UnityEvent m_OnYearChanged => m_onYearChanged;
        int m_monthIndex;
        int m_year;
        SeasonData m_currentSeason;
        float m_currentSeconds;
        int m_currentMonthDay;
        int m_currentYearDay;
        int m_allDays;
        private void Awake()
        {
            m_currentSeconds = 0;
            SetupDays();
            SetupMonthAndYear();
            
            void SetupDays()
            {
                m_currentMonthDay = 0;
                m_currentYearDay = 0;
                m_allDays = 0;
            }
            void SetupMonthAndYear()
            {
                m_currentSeason = null;
                m_monthIndex = 0;
                m_year = 0;
                SetSeason(GetCurrentMonth().m_Season);
            }
        }
        private void Update()
        {
            UpdateDayTime();
        }
        void UpdateDayTime()
        {
            m_currentSeconds += Time.deltaTime;
            int dayTime = GetDaySettings().GetFullTimeInSeconds();
            if (m_currentSeconds >= dayTime)
            {
                m_currentSeconds = 0;
                NextDay();
            }
        }
        void NextDay()
        {
            m_allDays++;
            m_currentMonthDay++;
            m_currentYearDay++;
            m_onDayChanged?.Invoke();
            if (m_currentMonthDay >= GetCurrentMonth().m_DayCount) NextMonth();
        }
        void NextMonth()
        {
            m_currentMonthDay = 0;
            m_monthIndex++;
            if (m_monthIndex >= m_settings.m_Months.Count)
            {
                m_monthIndex = 0;
                NextYear();
            }
            m_onMonthChanged?.Invoke(GetCurrentMonth());
            SetSeason(GetCurrentMonth().m_Season);
        }
        void NextYear()
        {
            m_year++;
            m_currentYearDay = 0;
            m_onYearChanged?.Invoke();
        }
        void SetSeason(SeasonData newSeason)
        {
            if (newSeason == null) return;
            if (m_currentSeason == newSeason) return;
            m_currentSeason = newSeason;
            m_onSeasonChanged?.Invoke(m_currentSeason);
        }

        public int GetMonthDay() => m_currentMonthDay;
        public int GetYearDay() => m_currentYearDay;
        public int GetAllDays() => m_allDays;
        public MonthSettingsData GetCurrentMonth() => GetAllMonths()[m_monthIndex];
        public List<MonthSettingsData> GetAllMonths() => m_settings.m_Months;
        public SeasonData GetCurrentSeason() => m_currentSeason;
        public int GetYear() => m_year;
        public float GetDayTimeElapsed() => m_currentSeconds;
        public float GetAllTimeElapsed() => GetDaySettings().GetFullTimeInSeconds() * GetAllDays() + GetDayTimeElapsed();
        public DaySettingsData GetDaySettings() => m_settings.m_DaySettingsData;
    }
}
