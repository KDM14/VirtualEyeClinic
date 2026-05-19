using UnityEngine;

namespace VirtualEyeClinic.Interaction
{
    [RequireComponent(typeof(Renderer))]
    public class InteractableHighlight : MonoBehaviour
    {
        [SerializeField] private Color highlightColor = new Color(0.4f, 0.8f, 1f, 1f);
        [SerializeField] private float highlightIntensity = 0.35f;

        private Renderer targetRenderer;
        private Material instanceMaterial;
        private Color originalEmission;
        private static readonly int EmissionColorId = Shader.PropertyToID("_EmissionColor");

        private void Awake()
        {
            targetRenderer = GetComponent<Renderer>();
            instanceMaterial = targetRenderer.material;
            originalEmission = instanceMaterial.HasProperty(EmissionColorId)
                ? instanceMaterial.GetColor(EmissionColorId)
                : Color.black;
        }

        public void SetHighlighted(bool highlighted)
        {
            if (instanceMaterial == null || !instanceMaterial.HasProperty(EmissionColorId))
            {
                return;
            }

            instanceMaterial.EnableKeyword("_EMISSION");
            instanceMaterial.SetColor(
                EmissionColorId,
                highlighted ? highlightColor * highlightIntensity : originalEmission);
        }

        private void OnDestroy()
        {
            if (instanceMaterial != null)
            {
                Destroy(instanceMaterial);
            }
        }
    }
}
