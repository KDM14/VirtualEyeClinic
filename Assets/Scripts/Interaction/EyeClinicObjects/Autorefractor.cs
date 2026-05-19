using System.Collections;
using UnityEngine;
using VirtualEyeClinic.Core;
using VirtualEyeClinic.UI;

namespace VirtualEyeClinic.Interaction.EyeClinicObjects
{
    public class Autorefractor : MonoBehaviour, IInteractable
    {
        [SerializeField] private string itemName = "Autorefractor";
        [SerializeField] [TextArea] private string infoText =
            "An autorefractor estimates refractive error automatically. It helps the clinician get a starting prescription before a full subjective refraction.";
        [SerializeField] private AudioClip interactSound;
        [SerializeField] private Animator deviceAnimator;
        [SerializeField] private Transform measurementArm;
        [SerializeField] private float armRotateDegrees = 15f;
        [SerializeField] private float armRotateDuration = 0.6f;

        public string GetInteractionPrompt()
        {
            return "Tap to run autorefractor scan";
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

            if (deviceAnimator != null)
            {
                deviceAnimator.SetTrigger("Scan");
            }

            if (measurementArm != null)
            {
                CoroutineRunner.Run(RotateArmRoutine());
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

        private IEnumerator RotateArmRoutine()
        {
            Quaternion start = measurementArm.localRotation;
            Quaternion end = start * Quaternion.Euler(0f, armRotateDegrees, 0f);
            float elapsed = 0f;

            while (elapsed < armRotateDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / armRotateDuration);
                measurementArm.localRotation = Quaternion.Slerp(start, end, t);
                yield return null;
            }

            measurementArm.localRotation = end;
        }
    }
}
