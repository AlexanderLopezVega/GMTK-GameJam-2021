using UnityEngine;

public struct LocRot
{
    public Vector3 location;
    public Quaternion rotation;

    public LocRot(Vector3 location, Quaternion rotation)
    {
        this.location = location;
        this.rotation = rotation;
    }
}
