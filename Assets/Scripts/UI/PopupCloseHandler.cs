using UnityEngine;

namespace VirtualEyeClinic.UI
{
    public class PopupCloseHandler : MonoBehaviour
    {
        public void CloseInfoPopup()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideInfoPopup();
            }
        }

        public void CloseImageView()
        {
            if (UIManager.Instance != null)
            {
                UIManager.Instance.HideImageView();
            }
        }
    }
}
