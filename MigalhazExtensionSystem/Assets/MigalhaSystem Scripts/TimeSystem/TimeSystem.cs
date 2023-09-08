using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem;
using MigalhaSystem.Extensions;

public class TimeSystem : Singleton<TimeSystem>
{
    [SerializeField] int m_days;
    [Tooltip("Quanto tempo na vida real dura 1 dia no jogo (ex: 1 dia no Minecraft equivale a 10 minutos na vida real)"), Header("Settings")]
    [SerializeField, Min(0)] int m_hours;
    [SerializeField, Range(0, 59)] int m_minutes;
    [SerializeField, Range(0, 59)] int m_seconds;
    [SerializeField] List<DayPeriod> m_daysList;
    [SerializeField, Range(0, 100)] float m_dayPercent;

    public float m_totalTimeInSeconds => m_seconds + (m_minutes * 60) + (m_hours * 3600);
    float m_currentTime;
    protected override void Awake()
    {
        base.Awake();
        m_currentTime = 0;
        m_days = 0;
    }
    private void Update()
    {
        TimeLogic();
    }

    void TimeLogic()
    {
        m_currentTime += Time.deltaTime;
        float percent = m_currentTime / m_totalTimeInSeconds;
        m_dayPercent = percent * 100;
        if (percent >= 1)
        {
            m_currentTime = 0;
            m_days++;
        }
        DayPeriod a = m_daysList.Find(x => x.m_Period.InRange(m_dayPercent));
        Debug.Log(a.m_DayTime);

    }

    public enum DayTime
    {
        Morning, Afternoon, Evening
    }

    [System.Serializable]
    public struct DayPeriod
    {
        [SerializeField] DayTime m_dayTime;
        [SerializeField] FloatRange m_period;

        public DayTime m_DayTime => m_dayTime;
        public FloatRange m_Period => m_period;
    }
}
