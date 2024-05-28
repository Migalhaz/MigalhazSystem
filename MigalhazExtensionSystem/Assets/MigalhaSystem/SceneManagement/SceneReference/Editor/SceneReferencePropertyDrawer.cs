using UnityEngine;
using UnityEditor;

namespace MigalhaSystem.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var assetScene = property.FindPropertyRelative("m_scene");
            assetScene.objectReferenceValue = EditorGUI.ObjectField(position, label, assetScene.objectReferenceValue, typeof(SceneAsset), false);

            var scenePath = property.FindPropertyRelative("m_path");
            var sceneName = property.FindPropertyRelative("m_name");
            var sceneBuildIndex = property.FindPropertyRelative("m_buildIndex");
            var validScene = property.FindPropertyRelative("m_validScene");

            if (assetScene != null && assetScene.objectReferenceValue is SceneAsset)
            {

                scenePath.stringValue = AssetDatabase.GetAssetPath(assetScene.objectReferenceValue);
                sceneName.stringValue = assetScene.objectReferenceValue.name;
                GUID assetGuid = AssetDatabase.GUIDFromAssetPath(scenePath.stringValue);

                for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
                {
                    if (EditorBuildSettings.scenes[i].guid != assetGuid)
                    {
                        sceneBuildIndex.intValue = -1;
                        continue;
                    }
                    sceneBuildIndex.intValue = i;
                    break;
                }
                validScene.boolValue = true;
            }
            else
            {
                scenePath.stringValue = string.Empty;
                sceneName.stringValue = string.Empty;
                sceneBuildIndex.intValue = -1;
                validScene.boolValue = false;
            }
        }
    }
}