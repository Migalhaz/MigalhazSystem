using MigalhaSystem.Analysis;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MigalhaSystem.SaveSystem.Example
{
    [CustomEditor(typeof(SaveControllerExample))]
    public class SaveControllerExampleEditor : Editor
    {
        bool m_openDirectoryOnSave = false;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            GUILayoutOption[] buttonLayout = new[]
            {
                GUILayout.ExpandWidth(true),
                GUILayout.MinWidth(75),
                GUILayout.MinHeight(35)
            };

            SaveControllerExample example = target as SaveControllerExample;

            bool openPersistentDataPathDirectory = GUILayout.Button("Open Directory", buttonLayout);

            GUI.backgroundColor = m_openDirectoryOnSave ? Color.green : Color.red;
            bool openDirectoryOnSave = GUILayout.Button("Open Directory on Save", buttonLayout);
            GUI.backgroundColor = Color.white;

            if (openDirectoryOnSave)
            {
                m_openDirectoryOnSave = !m_openDirectoryOnSave;
            }

            bool save = GUILayout.Button("Save", buttonLayout);
            bool load = GUILayout.Button("Load", buttonLayout);
            bool delete = GUILayout.Button("Delete Directory", buttonLayout);

            if (openPersistentDataPathDirectory)
            {
                OpenDirectory();
            }

            if (delete)
            {
                DirectoryManagement.DeleteDirectory(example.GetCurrentSaveSlot());
            }

            void OpenDirectory()
            {
                System.Diagnostics.Process.Start(DirectoryManagement.GetPersistentDataPath());
            }

            if (!example) return;

            if (save)
            {
                example.SaveAllData();
                if (m_openDirectoryOnSave)
                {
                    OpenDirectory();
                }
            }

            if (load) example.LoadAllData();
        }
    }
}
