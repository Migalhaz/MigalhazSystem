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
        [SerializeField] protected IntRange m_hpRange;
        [SerializeField] protected float m_currentHp;
        [SerializeField] protected UnityEvent OnHpMin;
        [SerializeField] protected UnityEvent OnHpMax;
        [SerializeField] protected UnityEvent OnHpChange;
    }

    interface IDamage
    {
        public UnityEvent OnTakeDamage { get; protected set; }
        public void Damage(float damage);
    }

    interface IHeal
    {
        public UnityEvent OnHeal { get; protected set; }
        public void Heal(float heal);
    }

    interface IDie
    {
        public UnityEvent OnDie { get; protected set; }
        public void Death();
    }
}