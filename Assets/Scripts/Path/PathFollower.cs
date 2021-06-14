using UnityEngine;

public class PathFollower : MonoBehaviour
{
    private const float DefaultSpeed = 1f;
    private const float DistanceThreshold = 0.1f;

    [SerializeField] private float speed = DefaultSpeed;
    [SerializeField] private Transform followerTransform = default;
    [SerializeField] private BeltConnection beltConnection = default;

    private int m_PathIndex = default;
    private Path m_Path = default;

    public void StartPath(Path path)
    {
        Path previousPath = m_Path;
        m_Path = path;
        m_PathIndex = 0;

        if (previousPath == null)
            followerTransform.position = m_Path[m_PathIndex];
    }

    public void StartPath(BeltConnection beltConnection)
    {
        this.beltConnection = beltConnection;
        StartPath(beltConnection.Path);
    }

    private void Update()
    {
        if (m_Path != null)
        {
            Vector3 currentPosition = followerTransform.position;
            Vector3 targetPosition = m_Path[m_PathIndex];

            float distance = Vector3.Distance(currentPosition, targetPosition);

            if (distance <= DistanceThreshold)
            {
                if (m_PathIndex >= m_Path.NumPoints - 1)
                {
                    BeltConnection nextBeltConnection = beltConnection.nextConnectedBelt;

                    if (nextBeltConnection == null)
                    {
                        followerTransform.position = m_Path[m_PathIndex];
                        m_Path = null;
                    }
                    else
                    {
                        StartPath(nextBeltConnection);
                    }
                }
                else
                {
                    ++m_PathIndex;
                }
            }
            else
            {
                Vector3 newPosition = Vector3.MoveTowards(currentPosition, targetPosition, speed * Time.deltaTime);
                followerTransform.position = newPosition;
            }
        }
    }

    public void Stop()
    {
        m_Path = null;
    }
}
