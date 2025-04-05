using KartGame.KartSystems;
using System.Collections;
using Unity.VisualScripting;
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
        if (!other.CompareTag("Player")) return;
        
        if (!other.isTrigger) return;

        arcadeKart = other.GetComponent<ArcadeKart>();
        playerPowers = other.gameObject.GetComponent<PlayerPowers>();
        Destroy(gameObject, timeToDissapear);
        StartCoroutine("Stunned");

    }

    IEnumerator Stunned()
    {
        StopCoroutine(playerPowers.IncressSpeed());

        float initialSpeed = playerPowers.initialSpeed;

        arcadeKart.Rigidbody.linearVelocity *= percentOfStun;
        arcadeKart.baseStats.TopSpeed *= percentOfStun;

        yield return new WaitForSeconds(timeStunned);

        arcadeKart.baseStats.TopSpeed = initialSpeed;
    }
    
}
