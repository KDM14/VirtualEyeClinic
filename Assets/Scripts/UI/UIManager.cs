using UnityEngine;
using TMPro;

namespace VirtualEyeClinic.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Crosshair")]
        [SerializeField] private GameObject crosshairPanel;
        [SerializeField] private TextMeshProUGUI promptText;

        [Header("Info Popup")]
        [SerializeField] private GameObject infoPopupPanel;
        [SerializeField] private TextMeshProUGUI infoTitleText;
        [SerializeField] private TextMeshProUGUI infoContentText;

        [Header("Image View")]
        [SerializeField] private GameObject imageViewPanel;
        [SerializeField] private UnityEngine.UI.Image largeImageView;

        [Header("Touch Controls")]
        [SerializeField] private GameObject touchControlsPanel;

        public bool IsModalOpen =>
            (infoPopupPanel != null && infoPopupPanel.activeSelf) ||
            (imageViewPanel != null && imageViewPanel.activeSelf);

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            HideAllPanels();
            ShowTouchControls(true);

            if (crosshairPanel != null)
            {
                crosshairPanel.SetActive(true);
            }

            if (promptText != null)
            {
                promptText.gameObject.SetActive(false);
            }
        }

        public void ShowCrosshairPrompt(string message)
        {
            if (IsModalOpen || promptText == null)
            {
                return;
            }

            promptText.text = message;
            promptText.gameObject.SetActive(true);
        }

        public void HideCrosshairPrompt()
        {
            if (promptText != null)
            {
                promptText.gameObject.SetActive(false);
            }
        }

        public void ShowInfoPopup(string title, string content)
        {
            if (infoTitleText != null)
            {
                infoTitleText.text = title;
            }

            if (infoContentText != null)
            {
                infoContentText.text = content;
            }

            if (infoPopupPanel != null)
            {
                infoPopupPanel.SetActive(true);
            }

            HideCrosshairPrompt();
            ShowTouchControls(false);
        }

        public void HideInfoPopup()
        {
            if (infoPopupPanel != null)
            {
                infoPopupPanel.SetActive(false);
            }

            ShowTouchControls(true);
        }

        public void ShowImageView(Sprite sprite)
        {
            if (largeImageView != null)
            {
                largeImageView.sprite = sprite;
            }

            if (imageViewPanel != null)
            {
                imageViewPanel.SetActive(true);
            }

            HideCrosshairPrompt();
            ShowTouchControls(false);
        }

        public void HideImageView()
        {
            if (imageViewPanel != null)
            {
                imageViewPanel.SetActive(false);
            }

            ShowTouchControls(true);
        }

        private void HideAllPanels()
        {
            if (infoPopupPanel != null)
            {
                infoPopupPanel.SetActive(false);
            }

            if (imageViewPanel != null)
            {
                imageViewPanel.SetActive(false);
            }
        }

        private void ShowTouchControls(bool show)
        {
            if (touchControlsPanel != null)
            {
                touchControlsPanel.SetActive(show);
            }
        }
    }
}
