using UnityEngine;
using UnityEngine.EventSystems;

namespace VirtualEyeClinic.UI
{
    public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform background;
        [SerializeField] private RectTransform handle;
        [SerializeField] private float handleRange = 50f;

        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }

        private Vector2 inputVector;
        private Canvas canvas;
        private Camera uiCamera;

        private void Start()
        {
            canvas = GetComponentInParent<Canvas>();
            if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                uiCamera = canvas.worldCamera;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                background,
                eventData.position,
                uiCamera,
                out Vector2 localPoint);

            Vector2 backgroundSize = background.sizeDelta;
            inputVector = new Vector2(
                localPoint.x / (backgroundSize.x * 0.5f),
                localPoint.y / (backgroundSize.y * 0.5f));

            inputVector = Vector2.ClampMagnitude(inputVector, 1f);
            handle.anchoredPosition = inputVector * handleRange;

            Horizontal = inputVector.x;
            Vertical = inputVector.y;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            inputVector = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            Horizontal = 0f;
            Vertical = 0f;
        }
    }
}
