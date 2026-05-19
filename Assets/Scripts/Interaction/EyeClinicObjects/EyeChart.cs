using UnityEngine;
using VirtualEyeClinic.Core;
using VirtualEyeClinic.UI;

namespace VirtualEyeClinic.Interaction.EyeClinicObjects
{
    public class EyeChart : MonoBehaviour, IInteractable
    {
        [SerializeField] private string itemName = "Snellen Eye Chart";
        [SerializeField] [TextArea] private string infoText =
            "The Snellen chart measures visual acuity. Patients read letters from a set distance while the doctor records the smallest line they can see clearly.";
        [SerializeField] private AudioClip interactSound;
        [SerializeField] private Animator chartAnimator;

        public string GetInteractionPrompt()
        {
            return "Tap to read eye chart info";
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

            if (chartAnimator != null)
            {
                chartAnimator.SetTrigger("Flip");
            }

            if (interactSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(interactSound);
            }

            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowInfoPopup(itemName, infoText);
            }
        }
    }
}
