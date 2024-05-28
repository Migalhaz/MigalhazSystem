using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MigalhaSystem.LifeSystem
{
    public abstract class CharacterStatus : GenericStatus, IHeal, IDie
    {
        [SerializeField] UnityEvent<GenericStatus> m_onTakeDamage;
        [SerializeField] UnityEvent<GenericStatus> m_onHeal;
        [SerializeField] UnityEvent<GenericStatus> m_onDie;
        [SerializeField] UnityEvent<GenericStatus> m_onLifeChange;

        public UnityEvent<GenericStatus> m_OnTakeDamage => m_onTakeDamage;
        public UnityEvent<GenericStatus> m_OnHeal => m_onHeal;
        public UnityEvent<GenericStatus> m_OnDie => m_onDie;
        public UnityEvent<GenericStatus> m_OnLifeChange => m_onLifeChange;

        public virtual void Damage(float damage)
        {
            if (!IsAlive()) return;
            m_CurrentHp -= damage;
            if (m_CurrentHp <= 0)
            {
                m_CurrentHp = 0;
            }
            m_onTakeDamage?.Invoke(this);
            m_onLifeChange?.Invoke(this);
            if (!IsAlive())
            {
                Death();
            }
        }

        public virtual void Death()
        {
            m_onDie?.Invoke(this);
        }

        public void Heal(float heal)
        {
            if (!IsAlive()) return;
            m_CurrentHp += heal;
            if (m_CurrentHp >= m_lifeData.m_MaxHp)
            {
                m_CurrentHp = m_lifeData.m_MaxHp;
            }
            m_onHeal?.Invoke(this);
            m_onLifeChange?.Invoke(this);
        }
    }
}