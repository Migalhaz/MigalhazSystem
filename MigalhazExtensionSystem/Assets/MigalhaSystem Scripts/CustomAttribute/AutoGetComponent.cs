using UnityEngine;

public class AutoGetComponent : PropertyAttribute
{
    public System.Type m_ComponentType { get; private set; }

    public AutoGetComponent(System.Type componentType)
    {
        m_ComponentType = componentType;
    }
}