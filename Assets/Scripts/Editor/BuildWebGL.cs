#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VirtualEyeClinic.Editor
{
    public static class BuildWebGL
    {
        private const string WebGLOutputPath = "Builds/WebGL";

        public static void Build()
        {
            try
            {
                Debug.Log("[BuildWebGL] Creating scenes...");
                ClinicProjectSetup.CreateScenes();

                Debug.Log("[BuildWebGL] Configuring WebGL player settings...");
                EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), WebGLOutputPath);
                Directory.CreateDirectory(outputPath);

                var options = new BuildPlayerOptions
                {
                    scenes = new[] { "Assets/Scenes/ClinicScene.unity" },
                    locationPathName = outputPath,
                    target = BuildTarget.WebGL,
                    options = BuildOptions.None
                };

                BuildReport report = BuildPipeline.BuildPlayer(options);

                if (report.summary.result != BuildResult.Succeeded)
                {
                    Debug.LogError("[BuildWebGL] Build failed: " + report.summary.result);
                    EditorApplication.Exit(1);
                    return;
                }

                Debug.Log("[BuildWebGL] Build succeeded: " + outputPath);
                EditorApplication.Exit(0);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                EditorApplication.Exit(1);
            }
        }
    }
}
#endif
