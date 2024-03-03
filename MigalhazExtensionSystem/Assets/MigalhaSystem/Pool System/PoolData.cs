using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem.Pool 
{
    [CreateAssetMenu(fileName = "New Pool Data", menuName = "Scriptable Object/Pool/New Pool")]
    public class PoolData : ScriptableObject
    {
        public GameObject m_Prefab;
        public int m_PoolSize = 1;
        public bool m_ExpandablePool = false;
        public bool m_DestroyExtraObjectsAfterUse;
        public int m_ExtraObjectsAllowed;

        public int MaxPoolSize()
        {
            if (!m_ExpandablePool)
            {
                return m_PoolSize;
            }
            return m_PoolSize + m_ExtraObjectsAllowed;
        }
    }
}