using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MigalhaSystem.Extensions;

namespace MigalhaSystem.LifeSystem
{
    [System.Serializable]
    public abstract class GenericLifeSystem : MonoBehaviour
    {
        [Header("Life System")]
        [SerializeField] bool m_alive;
        [SerializeField] protected float m_startHp;
        [SerializeField] protected IntRange m_hpRange;
        [SerializeField] protected float m_currentHp;
        [SerializeField] protected UnityEvent OnHpMin;
        [SerializeField] protected UnityEvent OnHpMax;
        [SerializeField] protected UnityEvent OnHpChange;
        protected virtual void Awake()
        {
            m_currentHp = m_startHp;
        }

        public virtual bool IsAlive()
        {
            if (m_currentHp <= m_hpRange.m_MinValue)
            {
                m_alive = false;
            }
            return m_alive;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (m_startHp >= m_hpRange.m_MaxValue)
            {
                m_startHp = m_hpRange.m_MaxValue;
            }
            if (m_startHp <= m_hpRange.m_MinValue)
            {
                m_startHp = m_hpRange.m_MinValue;
            }
        }
#endif
    }


    interface IDamage
    {
        public void Damage(float damage);
    }

    interface IHeal
    {
        public void Heal(float heal);
    }

    interface IDie : IDamage
    {
        public void Death();
    }
}