using UnityEditor;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float DefaultBorderWidth = 50;
    private const float MinBorderWidth = 5;
    private const float MaxBorderWidth = 200;

    private const float GizmosVirtualTargetRadius = 0.1f;

    private const float DefaultVirtualTargetSpeed = 1f;
    private const float DefaultSmoothTime = 0.2f;

    private const float DefaultYaw = 45f;
    private const float DefaultPitch = 45f;
    private const float DefaultOffset = 5f;

    [SerializeField] private Transform cameraTransform = default;
    [SerializeField] [Range(MinBorderWidth, MaxBorderWidth)] private float borderWidth = DefaultBorderWidth;
    [Header("Movement")]
    [SerializeField] private float virtualTargetSpeed = DefaultVirtualTargetSpeed;
    [SerializeField] private float smoothTime = DefaultSmoothTime;
    [Header("Position")]
    [SerializeField] private float yaw = DefaultYaw;
    [SerializeField] private float pitch = DefaultPitch;
    [SerializeField] private float offset = DefaultOffset;
    [Header("Debug")]
    [SerializeField] private bool drawGizmos = false;

    private Vector3 m_VirtualTarget = default;
    private Vector3 m_CameraVelocity = default;

    private void Update()
    {
        Vector2 moveInput = Vector2.zero;

        // Poll for input
        {
            Vector2 mousePosition = Input.mousePosition;

            if (mousePosition.x <= borderWidth)
            {
                moveInput.x = -1f;
            }
            else if (mousePosition.x >= Screen.width - borderWidth)
            {
                moveInput.x = 1f;
            }

            if (mousePosition.y >= Screen.height - borderWidth)
            {
                moveInput.y = 1f;
            }
            else if (mousePosition.y <= borderWidth)
            {
                moveInput.y = -1f;
            }
        }

        // Move camera
        {
            // Move virtual target
            {
                Vector3 right = cameraTransform.right;
                Vector3 forward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;

                Debug.DrawLine(cameraTransform.position, cameraTransform.position + right, Color.blue);
                Debug.DrawLine(cameraTransform.position, cameraTransform.position + forward, Color.red);

                Vector3 targetDirection = (right * moveInput.x + forward * moveInput.y).normalized;
                Vector3 velocity = targetDirection * virtualTargetSpeed;

                m_VirtualTarget += velocity * Time.deltaTime;
            }

            // Smooth damp move camera
            {
                Vector3 targetPosition = m_VirtualTarget + (Quaternion.Euler(pitch, 0f, 0f) * Quaternion.Euler(0f, yaw, 0f) * Vector3.back * offset);

                cameraTransform.position = Vector3.SmoothDamp(cameraTransform.position, targetPosition, ref m_CameraVelocity, smoothTime);
                cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (drawGizmos)
        {
            Handles.BeginGUI();

            Rect rect = new Rect(0, 0, 0, 0);

            // Draw horizontal borders
            DrawBorder(0f, 0f, Screen.width, borderWidth);
            DrawBorder(0f, Screen.height - borderWidth, Screen.width, borderWidth);

            // Draw vertical borders
            DrawBorder(0f, 0f, borderWidth, Screen.height);
            DrawBorder(Screen.width - borderWidth, 0f, borderWidth, Screen.height);

            void DrawBorder(float x, float y, float width, float height)
            {
                rect.x = x;
                rect.y = y;
                rect.width = width;
                rect.height = height;

                Handles.DrawSolidRectangleWithOutline(rect, Color.red, Color.red);
            }

            Handles.EndGUI();

            Gizmos.DrawSphere(m_VirtualTarget, GizmosVirtualTargetRadius);
        }
    }
}
