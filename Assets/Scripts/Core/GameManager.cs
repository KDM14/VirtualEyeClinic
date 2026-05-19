using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace VirtualEyeClinic.Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private HashSet<string> interactedObjects = new HashSet<string>();
        public int InteractedCount => interactedObjects.Count;
        public int TotalObjectsToFind { get; private set; } = 5;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadProgress();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RegisterInteraction(string objectId)
        {
            if (!interactedObjects.Contains(objectId))
            {
                interactedObjects.Add(objectId);
                SaveProgress();

                if (interactedObjects.Count == TotalObjectsToFind)
                {
                    VirtualEyeClinic.UI.UIManager.Instance?.ShowInfoPopup(
                        "Congratulations!",
                        "You have explored all the equipment in the clinic. The tour is complete!"
                    );
                }
            }
        }

        private void SaveProgress()
        {
            PlayerPrefs.SetInt("InteractedCount", interactedObjects.Count);
            int i = 0;
            foreach (var obj in interactedObjects)
            {
                PlayerPrefs.SetString($"InteractedObj_{i}", obj);
                i++;
            }
            PlayerPrefs.Save();
        }

        private void LoadProgress()
        {
            interactedObjects.Clear();
            int count = PlayerPrefs.GetInt("InteractedCount", 0);
            for (int i = 0; i < count; i++)
            {
                string obj = PlayerPrefs.GetString($"InteractedObj_{i}", "");
                if (!string.IsNullOrEmpty(obj))
                {
                    interactedObjects.Add(obj);
                }
            }
        }

        public void ResetProgress()
        {
            interactedObjects.Clear();
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }
    }
}
