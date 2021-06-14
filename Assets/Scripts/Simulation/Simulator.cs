using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Simulator : MonoBehaviour
{
    [SerializeField] private List<BoxSpawner> boxSpawnerList = default;
    [SerializeField] private UnityEvent onLevelCompleteEvent = default;

    private int m_CompletedReceivers = default;

    public void StartSimulation()
    {
        if (m_CompletedReceivers < boxSpawnerList.Count)
        {
            foreach (BoxSpawner spawner in boxSpawnerList)
            {
                spawner.ResetState();
                spawner.BeginSpawning();
            }
        }
    }

    public void StopSimulation()
    {
        if (m_CompletedReceivers < boxSpawnerList.Count)
        {
            foreach (BoxSpawner spawner in boxSpawnerList)
            {
                spawner.StopSpawning();
                spawner.DeleteBoxes();
                spawner.ResetState();
            }
        }
    }

    public void OnReceiverAllBoxesReceived()
    {
        ++m_CompletedReceivers;

        if (m_CompletedReceivers >= boxSpawnerList.Count)
        {
            onLevelCompleteEvent.Invoke();
        }
    }
}
