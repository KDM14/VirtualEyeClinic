#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace VirtualEyeClinic.Editor
{
    public static class BuildCi
    {
        private const string ApkOutputPath = "Builds/Android/VirtualEyeClinic.apk";

        [MenuItem("Virtual Eye Clinic/3. Build Android APK")]
        public static void BuildApkFromMenu()
        {
            if (!ScenesExist())
            {
                bool create = EditorUtility.DisplayDialog(
                    "Scenes Missing",
                    "Scenes are not set up yet. Create them now?",
                    "Yes",
                    "Cancel");

                if (!create)
                {
                    return;
                }

                ClinicProjectSetup.CreateScenes();
            }

            ConfigureAndroid();

            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), ApkOutputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? "Builds/Android");

            BuildReport report = RunBuild(outputPath);

            if (report.summary.result == BuildResult.Succeeded)
            {
                EditorUtility.DisplayDialog("Build Complete", "APK saved to:\n" + outputPath, "OK");
                EditorUtility.RevealInFinder(outputPath);
            }
            else
            {
                EditorUtility.DisplayDialog("Build Failed", report.summary.result.ToString(), "OK");
            }
        }

        public static void BuildAndroid()
        {
            try
            {
                Debug.Log("[BuildCi] Creating scenes and build settings...");
                ClinicProjectSetup.CreateScenes();

                Debug.Log("[BuildCi] Configuring Android player settings...");
                ConfigureAndroid();

                string outputPath = Path.Combine(Directory.GetCurrentDirectory(), ApkOutputPath);
                Directory.CreateDirectory(Path.GetDirectoryName(outputPath) ?? "Builds/Android");

                BuildReport report = RunBuild(outputPath);

                if (report.summary.result != BuildResult.Succeeded)
                {
                    Debug.LogError("[BuildCi] Build failed: " + report.summary.result);
                    EditorApplication.Exit(1);
                    return;
                }

                Debug.Log("[BuildCi] Build succeeded: " + outputPath);
                EditorApplication.Exit(0);
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
                EditorApplication.Exit(1);
            }
        }

        private static BuildReport RunBuild(string outputPath)
        {
            var options = new BuildPlayerOptions
            {
                scenes = GetEnabledScenePaths(),
                locationPathName = outputPath,
                target = BuildTarget.Android,
                options = BuildOptions.None
            };

            Debug.Log("[BuildCi] Building APK to: " + outputPath);
            return BuildPipeline.BuildPlayer(options);
        }

        private static bool ScenesExist()
        {
            return File.Exists("Assets/Scenes/MainMenu.unity") &&
                   File.Exists("Assets/Scenes/ClinicScene.unity");
        }

        private static void ConfigureAndroid()
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(
                BuildTargetGroup.Android,
                BuildTarget.Android);

            PlayerSettings.companyName = "VirtualEyeClinic";
            PlayerSettings.productName = "Virtual Eye Clinic";
            PlayerSettings.applicationIdentifier = "com.virtualeyeclinic.walkthrough";
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;

            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel24;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel33;

            EditorUserBuildSettings.buildAppBundle = false;
            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        }

        private static string[] GetEnabledScenePaths()
        {
            var scenes = EditorBuildSettings.scenes;
            int count = 0;
            foreach (var scene in scenes)
            {
                if (scene.enabled)
                {
                    count++;
                }
            }

            var paths = new string[count];
            int index = 0;
            foreach (var scene in scenes)
            {
                if (scene.enabled)
                {
                    paths[index++] = scene.path;
                }
            }

            return paths;
        }
    }
}
#endif
