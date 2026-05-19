using UnityEngine;
using UnityEngine.EventSystems;

namespace VirtualEyeClinic.Player
{
    public class CameraLook : MonoBehaviour
    {
        [SerializeField] private float sensitivity = 0.15f;
        [SerializeField] private Transform playerBody;
        [SerializeField] [Range(0f, 1f)] private float lookZoneScreenWidth = 0.55f;

        private float xRotation;
        private int lookTouchId = -1;

        private void Update()
        {
            HandleTouchLook();
#if UNITY_EDITOR
            HandleMouseLook();
#endif
        }

        private void HandleTouchLook()
        {
            if (Input.touchCount == 0)
            {
                lookTouchId = -1;
                return;
            }

            if (lookTouchId == -1)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (IsLookTouch(touch) && touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                    {
                        lookTouchId = touch.fingerId;
                        break;
                    }
                }
            }

            if (lookTouchId == -1)
            {
                return;
            }

            bool found = false;
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                if (touch.fingerId != lookTouchId)
                {
                    continue;
                }

                found = true;
                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    lookTouchId = -1;
                    break;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    ApplyLook(touch.deltaPosition.x, touch.deltaPosition.y);
                }

                break;
            }

            if (!found)
            {
                lookTouchId = -1;
            }
        }

        private bool IsLookTouch(Touch touch)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return false;
            }

            float normalizedX = touch.position.x / Screen.width;
            return normalizedX >= lookZoneScreenWidth;
        }

#if UNITY_EDITOR
        private void HandleMouseLook()
        {
            if (Input.GetMouseButton(1))
            {
                ApplyLook(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            }
        }
#endif

        private void ApplyLook(float deltaX, float deltaY)
        {
            float mouseX = deltaX * sensitivity;
            float mouseY = deltaY * sensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            if (playerBody != null)
            {
                playerBody.Rotate(Vector3.up * mouseX);
            }
        }
    }
}
