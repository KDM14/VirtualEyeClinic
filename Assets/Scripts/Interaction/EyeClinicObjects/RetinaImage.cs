using UnityEngine;
using VirtualEyeClinic.UI;
using VirtualEyeClinic.Core;

namespace VirtualEyeClinic.Interaction.EyeClinicObjects
{
    public class RetinaImage : MonoBehaviour, IInteractable
    {
        [SerializeField] private Sprite highResRetinaSprite;
        [SerializeField] private AudioClip interactSound;

        public string GetInteractionPrompt()
        {
            return "Tap to view Retina Scan";
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

            if (interactSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(interactSound);
            }

            if (UIManager.Instance != null && highResRetinaSprite != null)
            {
                UIManager.Instance.ShowImageView(highResRetinaSprite);
            }
        }
    }
}
