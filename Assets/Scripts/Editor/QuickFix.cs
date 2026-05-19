using UnityEditor;

namespace VirtualEyeClinic.Editor
{
    public static class QuickFix
    {
        public static void DoFix()
        {
            UnityEngine.Debug.Log("[QuickFix] Recreating scenes...");
            ClinicProjectSetup.CreateScenes();
            UnityEngine.Debug.Log("[QuickFix] Building WebGL...");
            BuildWebGL.Build();
            UnityEngine.Debug.Log("[QuickFix] Done!");
        }
    }
}
