using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BoxSpawner : MonoBehaviour
{
    private const int DefaultMaxNumBoxes = 2;
    private const float DefaultSpawnFrequency = 2f;

    [SerializeField] private GameObject boxPrefab = default;
    [SerializeField] private BeltConnection spawnerBeltConnection = default;
    [SerializeField] private Path path = default;
    [SerializeField] private int maxNumBoxes = DefaultMaxNumBoxes;
    [SerializeField] private float spawnFrequency = DefaultSpawnFrequency;
    [SerializeField] private BoxData.BoxColor spawnBoxColor = default;
    [SerializeField] private Material spawnBoxMaterial = default;
    [SerializeField] private UnityEvent<int> onBoxSpawnedEvent = default;
    [SerializeField] private UnityEvent<int> onSpawnerStartEvent = default;

    private bool m_CanSpawn = default;

    private float m_Timer = default;
    private List<GameObject> m_BoxList = new List<GameObject>();

    public int RemainingBoxes { get; private set; } = default;

    private void Start()
    {
        ResetState();
        onSpawnerStartEvent.Invoke(0);
    }

    private void Update()
    {
        if (m_CanSpawn && RemainingBoxes > 0 && m_Timer >= spawnFrequency)
        {
            m_Timer = 0f;
            SpawnBox();
        }
        else
        {
            m_Timer += Time.deltaTime;
        }
    }

    private void SpawnBox()
    {
        GameObject clone = Instantiate(boxPrefab, path[0], transform.rotation, null);

        clone.GetComponent<BoxData>().Color = spawnBoxColor;
        clone.GetComponentInChildren<MeshRenderer>().sharedMaterial = spawnBoxMaterial;

        PathFollower pathFollower = clone.GetComponent<PathFollower>();

        pathFollower.StartPath(spawnerBeltConnection);

        --RemainingBoxes;

        m_BoxList.Add(clone);

        onBoxSpawnedEvent.Invoke(1);
    }

    public void BeginSpawning()
    {
        m_CanSpawn = true;
    }

    public void StopSpawning()
    {
        m_CanSpawn = false;
    }

    public void DeleteBoxes()
    {
        for(int i = 0; i < m_BoxList.Count; ++i)
        {
            Destroy(m_BoxList[i]);
            m_BoxList[i] = null;
        }
    }

    public void ResetState()
    {
        RemainingBoxes = maxNumBoxes;
    }
}
