using UnityEngine;

namespace MigalhaSystem
{
    public class AutoGetComponentAttribute : PropertyAttribute
    {
        public System.Type m_ComponentType { get; private set; }

        public AutoGetComponentAttribute(System.Type componentType)
        {
            m_ComponentType = componentType;
        }
    }
}