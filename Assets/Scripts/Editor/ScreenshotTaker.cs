#if UNITY_EDITOR
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace VirtualEyeClinic.Editor
{
    public static class ScreenshotTaker
    {
        public static void CaptureScreenshots()
        {
            Debug.Log("[ScreenshotTaker] Starting screenshot process...");
            EditorApplication.update += Update;
            EditorApplication.isPlaying = true;
        }

        private static float waitTime = 3f;
        private static float timer = 0f;
        private static int state = 0;

        private static void Update()
        {
            if (!EditorApplication.isPlaying) return;

            timer += Time.deltaTime;

            if (state == 0 && timer > waitTime)
            {
                Debug.Log("[ScreenshotTaker] Capturing Main Menu...");
                ScreenCapture.CaptureScreenshot("screenshot_main_menu.png");
                state = 1;
                timer = 0f;
                // Switch to next scene by clicking button or directly loading
                SceneManager.LoadScene("ClinicScene");
            }
            else if (state == 1 && timer > waitTime)
            {
                Debug.Log("[ScreenshotTaker] Capturing Clinic Scene...");
                ScreenCapture.CaptureScreenshot("screenshot_clinic_scene.png");
                state = 2;
                timer = 0f;
            }
            else if (state == 2 && timer > waitTime)
            {
                Debug.Log("[ScreenshotTaker] Done capturing.");
                EditorApplication.isPlaying = false;
                EditorApplication.Exit(0);
            }
        }
    }
}
#endif
