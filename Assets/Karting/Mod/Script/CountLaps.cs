using UnityEngine;

/// <summary>
/// This class inherits from TargetObject and represents a LapObject.
/// </summary>
public class CountLaps : MonoBehaviour
{
    RaceObjectives raceObjectives;
        
    void Start()
    {
        raceObjectives = FindAnyObjectByType<RaceObjectives>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        raceObjectives.CountLapsPerPlayer(other.gameObject);
    }
}
