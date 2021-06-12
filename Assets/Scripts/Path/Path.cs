using UnityEngine;

public class Path : MonoBehaviour
{
    private const float GizmosSphereRadius = 0.05f;

    [SerializeField] private Vector3[] points = default;

    public int NumPoints => points.Length;

    public Vector3 this[int i]
    {
        get => transform.position + transform.rotation * points[i];
    }

    private void OnDrawGizmos()
    {
        if (points != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = transform.position;

            for (int i = 0; i < points.Length; ++i)
            {
                Vector3 currentPoint = points[i];
                Gizmos.DrawWireSphere(offset + transform.rotation * currentPoint, GizmosSphereRadius);

                if (i > 0)
                {
                    Vector3 previousPoint = points[i - 1];

                    Gizmos.DrawLine(offset + transform.rotation * previousPoint, offset + transform.rotation * currentPoint);
                }
            }
        }
    }
}
