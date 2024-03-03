using MigalhaSystem.Extensions;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MigalhaSystem.BuildValidate
{
    public class BuildCheckValidate : IProcessSceneWithReport
    {
        public int callbackOrder => 0;

        public void OnProcessScene(Scene scene, BuildReport report)
        {
            if (!BuildPipeline.isBuildingPlayer) return;
            if (!BuildValidate(scene)) throw new BuildFailedException($"The scene {scene.name} has failed validation");
        }

        static bool BuildValidate(Scene scene)
        {
            bool validBuild = true;
            foreach (GameObject obj in scene.GetRootGameObjects())
            {
                IBuildValidate[] validateChecks = obj.GetComponentsInChildren<IBuildValidate>(true);
                if (validateChecks.IsNullOrEmpty()) continue;
                foreach (IBuildValidate validateCheck in validateChecks)
                {
                    if (!validateCheck.BuildValidate()) validBuild = false;
                }
            }
            return validBuild;
        }
    }
}