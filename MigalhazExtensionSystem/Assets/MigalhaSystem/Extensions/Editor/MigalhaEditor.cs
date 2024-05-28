using System.Reflection;
using UnityEditor;
using UnityEngine;
using MigalhaSystem.Extensions;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.ShortcutManagement;
using UnityEditorInternal;
using System.Linq;


namespace MigalhaSystem.MigalhaEditor 
{
    public static class MigalhaEditor
    {
        public const string m_MENUFOLDER = "Migalha Tools/";
        public const string m_MIGALHASYSTEMFOLDERPATH = "Assets/MigalhaSystem";
    }

    public class LayoutTool
    {
        const string m_LAYOUTTOOLMENUFOLDER = MigalhaEditor.m_MENUFOLDER + "Layout/";

        [MenuItem(m_LAYOUTTOOLMENUFOLDER + "Lock Current Window _F1")]
        public static void ToggleLockCurrentWindow()
        {
            EditorWindow inspectorToBeLocked = EditorWindow.focusedWindow;

            System.Type projectBrowserType = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.ProjectBrowser");

            System.Type inspectorWindowType = Assembly.GetAssembly(typeof(Editor)).GetType("UnityEditor.InspectorWindow");

            PropertyInfo propertyInfo;
            if (inspectorToBeLocked.GetType() == projectBrowserType)
            {
                propertyInfo = projectBrowserType.GetProperty("isLocked", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            }
            else if (inspectorToBeLocked.GetType() == inspectorWindowType)
            {
                propertyInfo = inspectorWindowType.GetProperty("isLocked");
            }
            else
            {
                return;
            }

            bool value = (bool)propertyInfo.GetValue(inspectorToBeLocked, null);
            propertyInfo.SetValue(inspectorToBeLocked, !value, null);
            inspectorToBeLocked.Repaint();
        }

        [MenuItem(m_LAYOUTTOOLMENUFOLDER + "Switch Play Mode _F2")]
        public static void SwitchPlayMode()
        {
            EditorApplication.isPlaying = !EditorApplication.isPlaying;
        }

        [MenuItem(m_LAYOUTTOOLMENUFOLDER + "Close Current Window _F4")]
        public static void CloseCurrentTab()
        {
            EditorWindow.focusedWindow.Close();
        }

        [MenuItem(m_LAYOUTTOOLMENUFOLDER + "Maximize Current Tab _F11")]
        public static void MaximizeCurrentTab()
        {
            EditorWindow.focusedWindow.maximized = !EditorWindow.focusedWindow.maximized;
        }
    }
    public static class ActionTool
    {
        public const string m_ACTIONTOOLMENUFOLDER = MigalhaEditor.m_MENUFOLDER + "Actions/";

        [MenuItem(m_ACTIONTOOLMENUFOLDER + "Notify")]
        public static void Notify()
        {
            Notify("Default Notification");
        }

        public static void Notify(string message, float fadeDuration = 2f, string toolTip = null, Texture image = null)
        {
            GUIContent content = new GUIContent(message);
            content.tooltip = string.IsNullOrEmpty(toolTip) ? content.tooltip : toolTip;
            content.image = (image is null)? content.image : image;

            SceneView.lastActiveSceneView.ShowNotification(content, fadeDuration);
        }
        [MenuItem(m_ACTIONTOOLMENUFOLDER + "Capture Screenshot _F12")]
        public static void ScreenshotToolMenuItem()
        {
            string path = ScreenshotDefaultPath();
            if (!System.IO.Directory.Exists(path)) System.IO.Directory.CreateDirectory(path);
            string filePath = System.IO.Path.Combine(path, $"{Application.productName}_{System.DateTime.Now.ToString("yyyymmddhhmmss")}.png");
            ScreenCapture.CaptureScreenshot(filePath, 1);
        }

        public static string ScreenshotDefaultPath()
        {
            string defaultPath = System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "Screenshots");
            return defaultPath;
        }
    }

    public class ComponentTool
    {
        const string m_CONTEXTPATH = "CONTEXT/Component/";
        
        const string m_MOVETOTOPPATH = m_CONTEXTPATH + "Move to top";

        const string m_MOVETOBOTTOMPATH = m_CONTEXTPATH + "Move to bottom";
        const string m_CUTPATH = m_CONTEXTPATH + "Cut";
        const string m_REMOVEMONOBEHAVIOURWITHCALLBACK = m_CONTEXTPATH + "Remove with Callback";

        [MenuItem(m_MOVETOTOPPATH, priority = 503)]
        public static void MoveComponentToTop(MenuCommand command)
        {
            while (UnityEditorInternal.ComponentUtility.MoveComponentUp(command.context as Component)) ;
        }
        [MenuItem(m_MOVETOTOPPATH, validate = true)]
        public static bool MoveComponentToTopValidate(MenuCommand command)
        {
            Component[] components = ((Component)command.context).gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if ((Component)command.context is Transform) return false;
                if (components[i] == (Component) command.context) return i != 1;
            }
            return true;
        }


        [MenuItem(m_MOVETOBOTTOMPATH, priority = 504)]
        public static void MoveComponentToBottom(MenuCommand command)
        {
            while (UnityEditorInternal.ComponentUtility.MoveComponentDown(command.context as Component)) ;
        }

        [MenuItem(m_MOVETOBOTTOMPATH, validate = true)]
        public static bool MoveComponentToBottomValidate(MenuCommand command)
        {
            Component[] components = ((Component)command.context).gameObject.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                if ((Component)command.context is Transform) return false;
                if (components[i] == (Component)command.context) return i != (components.Length - 1);
            }
            return true;
        }

        [MenuItem(m_CUTPATH, priority = 501)]
        public static void CutComponent(MenuCommand command)
        {
            Component component = (Component)command.context;
            if (component is Transform) return;
            UnityEditorInternal.ComponentUtility.CopyComponent(component);
            Undo.DestroyObjectImmediate(component);
        }

        [MenuItem(m_CUTPATH, validate = true)]
        public static bool CutComponentValidate(MenuCommand command)
        {   
            Component component = (Component)command.context;
            if (!component.gameObject.CanDestroy(component.GetType())) return false;
            return !(component is Transform);
        }

        [MenuItem(m_REMOVEMONOBEHAVIOURWITHCALLBACK, priority = 502)]
        public static void RemoveWithCallback(MenuCommand command)
        {
            Component component = (Component)command.context;
            if (component is Transform) return;
            if (component.TryGetComponent(out IOnRemoveComponent callback))
            {
                callback.OnRemoveComponent();
            }
            Undo.DestroyObjectImmediate(component);
        }

        [MenuItem(m_REMOVEMONOBEHAVIOURWITHCALLBACK, validate = true)]
        public static bool RemoveWithCallbackValidate(MenuCommand command)
        {
            Component component = (Component)command.context;
            if (!component.gameObject.CanDestroy(component.GetType())) return false;
            return !(component is Transform);
        }
    }

    public class MissingScriptTool
    {
        const string m_MISSINGSCRIPTTOOLMENUFOLDER = ActionTool.m_ACTIONTOOLMENUFOLDER + "Missing Scripts/";

        [MenuItem(m_MISSINGSCRIPTTOOLMENUFOLDER + "Find")]
        public static void FindMissingScripts()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>(true))
            {
                foreach (Component c in obj.GetComponentsInChildren<Component>())
                {
                    if (c != null) continue;
                    Debug.LogWarning($"GameObject found with missing script {obj.name}", obj);
                    break;
                }
            }
        }

        [MenuItem(m_MISSINGSCRIPTTOOLMENUFOLDER + "Delete")]
        public static void DeleteMissingScripts()
        {
            foreach (GameObject obj in GameObject.FindObjectsOfType<GameObject>(true))
            {
                foreach (Component c in obj.GetComponentsInChildren<Component>())
                {
                    if (c != null) continue;
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(obj);
                    break;
                }
            }
        }
    }
    public class SceneTool
    {
        const string m_SCENETOOLMENUFOLDER = MigalhaEditor.m_MENUFOLDER + "Scene Tool/";

        [MenuItem(m_SCENETOOLMENUFOLDER + "Reload Scene _F5")]
        public static void ReloadScene()
        {
            if (EditorApplication.isPlaying)
            {
                string sceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneName);
            }
        }

        [MenuItem(m_SCENETOOLMENUFOLDER + "Add Current Scene To Build Settings _F7")]
        public static void AddSceneToBuildSettings()
        {
            string currentScenePath = SceneManager.GetActiveScene().path;
            List<EditorBuildSettingsScene> buildSettingsScenes = EditorBuildSettings.scenes.ToList();

            if (buildSettingsScenes.Exists(x => x.path == currentScenePath)) return;

            EditorBuildSettingsScene newScene = new()
            {
                enabled = true,
                path = currentScenePath
            };
            buildSettingsScenes.Add(newScene);
            EditorBuildSettings.scenes = buildSettingsScenes.ToArray();
        }

        [MenuItem(m_SCENETOOLMENUFOLDER + "Remove Current Scene From Build Settings _%F7")]
        public static void RemoveSceneToBuildSettings()
        {
            string currentScenePath = SceneManager.GetActiveScene().path;
            List<EditorBuildSettingsScene> buildSettingsScenes = EditorBuildSettings.scenes.ToList();

            if (!buildSettingsScenes.Exists(x => x.path == currentScenePath)) return;

            EditorBuildSettingsScene sceneToRemove = buildSettingsScenes.Find(x => x.path == currentScenePath);

            buildSettingsScenes.Remove(sceneToRemove);
            EditorBuildSettings.scenes = buildSettingsScenes.ToArray();
        }
    }

    public static class CreateScriptTemplates
    {
        const string m_TEMPLATESFOLDERPATH = MigalhaEditor.m_MIGALHASYSTEMFOLDERPATH + "/Extensions/Editor/Templates";
        const string m_MENUITEMPATH = "Assets/Create/Code/";
        
        [MenuItem(m_MENUITEMPATH + "MonoBehaviour", priority = 40)]
        public static void CreateMonoBehaviourMenuItem()
        {
            string templatePath = m_TEMPLATESFOLDERPATH + "/MonoBehaviour.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScript.cs");
        }

        [MenuItem(m_MENUITEMPATH + "ScriptableObject", priority = 41)]
        public static void CreateScriptableObjectMenuItem()
        {
            string templatePath = m_TEMPLATESFOLDERPATH + "/ScriptableObject.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewScriptableObject.cs");
        }

        [MenuItem(m_MENUITEMPATH + "Custom Editor", priority = 42)]
        public static void CreateCustomEditorMenuItem()
        {
            string templatePath = m_TEMPLATESFOLDERPATH + "/Editor.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewCustomEditor.cs");
        }

        [MenuItem(m_MENUITEMPATH + "Empty Class", priority = 43)]
        public static void CreateEmptyClassMenuItem()
        {
            string templatePath = m_TEMPLATESFOLDERPATH + "/EmptyClass.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewClass.cs");
        }

        [MenuItem(m_MENUITEMPATH + "Enum", priority = 44)]
        public static void CreateEnumMenuItem()
        {
            string templatePath = m_TEMPLATESFOLDERPATH + "/Enum.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewEnum.cs");
        }

        [MenuItem(m_MENUITEMPATH + "Interface", priority = 45)]
        public static void CreateInterfaceMenuItem()
        {
            string templatePath = m_TEMPLATESFOLDERPATH + "/Interface.cs.txt";
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NewInterface.cs");
        }
    }



    public abstract class CustomGameObjectCreation
    {
        protected const string m_MENUITEMPATH = "GameObject/Custom Game Object/";
        public static void CreateCustomGameObjectCreation<T>() where T : CustomGameObjectCreation, new()
        {
            T creator = new();
            string defaultLayer = creator.GetGameObjectDefaultLayer();
            string defaultGameObjectName = creator.GetGameObjectDefaultName();

            GameObject newGameObject = new GameObject(defaultGameObjectName);
            newGameObject.layer = LayerMask.NameToLayer(defaultLayer);

            if (Selection.activeGameObject)
            {
                newGameObject.transform.SetParent(Selection.activeGameObject.transform);
                newGameObject.transform.ResetLocalTransformation();
            }

            Selection.activeGameObject = newGameObject;

            foreach (System.Type type in creator.ComponentsToAdd())
            {
                newGameObject.AddComponent(type);
            }

            EditorApplication.delayCall += () =>
            {
                System.Type sceneHierarchyType = System.Type.GetType("UnityEditor.SceneHierarchyWindow,UnityEditor");
                EditorWindow hierarchyWindow = EditorWindow.GetWindow(sceneHierarchyType);
                hierarchyWindow.SendEvent(EditorGUIUtility.CommandEvent("Rename"));
            };
        }

        public virtual string GetGameObjectDefaultName() => "Game Object";
        public virtual string GetGameObjectDefaultLayer() => "Default";
        public abstract List<System.Type> ComponentsToAdd();
    }

    public class RigidBodyCustomObj : CustomGameObjectCreation
    {

        [MenuItem(m_MENUITEMPATH + "Physic object")]
        public static void CreateCustomGameObjectCreation()
        {
            CreateCustomGameObjectCreation<RigidBodyCustomObj>();
        }

        public override string GetGameObjectDefaultName() => "Physics Object";

        public override List<System.Type> ComponentsToAdd()
        {
            bool dimension = EditorUtility.DisplayDialog("Dimension", "Rigidbody dimension", "3D", "2D");

            System.Type GetCollider()
            {
                bool ColliderMesh = EditorUtility.DisplayDialog("Collider Mesh", "Collider Mesh", "Circle", "Box");
                if (dimension) return ColliderMesh ? typeof(SphereCollider) : typeof(BoxCollider);
                else return ColliderMesh ? typeof(CircleCollider2D) : typeof(BoxCollider2D);
            }

            return new List<System.Type>()
            {
                dimension ? typeof(Rigidbody) : typeof(Rigidbody2D),
                GetCollider()
            };
        }
    }


    public class QuickSelectionWindow : EditorWindow
    {
        GUIContent[] m_content;
        int m_selectionIndex = 0;
        static int m_currentLayout;
        bool m_selected;
        GUIStyle m_style;

        [Shortcut("QuickSelectionWindow/Show", KeyCode.G, ShortcutModifiers.Alt)]
        static void ShowPopUpWindow()
        {
            QuickSelectionWindow dialog = CreateWindow<QuickSelectionWindow>();
            dialog.titleContent = new GUIContent("Layout Selection");

            Vector2 screenSize = new Vector2(Screen.currentResolution.width, Screen.currentResolution.height);

            int layoutCount = GetAllLayouts().Length;
            int yCells = layoutCount % 2 == 0 ? layoutCount / 2 : (layoutCount + 1) / 2;
            Vector2 cellCount = new Vector2(2, yCells);
            Vector2 cellSize = new Vector2(256, 32);

            Vector2 dialogSize = new Vector2(cellCount.x * cellSize.x, (cellCount.y * cellSize.y) + 10f);
            dialog.position = new Rect((screenSize - dialogSize) * .5f, dialogSize);
            
            dialog.ShowModalUtility();
        }

        private void OnGUI()
        {
            if (Event.current != null && Event.current.modifiers == EventModifiers.Alt)
            {
                if (Event.current.keyCode == KeyCode.G && !m_selected)
                {
                    m_selected = true;
                    m_selectionIndex++;
                    if (m_selectionIndex >= m_content.Length) m_selectionIndex = 0;
                    Repaint();
                }

                if (Event.current.type == EventType.KeyUp)
                {
                    m_selected = false;
                }
            }
            else
            {
                SelectContent(m_selectionIndex);
            }

            if (m_style == null)
            {
                m_style = new(GUI.skin.button);
                m_style.fixedHeight = 32;
                m_style.fixedWidth = 256;
                m_style.fontSize = 14;
            }
            int i = GUILayout.SelectionGrid(m_selectionIndex, m_content, 2, m_style);
            if (i != m_selectionIndex) SelectContent(i);
        }

        void SelectContent(int index)
        {
            index = Mathf.Clamp(index, 0, m_content.Length - 1);
            Close();
            m_currentLayout = index;
            OpenLayout(m_content[index].text);
            GUIUtility.ExitGUI();
        }

        private void OnEnable()
        {
            int contentCount = GetAllLayouts().Length;
            m_content = new GUIContent[contentCount];
            m_selectionIndex = m_currentLayout - 1;


            for (int i = 0; i < contentCount; i++)
            {
                GUIContent newContent = new($"{System.IO.Path.GetFileNameWithoutExtension(GetAllLayouts()[i])}");
                m_content[i] = newContent;
            }

        }

        static bool OpenLayout(string name)
        {
            string path = GetWindowLayoutPath(name);
            if (string.IsNullOrEmpty(path)) return false;

            System.Type windowLayoutType = typeof(Editor).Assembly.GetType("UnityEditor.WindowLayout");
            if (windowLayoutType == null) return false;
            
            MethodInfo tryLoadLayoutMethod = windowLayoutType.GetMethod("LoadWindowLayout", BindingFlags.Public | BindingFlags.Static, null, new System.Type[] { typeof(string), typeof(bool)}, null);
            if (tryLoadLayoutMethod == null) return false;

            object[] args = new object[] { path, false };
            bool result = (bool)tryLoadLayoutMethod.Invoke(null, args);
            return result;
        }

        static string GetWindowLayoutPath(string name)
        {
            string[] layoutsPath = GetAllLayouts();
            if (layoutsPath == null) return null;
            foreach (string layout in layoutsPath)
            {
                if (string.Compare(name, System.IO.Path.GetFileNameWithoutExtension(layout)) == 0) return layout; 
            }
           
            return null;
        }

        static string[] GetAllLayouts()
        {
            string layoutsPreferencesPath = System.IO.Path.Combine(InternalEditorUtility.unityPreferencesFolder, "Layouts");
            string layoutsModePreferencesPath = System.IO.Path.Combine(layoutsPreferencesPath, ModeService.currentId);

            if (!System.IO.Directory.Exists(layoutsModePreferencesPath)) return null;
            return System.IO.Directory.GetFiles(layoutsModePreferencesPath).Where(path => path.EndsWith(".wlt")).ToArray();
        }
    }
}