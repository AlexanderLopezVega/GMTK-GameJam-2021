using UnityEngine;

public class PropDestroyer : MonoBehaviour
{
    [SerializeField] private PropSelector selector = default;

    public void TryDeleteSelectedProp()
    {
        // Note: Unity's event system is bad.
        DeleteSelectedProp();
    }

    public bool DeleteSelectedProp()
    {
        return DeleteProp(selector.SelectedPropConnection);
    }

    public bool DeleteProp(PropConnection connection)
    {
        if (connection == null || !connection.Destroyable)
            return false;

        if (connection.nextConnectedProp != null)
            connection.nextConnectedProp.previousConnectedProp = null;

        if (connection.previousConnectedProp != null)
            selector.SelectedPropConnection = connection.previousConnectedProp;
        else if (connection.nextConnectedProp != null)
            selector.SelectedPropConnection = connection.nextConnectedProp;

        if (connection.previousConnectedProp != null)
            connection.previousConnectedProp.nextConnectedProp = null;

        Destroy(connection.gameObject);

        return true;
    }
}
