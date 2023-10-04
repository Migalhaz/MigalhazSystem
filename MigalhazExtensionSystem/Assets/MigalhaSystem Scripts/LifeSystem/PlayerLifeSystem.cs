using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MigalhaSystem.LifeSystem
{
    using Extensions;
    public class PlayerLifeSystem : GenericLifeSystem, IHeal, IDie
    {
        [SerializeField, AutoGetComponent(typeof(Rigidbody2D))] Rigidbody2D m_rig;
        public Rigidbody2D m_Rig => this.CacheGetComponent(ref m_rig);


        protected override void Awake()
        {
            base.Awake();
            Debug.Log($"{m_rig is null}".Color(Color.red));
            Debug.Log($"{m_Rig is null}".Color(Color.green));
            Debug.Log($"{m_rig is null}".Color(Color.blue));
        }

        public void Damage(float damage)
        {
            throw new System.NotImplementedException();
        }

        public void Death()
        {
            throw new System.NotImplementedException();
        }

        public void Heal(float heal)
        {
            throw new System.NotImplementedException();
        }
        
    }
}