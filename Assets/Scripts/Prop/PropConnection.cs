using UnityEngine;

public class PropConnection : MonoBehaviour
{
    [SerializeField] private bool isDestroyable = default;
    [SerializeField] public PropConnection previousConnectedProp = default;
    [SerializeField] public PropConnection nextConnectedProp = default;
    [SerializeField] private Transform previousConnectedPropTransform = default;
    [SerializeField] private Transform nextConnectedPropTransform = default;
    [SerializeField] private Path path = default;

    public LocRot PrevLocRot => new LocRot(previousConnectedPropTransform.position, previousConnectedPropTransform.rotation);
    public LocRot NextLocRot => new LocRot(nextConnectedPropTransform.position, nextConnectedPropTransform.rotation);
    public bool Destroyable => isDestroyable;
    public Path Path => path;
}
