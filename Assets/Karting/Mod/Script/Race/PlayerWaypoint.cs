using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerWaypoint : MonoBehaviour
{
    RaceObjectives raceObjectives;
    GameObject waypointBackup;
    int playerNumber;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raceObjectives = FindAnyObjectByType<RaceObjectives>();
        string playerStringNumber = Regex.Replace(gameObject.name, @"[^\d]", ""); // Retira todos os caracteres exceto números.
        playerNumber = (Convert.ToInt32(playerStringNumber)) - 1;
    }

    private void Update()
    {
        if (raceObjectives.closestWaypointOfPlayer.Any(waypoint => waypoint == null)) return;

        // Distância do player até o waypoint em que ele está.
        Vector3 distance = raceObjectives.closestWaypointOfPlayer[playerNumber].transform.position - transform.position;
        raceObjectives.distanceClosestWaypoint[playerNumber] = distance.magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Waypoint"))
        {
            waypointBackup = raceObjectives.closestWaypointOfPlayer[playerNumber];
            raceObjectives.closestWaypointOfPlayer[playerNumber] = other.transform.parent.gameObject;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Waypoint") && other.transform.parent.gameObject.name == raceObjectives.closestWaypointOfPlayer[playerNumber].name)
        {
            if (waypointBackup != null)
                raceObjectives.closestWaypointOfPlayer[playerNumber] = waypointBackup;
        }
    }
}
