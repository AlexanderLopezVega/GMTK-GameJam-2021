using UnityEngine;
using UnityEngine.Events;

public class BoxReceiver : MonoBehaviour
{
    private const int DefaultMaxBoxesToReceive = 2;

    [SerializeField] private int maxBoxesToReceive = DefaultMaxBoxesToReceive;
    [SerializeField] private BoxData.BoxColor receiverColor = default;
    [SerializeField] private bool receiverEnabled = default;
    [SerializeField] private UnityEvent<int> boxReceiverStartEvent = default;
    [SerializeField] private UnityEvent<int> boxReceivedEvent = default;
    [SerializeField] private UnityEvent allBoxesReceivedEvent = default;

    private int m_RemainingBoxes = default;

    private void Start()
    {
        m_RemainingBoxes = maxBoxesToReceive;
        boxReceiverStartEvent.Invoke(0);
    }

    public void OnBoxReceived(Collider boxCollider)
    {
        if (boxCollider.TryGetComponent(out BoxData boxData) && boxData.Color.Equals(receiverColor) && receiverEnabled)
        {
            --m_RemainingBoxes;
            boxReceivedEvent.Invoke(1);

            if (m_RemainingBoxes <= 0)
            {
                receiverEnabled = false;
                allBoxesReceivedEvent.Invoke();
            }

            Destroy(boxCollider.gameObject);
        }
        else
        {
            boxCollider.GetComponent<PathFollower>().Stop();
        }
    }
}
