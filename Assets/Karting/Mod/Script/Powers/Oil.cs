using KartGame.KartSystems;
using System.Collections;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Oil : MonoBehaviour
{
    [Header("Oil Infos")]
    [SerializeField] float timeToDissapear = 30;
    [SerializeField] float timeStunned = 4;
    [SerializeField] float percentOfStun = 0.7f;

    ArcadeKart arcadeKart;
    PlayerPowers playerPowers;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            arcadeKart = other.GetComponent<ArcadeKart>();
            playerPowers = other.gameObject.GetComponent<PlayerPowers>();
            StartCoroutine("Stunned");
        }
    }

    IEnumerator Stunned()
    {
        StopCoroutine(playerPowers.IncressSpeed());

        arcadeKart.baseStats.TopSpeed = playerPowers.initialSpeed * percentOfStun;

        yield return new WaitForSeconds(timeStunned);

        arcadeKart.baseStats.TopSpeed = playerPowers.initialSpeed;
    }
}
