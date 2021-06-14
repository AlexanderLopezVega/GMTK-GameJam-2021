using UnityEngine;
using UnityEngine.Events;

public class BeltSpawner : MonoBehaviour
{
    private static readonly Vector3 PhysicsCheckBoxHalfExtents = new Vector3(.4f, .25f, .4f);

    [SerializeField] private BeltSelector beltSelector = default;
    [SerializeField] private BeltPreview beltPreview = default;
    [SerializeField] private LayerMask beltMask = default;
    [SerializeField] private UnityEvent<BeltConnection> onEndOfLine = default;
    [SerializeField] private bool spawnerEnabled = default;

    public bool SpawnBelt(BeltSO beltSO)
    {
        if (!spawnerEnabled)
            return false;
        if (beltSelector.Selected == null)
            return false;

        LocRot nextLocRot = beltSelector.SelectedBeltConnection.NextLocRot;

        Vector3 newPosition = nextLocRot.location;
        Quaternion newRotation = nextLocRot.rotation;

        // Check if there are belts in the way
        {
            Collider[] affectedColliders = Physics.OverlapBox(newPosition, PhysicsCheckBoxHalfExtents, Quaternion.identity, beltMask);

            if (affectedColliders.Length > 0)
            {
                GameObject previewGO = beltPreview.PreviewGameObject;

                foreach (Collider collider in affectedColliders)
                {
                    if (collider.gameObject != previewGO)
                    {
                        Debug.Log("Belt in the way");
                        return false;
                    }
                }
            }
        }

        // Instantiate new belt
        GameObject beltClone = Instantiate(beltSO.Prefab, newPosition, newRotation, null);

        // Add belt to linked list
        BeltConnection beltConnection = beltClone.GetComponent<BeltConnection>();

        beltConnection.previousConnectedBelt = beltSelector.SelectedBeltConnection;
        beltSelector.SelectedBeltConnection.nextConnectedBelt = beltConnection;

        // Update selected belt
        beltSelector.Selected = beltClone;


        // Check if there are belts in the way
        {
            nextLocRot = beltSelector.SelectedBeltConnection.NextLocRot;

            newPosition = nextLocRot.location;

            Collider[] affectedColliders = Physics.OverlapBox(newPosition, PhysicsCheckBoxHalfExtents, Quaternion.identity, beltMask);

            foreach (Collider collider in affectedColliders)
            {
                if(collider.gameObject.TryGetComponent(out BeltConnection affectedBeltConnection))
                {
                    beltSelector.SetSelectedNext(affectedBeltConnection);
                }

                if (collider.gameObject.GetComponent<BoxReceiver>() != null)
                {
                    Debug.Log("End of line");

                    BeltConnection endOfLineBeltConnection = collider.gameObject.GetComponent<BeltConnection>();

                    onEndOfLine.Invoke(endOfLineBeltConnection);

                    return true;
                }
            }
        }


        return true;
    }

    public void SetSpawnerEnabled(bool value)
    {
        spawnerEnabled = value;
    }
}
