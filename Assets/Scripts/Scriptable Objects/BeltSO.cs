using UnityEngine;

[CreateAssetMenu(fileName = "Belt SO", menuName = "Scriptable Objects / Belt")]
public class BeltSO : ScriptableObject
{
    [SerializeField] private GameObject beltPrefab = default;
    [SerializeField] private BeltConnection beltConnection = default;

    public GameObject Prefab => beltPrefab;
    public BeltConnection Connection => beltConnection;
}
