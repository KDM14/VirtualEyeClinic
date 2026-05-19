using UnityEngine;
using VirtualEyeClinic.UI;

namespace VirtualEyeClinic.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 3.5f;
        [SerializeField] private VirtualJoystick movementJoystick;
        [SerializeField] private float gravity = -9.81f;

        [Header("Head Bobbing")]
        [SerializeField] private bool enableHeadBob = true;
        [SerializeField] private float bobFrequency = 10f;
        [SerializeField] private float bobAmplitude = 0.05f;

        private CharacterController characterController;
        private float verticalVelocity;
        private Camera playerCamera;
        private Vector3 cameraInitialLocalPos;
        private float bobTimer;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerCamera = GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                cameraInitialLocalPos = playerCamera.transform.localPosition;
            }
        }

        private void Update()
        {
            float horizontal = 0f;
            float vertical = 0f;

            if (movementJoystick != null)
            {
                horizontal = movementJoystick.Horizontal;
                vertical = movementJoystick.Vertical;
            }
#if UNITY_EDITOR
            else
            {
                horizontal = Input.GetAxisRaw("Horizontal");
                vertical = Input.GetAxisRaw("Vertical");
            }
#endif

            Vector3 moveDirection = transform.right * horizontal + transform.forward * vertical;
            if (moveDirection.sqrMagnitude > 1f)
            {
                moveDirection.Normalize();
            }

            if (characterController.isGrounded && verticalVelocity < 0f)
            {
                verticalVelocity = -2f;
            }

            verticalVelocity += gravity * Time.deltaTime;

            Vector3 horizontalMove = moveDirection * moveSpeed * Time.deltaTime;
            Vector3 verticalMove = Vector3.up * verticalVelocity * Time.deltaTime;
            characterController.Move(horizontalMove + verticalMove);

            HandleHeadBob();
        }

        private void HandleHeadBob()
        {
            if (enableHeadBob && playerCamera != null && characterController.isGrounded)
            {
                float currentSpeed = new Vector3(characterController.velocity.x, 0, characterController.velocity.z).magnitude;
                if (currentSpeed > 0.1f)
                {
                    bobTimer += Time.deltaTime * bobFrequency;
                    float newY = cameraInitialLocalPos.y + Mathf.Sin(bobTimer) * bobAmplitude;
                    playerCamera.transform.localPosition = new Vector3(cameraInitialLocalPos.x, newY, cameraInitialLocalPos.z);
                }
                else
                {
                    bobTimer = 0f;
                    playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, cameraInitialLocalPos, Time.deltaTime * bobFrequency);
                }
            }
        }
    }
}
