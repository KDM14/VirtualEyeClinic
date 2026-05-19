using UnityEngine;
using VirtualEyeClinic.UI;
using VirtualEyeClinic.Core;

namespace VirtualEyeClinic.Interaction.EyeClinicObjects
{
    public class SlitLamp : MonoBehaviour, IInteractable
    {
        [SerializeField] private string itemName = "Slit Lamp";
        [SerializeField] [TextArea] private string infoText = "This is a slit lamp. It is used by optometrists and ophthalmologists to examine the anterior segment of the eye, including the cornea, lens, and iris.";
        [SerializeField] private AudioClip interactSound;
        [SerializeField] private Animator animator;

        public string GetInteractionPrompt()
        {
            return $"Tap to view {itemName} info";
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

            // Trigger animation if available
            if (animator != null)
            {
                animator.SetTrigger("Activate");
            }

            // Play sound
            if (interactSound != null && AudioManager.Instance != null)
            {
                AudioManager.Instance.PlaySound(interactSound);
            }

            // Show UI popup
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowInfoPopup(itemName, infoText);
            }
        }
    }
}
