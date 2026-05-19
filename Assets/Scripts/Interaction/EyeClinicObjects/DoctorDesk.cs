using UnityEngine;
using VirtualEyeClinic.UI;
using VirtualEyeClinic.Core;

namespace VirtualEyeClinic.Interaction.EyeClinicObjects
{
    public class DoctorDesk : MonoBehaviour, IInteractable
    {
        [SerializeField] private AudioClip doctorAudioClip;
        [SerializeField] [TextArea] private string subtitles = "Doctor: Please have a seat. We will begin the examination shortly.";

        public string GetInteractionPrompt()
        {
            return "Tap to listen to Doctor";
        }

        public void Interact()
        {
#if UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RegisterInteraction(gameObject.name);
            }

            if (doctorAudioClip != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(doctorAudioClip);
            }

            if (UIManager.Instance != null)
            {
                // Reusing InfoPopup for subtitles/text appearance
                UIManager.Instance.ShowInfoPopup("Doctor's Desk", subtitles);
            }
        }
    }
}
