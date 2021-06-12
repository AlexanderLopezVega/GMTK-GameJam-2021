using UnityEngine;
using UnityEngine.Assertions;

public class PropSpawner : MonoBehaviour
{
    private static readonly Vector3 PhysicsCheckBoxHalfExtents = Vector3.one * 0.45f;

    [SerializeField] private PropSelector selector = default;
    [SerializeField] private PropDestroyer destroyer = default;
    [SerializeField] private GameObject straightCBPrefab = default;
    [SerializeField] private GameObject leftTurnCBPrefab = default;
    [SerializeField] private GameObject rightTurnCBPrefab = default;

    public void SpawnStraightProp() => SpawnProp(PropType.Straight);

    public void SpawnLeftTurnProp() => SpawnProp(PropType.Turn_Left);

    public void SpawnRightTurnProp() => SpawnProp(PropType.Turn_Right);

    public void SpawnProp(PropType type)
    {
        LocRot nextLocRot = selector.SelectedPropConnection.NextLocRot;

        Vector3 newPosition = nextLocRot.location;

        Quaternion newRotation = nextLocRot.rotation;

        Collider[] affectedColliders = Physics.OverlapBox(newPosition, PhysicsCheckBoxHalfExtents, Quaternion.identity);

        if (affectedColliders.Length > 0)
        {
            Assert.AreEqual(affectedColliders.Length, 1);

            PropConnection hitPropConnection = affectedColliders[0].GetComponent<PropConnection>();

            if(!destroyer.DeleteProp(hitPropConnection))
            {
                return;
            }
        }

        GameObject clone;

        switch (type)
        {
            case PropType.Straight:
                {
                    clone = Instantiate(straightCBPrefab, newPosition, newRotation, null);
                }
                break;
            case PropType.Turn_Left:
                {
                    clone = Instantiate(leftTurnCBPrefab, newPosition, newRotation, null);
                }
                break;
            case PropType.Turn_Right:
                {
                    clone = Instantiate(rightTurnCBPrefab, newPosition, newRotation, null);
                }
                break;
            default: return;
        }

        PropConnection connection = clone.GetComponent<PropConnection>();

        connection.previousConnectedProp = selector.SelectedPropConnection;
        selector.SelectedPropConnection.nextConnectedProp = connection;

        selector.Selected = clone;
    }
}
