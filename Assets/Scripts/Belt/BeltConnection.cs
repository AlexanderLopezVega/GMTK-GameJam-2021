using UnityEngine;

public class BeltConnection : MonoBehaviour
{
    [SerializeField] private bool isDestroyable = default;
    [SerializeField] private bool isSelectable = default;
    [SerializeField] public BeltConnection previousConnectedBelt = default;
    [SerializeField] public BeltConnection nextConnectedBelt = default;
    [SerializeField] private Transform previousConnectedBeltTransform = default;
    [SerializeField] private Transform nextConnectedBeltTransform = default;
    [SerializeField] private Path path = default;

    public LocRot PrevLocRot => new LocRot(previousConnectedBeltTransform.position, previousConnectedBeltTransform.rotation);
    public LocRot NextLocRot => new LocRot(nextConnectedBeltTransform.position, nextConnectedBeltTransform.rotation);
    public bool Destroyable => isDestroyable;
    public bool Selectable => isSelectable;
    public Path Path => path;
}
