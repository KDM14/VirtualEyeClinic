using System.Collections;
using UnityEngine;

namespace VirtualEyeClinic.Core
{
    public class CoroutineRunner : MonoBehaviour
    {
        private static CoroutineRunner instance;

        public static CoroutineRunner Instance
        {
            get
            {
                if (instance == null)
                {
                    var go = new GameObject("CoroutineRunner");
                    instance = go.AddComponent<CoroutineRunner>();
                    DontDestroyOnLoad(go);
                }

                return instance;
            }
        }

        public static void Run(IEnumerator routine)
        {
            Instance.StartCoroutine(routine);
        }
    }
}
