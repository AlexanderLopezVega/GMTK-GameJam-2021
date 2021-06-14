using UnityEngine;

public class BeltDestroyer : MonoBehaviour
{
    [SerializeField] private BeltSelector selector = default;
    [SerializeField] private bool destructionEnabled = default;

    public void SetDestroyerEnabled(bool value)
    {
        destructionEnabled = value;
    }

    public void TryDeleteSelectedBelt()
    {
        // Note: Unity's event system is bad.
        DeleteSelectedBelt();
    }

    public bool DeleteSelectedBelt()
    {
        return DeleteBelt(selector.SelectedBeltConnection);
    }

    public bool DeleteBelt(BeltConnection connection)
    {
        if (!destructionEnabled)
            return false;

        if (connection == null || !connection.Destroyable)
            return false;

        if (connection.previousConnectedBelt != null)
            selector.SelectedBeltConnection = connection.previousConnectedBelt;
        else if (connection.nextConnectedBelt != null)
            selector.SelectedBeltConnection = connection.nextConnectedBelt;
        else
            selector.SelectedBeltConnection = null;

        if (connection.nextConnectedBelt != null)
            connection.nextConnectedBelt.previousConnectedBelt = null;
        if (connection.previousConnectedBelt != null)
            connection.previousConnectedBelt.nextConnectedBelt = null;

        Destroy(connection.gameObject);

        return true;
    }
}
