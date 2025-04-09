using NUnit.Framework.Internal;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// This class inherits from TargetObject and represents a LapObject.
/// </summary>
public class CountLaps : MonoBehaviour
{
    RaceObjectives raceObjectives;
    [SerializeField] Collider initialCollider;
    [SerializeField] Collider finalCollider;

    bool[] initialTriggerEnter;
    bool[] finalTriggerEnter;

    bool[] initialTriggerExit;
    bool[] finalTriggerExit;

    void Start()
    {
        raceObjectives = FindAnyObjectByType<RaceObjectives>();

        initialTriggerEnter = new bool[raceObjectives.players.Length];
        finalTriggerEnter = new bool[raceObjectives.players.Length];
        initialTriggerExit = new bool[raceObjectives.players.Length];
        finalTriggerExit = new bool[raceObjectives.players.Length];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (!other.isTrigger) return;

        string playerStringNumber = Regex.Replace(other.name, @"[^\d]", ""); // Retira todos os caracteres exceto números.
        int playerNumber = (Convert.ToInt32(playerStringNumber)) - 1; // Transforma string em número.

        // Aumentar a volta
        if (finalCollider.bounds.Intersects(other.bounds))
        {
            if (initialTriggerEnter[playerNumber])
            {
                finalTriggerEnter[playerNumber] = true;
            }
        }

        if (initialCollider.bounds.Intersects(other.bounds) && !finalCollider.bounds.Intersects(other.bounds))
        {
            initialTriggerEnter[playerNumber] = true;
        }

        // Diminuir a volta.
        if (initialCollider.bounds.Intersects(other.bounds))
        {
            if (finalTriggerExit[playerNumber])
            {
                initialTriggerExit[playerNumber] = true;
            }
        }

        if (finalCollider.bounds.Intersects(other.bounds) && !initialCollider.bounds.Intersects(other.bounds))
        {
            finalTriggerExit[playerNumber] = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (!other.isTrigger) return;

        string playerStringNumber = Regex.Replace(other.name, @"[^\d]", ""); // Retira todos os caracteres exceto números.
        int playerNumber = (Convert.ToInt32(playerStringNumber)) - 1; // Transforma string em número.

        // Aumentar a volta.
        if (!initialCollider.bounds.Intersects(other.bounds) && !finalCollider.bounds.Intersects(other.bounds))
        {
            if (finalTriggerEnter[playerNumber])
            {
                raceObjectives.CountLapsPerPlayer(other.gameObject, 1);
                initialTriggerEnter[playerNumber] = false;
                finalTriggerEnter[playerNumber] = false;
            }

            if (initialTriggerExit[playerNumber])
            {
                raceObjectives.CountLapsPerPlayer(other.gameObject, -1);
                initialTriggerExit[playerNumber] = false;
                finalTriggerExit[playerNumber] = false;
            }
        }

        if (!initialCollider.bounds.Intersects(other.bounds))
        {
            initialTriggerEnter[playerNumber] = false;

            initialTriggerExit[playerNumber] = false;
        }

        if (!finalCollider.bounds.Intersects(other.bounds))
        {
            finalTriggerEnter[playerNumber] = false;

            finalTriggerExit[playerNumber] = false;
        }
    }
}
