using KartGame.KartSystems;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Oil : MonoBehaviour
{
    [Header("Oil Infos")]

    [Tooltip("Tempo para o óleo desaparecer da cena")]
    [SerializeField] float timeToDissapear = 30;

    [Tooltip("Tempo que os players adversários ficam lentos")]
    [SerializeField] float timeStunned = 4;

    [Range(0,1), Tooltip("Poncentagem que deixa os players adversários mais lentos, Exemplo: speed = speed + speed * percentOfStun")]
    [SerializeField] float percentOfStun = 0.7f;

    ArcadeKart arcadeKart;
    PlayerPowers playerPowers;

    private void Start()
    {
        Destroy(gameObject, timeToDissapear);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        if (!other.isTrigger) return;

        arcadeKart = other.GetComponent<ArcadeKart>();
        playerPowers = other.gameObject.GetComponent<PlayerPowers>();

        StartCoroutine("Stunned");
        playerPowers.oilPercentOfStun = percentOfStun;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!other.isTrigger) return;

        GetComponent<AudioSource>().enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!other.isTrigger) return;

        GetComponent<AudioSource>().enabled = false;
    }

    IEnumerator Stunned()
    {
        float initialSpeed = playerPowers.initialTopSpeed;

        arcadeKart.Rigidbody.linearVelocity *= 1 - ((initialSpeed * percentOfStun) / arcadeKart.baseStats.TopSpeed);
        arcadeKart.baseStats.TopSpeed -= initialSpeed * percentOfStun;

        yield return new WaitForSeconds(timeStunned);

        arcadeKart.baseStats.TopSpeed += initialSpeed * percentOfStun;
    }
    
}
