using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    [SerializeField] private List<Spawner> spawnerList = default;

    public void StartSimulation()
    {
        foreach(Spawner spawner in spawnerList)
        {
            spawner.BeginSpawning();
        }
    }

    public void StopSimulation()
    {
        foreach(Spawner spawner in spawnerList)
        {
            spawner.StopSpawning();
            spawner.DeleteBoxes();
            spawner.ResetState();
        }
    }
}
