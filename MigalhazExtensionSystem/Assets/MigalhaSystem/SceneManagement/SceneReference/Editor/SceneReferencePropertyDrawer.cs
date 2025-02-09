using UnityEngine;
using UnityEditor;

namespace MigalhaSystem.SceneManagement
{
    [CustomPropertyDrawer(typeof(SceneReference))]
    public class SceneReferencePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty assetScene = property.FindPropertyRelative("m_scene");
            SerializedProperty scenePath = property.FindPropertyRelative("m_path");
            SerializedProperty sceneName = property.FindPropertyRelative("m_name");
            SerializedProperty sceneBuildIndex = property.FindPropertyRelative("m_buildIndex");
            SerializedProperty sceneGuid = property.FindPropertyRelative("m_guid");

            assetScene.objectReferenceValue = EditorGUI.ObjectField(position, label, assetScene.objectReferenceValue, typeof(SceneAsset), false);

            if (assetScene.objectReferenceValue == null)
            {
                InvalidScene();
                return;
            }
            if (!(assetScene.objectReferenceValue is SceneAsset))
            {
                InvalidScene();
                return;
            }

            scenePath.stringValue = AssetDatabase.GetAssetPath(assetScene.objectReferenceValue);
            sceneName.stringValue = assetScene.objectReferenceValue.name;
            sceneGuid.stringValue = AssetDatabase.GUIDFromAssetPath(scenePath.stringValue).ToString();

            sceneBuildIndex.intValue = -1;
            GUID assetGuid = new GUID(sceneGuid.stringValue);
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                if (EditorBuildSettings.scenes[i].guid == assetGuid)
                {
                    sceneBuildIndex.intValue = i;
                    break;
                }
            }

            void InvalidScene()
            {
                assetScene.objectReferenceValue = null;
                scenePath.stringValue = string.Empty;
                sceneName.stringValue = string.Empty;
                sceneGuid.stringValue = string.Empty;
                sceneBuildIndex.intValue = -1;
            }
        }
    }
}