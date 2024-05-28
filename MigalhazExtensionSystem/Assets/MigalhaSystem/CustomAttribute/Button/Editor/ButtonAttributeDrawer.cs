using UnityEngine;
using UnityEditor;

namespace MigalhaSystem
{
	[CustomPropertyDrawer(typeof(ButtonAttribute))]
	public class ButtonAttributeDrawer : PropertyDrawer
	{
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {

            //Object target = property.serializedObject.targetObject;
            //string methodName = (property.serializedObject. as ButtonAttribute).m_MethodName;
            //System.Type type = target.GetType();
            //System.Reflection.MethodInfo method = type.GetMethod(methodName);
            //if (method == null)
            //{
            //    GUI.Label(position, "Method could not be found. Is it public?");
            //    return;
            //}
            //if (method.GetParameters().Length > 0)
            //{
            //    GUI.Label(position, "Method cannot have parameters.");
            //    return;
            //}
            //if (GUI.Button(position, method.Name))
            //{
            //    method.Invoke(target, null);
            //}
        }
    }
}