using System;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private const float DefaultSpeed = 1f;
    private const float DistanceThreshold = 0.2f;

    [SerializeField] private float speed = DefaultSpeed;
    [SerializeField] private Transform followerTransform = default;
    [SerializeField] private PropConnection propConnection = default;

    public Path Path { get; set; }

    public bool Moving { get; set; }

    private int m_PathIndex = default;

    private void Update()
    {
        Vector3 nextPoint = Path[m_PathIndex];
        Vector3 currentPosition = followerTransform.position;

        if (Vector3.Distance(currentPosition, nextPoint) < DistanceThreshold)
        {
            if (m_PathIndex == Path.NumPoints - 1)
            {
                MoveToNextPath();
            }
            else
            {
                nextPoint = Path[++m_PathIndex];
            }
        }

        Vector3 direction = (nextPoint - currentPosition).normalized;

        followerTransform.Translate(direction * speed);
    }

    private void MoveToNextPath()
    {
        PropConnection nextPropConnection = propConnection.nextConnectedProp;

        if (nextPropConnection == null)
        {
            followerTransform.position = Path[Path.NumPoints - 1];
        }
        else
        {
            Path = nextPropConnection.Path;

            followerTransform.position = Path[0];
        }
    }
}
