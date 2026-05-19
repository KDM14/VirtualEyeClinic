using UnityEngine;
using UnityEngine.SceneManagement;
using VirtualEyeClinic.Core;

namespace VirtualEyeClinic.UI
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private string clinicSceneName = "ClinicScene";

        public void OnStartButtonPressed()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.LoadScene(clinicSceneName);
            }
            else
            {
                SceneManager.LoadScene(clinicSceneName);
            }
        }

        public void OnQuitButtonPressed()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.QuitApplication();
            }
            else
            {
                Application.Quit();
            }
        }
    }
}
