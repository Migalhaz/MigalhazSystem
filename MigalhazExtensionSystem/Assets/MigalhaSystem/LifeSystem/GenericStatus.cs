using UnityEngine;

namespace MigalhaSystem.LifeSystem
{
    [System.Serializable]
    public abstract class GenericStatus : MonoBehaviour
    {
        [Header("Life System")]
        [SerializeField] protected LifeStatusSO m_lifeData;
        public float m_CurrentHp { get; protected set; }

        protected virtual void Awake()
        {
            m_CurrentHp = m_lifeData.m_StartHp;
        }

        public virtual bool IsAlive() => m_CurrentHp > 0;
        public virtual float GetHpPercentage() => GetCurrentHealth() / GetMaxHealth();
        public virtual float GetMaxHealth() => m_lifeData.m_MaxHp;
        public virtual float GetCurrentHealth() => m_CurrentHp;
    }


    interface IDamage
    {
        public void Damage(float damage);
    }
    interface IDie : IDamage
    {
        public void Death();
    }
    interface IHeal
    {
        public void Heal(float heal);
    }
}

