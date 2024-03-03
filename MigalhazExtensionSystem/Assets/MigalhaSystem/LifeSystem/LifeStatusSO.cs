using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MigalhaSystem.LifeSystem
{
    [CreateAssetMenu(fileName = "NewLifeStatus", menuName = "Scriptable Object/Status/New Life Status")]
    public class LifeStatusSO : ScriptableObject
    {
        [field: SerializeField] public float m_StartHp { get; private set; }
        [field: SerializeField] public float m_MaxHp { get; private set; }
    }
}