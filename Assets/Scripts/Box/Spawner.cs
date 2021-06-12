using System;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private const int DefaultMaxNumBoxes = 2;
    private const float DefaultSpawnFrequency = 2f;

    [SerializeField] private GameObject boxPrefab = default;
    [SerializeField] private Path path = default;
    [SerializeField] private int maxNumBoxes = DefaultMaxNumBoxes;
    [SerializeField] private float spawnFrequency = DefaultSpawnFrequency;

    private bool m_CanSpawn = default;

    private float m_Timer = default;
    private int m_RemainingBoxes = default;

    private List<GameObject> m_BoxList = new List<GameObject>();

    private void Start()
    {
        ResetState();
    }

    private void Update()
    {
        if (m_CanSpawn && m_RemainingBoxes > 0 && m_Timer >= spawnFrequency)
        {
            m_Timer = 0f;
            Spawn();
        }
        else
        {
            m_Timer += Time.deltaTime;
        }
    }

    private void Spawn()
    {
        GameObject clone = Instantiate(boxPrefab, path[0], transform.rotation, null);

        PathFollower pathFollower = clone.GetComponent<PathFollower>();

        pathFollower.Path = path;
        pathFollower.Moving = true;

        --m_RemainingBoxes;

        m_BoxList.Add(clone);
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
        m_RemainingBoxes = maxNumBoxes;
    }
}
