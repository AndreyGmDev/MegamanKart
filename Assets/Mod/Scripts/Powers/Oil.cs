using KartGame.KartSystems;
using System.Collections;
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

        playerPowers.oilPercentOfStun = percentOfStun;

        StartCoroutine("Stunned");

        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<BoxCollider>().enabled = false;

        Destroy(gameObject, timeStunned + 1);
    }

    IEnumerator Stunned()
    {
        GetComponent<AudioSource>().enabled = true;

        float initialSpeed = playerPowers.initialTopSpeed;
        float initialReverseSpeed = playerPowers.initialReverseSpeed;

        arcadeKart.Rigidbody.linearVelocity *= 1 - ((initialSpeed * percentOfStun) / arcadeKart.baseStats.TopSpeed);
        arcadeKart.baseStats.TopSpeed -= initialSpeed * percentOfStun;
        arcadeKart.baseStats.ReverseSpeed -= playerPowers.initialReverseSpeed * percentOfStun;

        yield return new WaitForSeconds(timeStunned);

        arcadeKart.baseStats.TopSpeed += initialSpeed * percentOfStun;
        arcadeKart.baseStats.ReverseSpeed += initialReverseSpeed * percentOfStun;

        GetComponent<AudioSource>().enabled = false;
    }
    
}
