using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AutoGetComponent))]
public class AutoGetComponentDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        MonoBehaviour targetObject = property.serializedObject.targetObject as MonoBehaviour;

        if (targetObject != null)
        {
            AutoGetComponent autoGetComponentAttribute = (AutoGetComponent)attribute;
            System.Type componentType = autoGetComponentAttribute.m_ComponentType;

            EditorGUI.PropertyField(position, property, label, true);

            if (property.objectReferenceValue == null)
            {
                Component component = targetObject.GetComponent(componentType);

                if (component != null)
                {
                    property.objectReferenceValue = component;
                    property.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        EditorGUI.EndProperty();
    }
}