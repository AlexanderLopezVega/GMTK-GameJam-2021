using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent<Collider> onTriggerEnterEvent = default;
    [SerializeField] private UnityEvent<Collider> onTriggerStayEvent = default;
    [SerializeField] private UnityEvent<Collider> onTriggerExitEvent = default;

    private void OnTriggerEnter(Collider other) => onTriggerEnterEvent.Invoke(other);
    private void OnTriggerStay(Collider other) => onTriggerStayEvent.Invoke(other);
    private void OnTriggerExit(Collider other) => onTriggerExitEvent.Invoke(other);
}
