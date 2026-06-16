using UnityEngine;

namespace AHAKuo.Signalia.LocalizationStandalone.Examples
{
    /// <summary>
    /// Example hover script that gently moves and rotates a transform in its local space.
    /// Attach this component to any GameObject to create a smooth hovering effect.
    /// </summary>
    [AddComponentMenu("Signalia Examples/Hover Example")]
    public class HoverExample : MonoBehaviour
    {
        [Header("Position Hover Settings")]
        [Tooltip("Enable position hovering")]
        [SerializeField] private bool enablePositionHover = true;
        
        [Tooltip("The axis in local space to hover along (normalized vector)")]
        [SerializeField] private Vector3 hoverAxis = Vector3.up;
        
        [Tooltip("The distance to hover in local space")]
        [SerializeField] private float hoverDistance = 0.5f;
        
        [Tooltip("The speed of the hover movement")]
        [SerializeField] private float hoverSpeed = 2f;
        
        [Tooltip("The starting offset for the hover animation (0-1)")]
        [Range(0f, 1f)]
        [SerializeField] private float hoverOffset = 0f;

        [Header("Rotation Hover Settings")]
        [Tooltip("Enable rotation hovering")]
        [SerializeField] private bool enableRotationHover = true;
        
        [Tooltip("The axis in local space to rotate around (normalized vector)")]
        [SerializeField] private Vector3 rotationAxis = Vector3.up;
        
        [Tooltip("The maximum rotation angle in degrees")]
        [SerializeField] private float rotationAngle = 15f;
        
        [Tooltip("The speed of the rotation")]
        [SerializeField] private float rotationSpeed = 1f;
        
        [Tooltip("The starting offset for the rotation animation (0-1)")]
        [Range(0f, 1f)]
        [SerializeField] private float rotationOffset = 0f;

        [Header("Advanced Settings")]
        [Tooltip("Use unscaled time (ignores Time.timeScale)")]
        [SerializeField] private bool useUnscaledTime = false;

        private Vector3 initialLocalPosition;
        private Quaternion initialLocalRotation;
        private float hoverTime;
        private float rotationTime;

        private void Awake()
        {
            // Store initial local position and rotation
            initialLocalPosition = transform.localPosition;
            initialLocalRotation = transform.localRotation;
            
            // Initialize time values with offsets
            hoverTime = hoverOffset * Mathf.PI * 2f;
            rotationTime = rotationOffset * Mathf.PI * 2f;
        }

        private void Update()
        {
            float deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            
            // Update position hover
            if (enablePositionHover)
            {
                hoverTime += hoverSpeed * deltaTime;
                
                // Use sine wave for smooth oscillation
                float sineValue = Mathf.Sin(hoverTime);
                
                // Calculate offset in local space
                Vector3 offset = hoverAxis.normalized * (sineValue * hoverDistance);
                
                // Apply to local position
                transform.localPosition = initialLocalPosition + offset;
            }
            
            // Update rotation hover
            if (enableRotationHover)
            {
                rotationTime += rotationSpeed * deltaTime;
                
                // Use sine wave for smooth oscillation
                float sineValue = Mathf.Sin(rotationTime);
                
                // Calculate rotation angle
                float angle = sineValue * rotationAngle;
                
                // Apply rotation in local space
                transform.localRotation = initialLocalRotation * Quaternion.AngleAxis(angle, rotationAxis.normalized);
            }
        }

        /// <summary>
        /// Resets the hover animation to its initial state.
        /// Useful for resetting the animation when needed.
        /// </summary>
        public void ResetHover()
        {
            transform.localPosition = initialLocalPosition;
            transform.localRotation = initialLocalRotation;
            hoverTime = hoverOffset * Mathf.PI * 2f;
            rotationTime = rotationOffset * Mathf.PI * 2f;
        }

        /// <summary>
        /// Updates the initial position and rotation to the current values.
        /// Useful if you want to change the base position/rotation at runtime.
        /// </summary>
        public void UpdateInitialTransform()
        {
            initialLocalPosition = transform.localPosition;
            initialLocalRotation = transform.localRotation;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            // Ensure axes are normalized
            hoverAxis = hoverAxis.normalized;
            rotationAxis = rotationAxis.normalized;
            
            // Clamp values to reasonable ranges
            hoverDistance = Mathf.Max(0f, hoverDistance);
            hoverSpeed = Mathf.Max(0f, hoverSpeed);
            rotationAngle = Mathf.Clamp(rotationAngle, 0f, 360f);
            rotationSpeed = Mathf.Max(0f, rotationSpeed);
        }
#endif
    }
}
