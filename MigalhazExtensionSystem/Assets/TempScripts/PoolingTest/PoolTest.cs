using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MigalhaSystem.Pool;
using MigalhaSystem.Extensions;

public class PoolTest : MonoBehaviour, IPoolable
{
    [SerializeField] PoolData m_poolData;
    [SerializeField] Timer m_timer;
    
    void Update()
    {
        m_timer.TimerElapse(Time.deltaTime);
        transform.Translate(Time.deltaTime * 10f * Vector3.right);
    }
    public void PushPool()
    {
        PoolManager.Instance.PushObject(m_poolData, gameObject);
    }

    public void OnPull()
    {
        Debug.Log("Pull Test 1".Color(Color.red));
        m_timer.SetupTimer();
        transform.position = Vector3.zero;
    }
    
    public void OnPush()
    {
        Debug.Log("Push Test 1".Color(Color.red));
    }
}
