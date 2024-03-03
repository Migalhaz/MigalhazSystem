using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MigalhaSystem.LifeSystem
{
    public abstract class CharacterStatus : GenericStatus, IHeal, IDie
    {
        [SerializeField] UnityEvent m_OnTakeDamage;
        [SerializeField] UnityEvent m_OnHeal;
        [SerializeField] UnityEvent m_OnDie;
        public virtual void Damage(float damage)
        {
            if (!IsAlive()) return;
            m_CurrentHp -= damage;
            m_OnTakeDamage?.Invoke();
            if (!IsAlive())
            {
                Death();
            }
        }

        public virtual void Death()
        {
            m_OnDie?.Invoke();
        }

        public void Heal(float heal)
        {
            if (!IsAlive()) return;
            m_CurrentHp += heal;
            m_OnHeal?.Invoke();
            if (m_CurrentHp >= m_lifeData.m_MaxHp)
            {
                m_CurrentHp = m_lifeData.m_MaxHp;
            }
        }
    }
}