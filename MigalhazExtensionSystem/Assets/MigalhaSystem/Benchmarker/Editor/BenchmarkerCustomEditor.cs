using UnityEngine;
using UnityEditor;

namespace MigalhaSystem.Analysis
{
	[CustomEditor(typeof(Benchmarker))]
	public class BenchmarkerCustomEditor : Editor
	{
        public override void OnInspectorGUI()
        {
            Benchmarker benchmarker = (target as Benchmarker);
            SerializedProperty iterationsProperty = serializedObject.FindProperty("m_Iteration");
            SerializedProperty actionListProperty = serializedObject.FindProperty("m_Actions");
            
            GUILayoutOption[] buttonLayout = new[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(75),
                GUILayout.MinHeight(35)
            };
            

            GUILayout.Label("Iterations");
            iterationsProperty.intValue = EditorGUILayout.IntSlider(iterationsProperty.intValue, 1000, 1_000_000);
            EditorGUILayout.PropertyField(actionListProperty, true);

            bool iterationsPressed = GUILayout.Button("Execute", buttonLayout);

            
            if (iterationsPressed)
            {
                benchmarker?.Execute();
            }

            
            
            GUI.backgroundColor = benchmarker.m_GenerateLog ? Color.green : Color.red;
            bool generateLog = GUILayout.Button("Generate log file", buttonLayout);
            if (generateLog)
            {
                benchmarker.m_GenerateLog = !benchmarker.m_GenerateLog;
            }
            GUI.backgroundColor = Color.white;
            GenerateLog();

            void GenerateLog()
            {
                if (!benchmarker.m_GenerateLog) return;

                GUILayout.Space(10);
                GUIStyle headStyle = new GUIStyle();
                headStyle.fontSize = 20;
                headStyle.alignment = TextAnchor.MiddleCenter;
                headStyle.normal.textColor = Color.green;
                GUILayout.Label("File Settings", headStyle);
                GUILayout.Space(10);

                SerializedProperty pathProperty = serializedObject.FindProperty("m_LogPath");
                pathProperty.stringValue = EditorGUILayout.TextField("Log file path:", pathProperty.stringValue);

                SerializedProperty logTextFileNameProperty = serializedObject.FindProperty("m_LogTextFileName");
                logTextFileNameProperty.stringValue = EditorGUILayout.TextField("Log text file name:", logTextFileNameProperty.stringValue);

                LogSettings();
                if (GUILayout.Button("Delete Log", buttonLayout))
                {
                    benchmarker.DeleteLog();
                }
                GUI.backgroundColor = benchmarker.m_OpenFileOnWrite ? Color.green : Color.red;
                bool openFileOnLog = GUILayout.Button("Open file on log", buttonLayout);
                if (openFileOnLog)
                {
                    benchmarker.m_OpenFileOnWrite = !benchmarker.m_OpenFileOnWrite;
                }
                GUI.backgroundColor = Color.white;
                
            }

            void LogSettings()
            {
                EditorGUILayout.BeginHorizontal();

                SerializedProperty logTimeProperty = serializedObject.FindProperty("m_LogTime");
                SerializedProperty logObjNameProperty = serializedObject.FindProperty("m_LogGameObjectName");
                SerializedProperty logIterationCountProperty = serializedObject.FindProperty("m_LogIterationsCount");
                SerializedProperty logMessageProperty = serializedObject.FindProperty("m_LogMessage");

                GUI.backgroundColor = logTimeProperty.boolValue ? Color.green : Color.red;
                if (GUILayout.Button("Log Time"))
                {
                    logTimeProperty.boolValue = !logTimeProperty.boolValue;
                }
                GUI.backgroundColor = Color.white;

                GUI.backgroundColor = logObjNameProperty.boolValue ? Color.green : Color.red;
                if (GUILayout.Button("Log Game Object Name"))
                {
                    logObjNameProperty.boolValue = !logObjNameProperty.boolValue;
                }
                GUI.backgroundColor = Color.white;

                GUI.backgroundColor = logIterationCountProperty.boolValue ? Color.green : Color.red;
                if (GUILayout.Button("Log Iterations Count"))
                {
                    logIterationCountProperty.boolValue = !logIterationCountProperty.boolValue;
                }
                GUI.backgroundColor = Color.white;

                GUI.backgroundColor = logMessageProperty.boolValue ? Color.green : Color.red;
                if (GUILayout.Button("Log Message"))
                {
                    logMessageProperty.boolValue = !logMessageProperty.boolValue;
                }
                GUI.backgroundColor = Color.white;
                SerializedProperty logMessage = serializedObject.FindProperty("m_Message");
                
                EditorGUILayout.EndHorizontal();
                if (logMessageProperty.boolValue)
                {
                    logMessage.stringValue = EditorGUILayout.TextField("Log Message:", logMessage.stringValue);
                }
                else
                {
                    logMessage.stringValue = string.Empty;
                }
            }


            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(this);
        }
    }
}