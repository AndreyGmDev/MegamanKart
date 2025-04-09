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
    Coroutine coroutine;
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

        if(coroutine != null)
            StopCoroutine(coroutine);

        coroutine = StartCoroutine(playerPowers.Teste());
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!other.isTrigger) return;

        if (arcadeKart.Rigidbody.linearVelocity.magnitude > 0.1)
        {
            GetComponent<AudioSource>().enabled = true;
        }
        else
        {
            GetComponent<AudioSource>().enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (!other.isTrigger) return;

        GetComponent<AudioSource>().enabled = false;
    }

    /*IEnumerator Stunned()
    {
        float initialSpeed = playerPowers.initialTopSpeed;
        float initialReverseSpeed = playerPowers.initialReverseSpeed;

        arcadeKart.Rigidbody.linearVelocity *= 1 - ((initialSpeed * percentOfStun) / arcadeKart.baseStats.TopSpeed);
        arcadeKart.baseStats.TopSpeed -= initialSpeed * percentOfStun;

        // Confere se o carro está sendo afetado por algum poder, ou seja, se sofreu alguma perda na velocidade máxima.
        if (arcadeKart.baseStats.TopSpeed <= initialSpeed)
        {
            arcadeKart.baseStats.TopSpeed -= initialSpeed * percentOfStun;
            arcadeKart.baseStats.TopSpeed = Mathf.Clamp(arcadeKart.baseStats.TopSpeed, (1 - percentOfStun) * initialSpeed, arcadeKart.baseStats.TopSpeed);

            arcadeKart.baseStats.ReverseSpeed -= playerPowers.initialReverseSpeed * percentOfStun;
            arcadeKart.baseStats.ReverseSpeed = Mathf.Clamp(arcadeKart.baseStats.ReverseSpeed, (1 - percentOfStun) * initialReverseSpeed, arcadeKart.baseStats.ReverseSpeed);
        }
        else
        {
            arcadeKart.baseStats.TopSpeed -= initialSpeed * percentOfStun;
            arcadeKart.baseStats.TopSpeed = Mathf.Clamp(arcadeKart.baseStats.TopSpeed, ((1 - percentOfStun) * initialSpeed) + (playerPowers.percentOfIncressSpeed * initialSpeed), arcadeKart.baseStats.TopSpeed);

            arcadeKart.baseStats.ReverseSpeed -= initialReverseSpeed * percentOfStun;
            arcadeKart.baseStats.ReverseSpeed = Mathf.Clamp(arcadeKart.baseStats.ReverseSpeed, ((1 - percentOfStun) * initialSpeed) + playerPowers.percentOfIncressSpeed * initialReverseSpeed, arcadeKart.baseStats.ReverseSpeed);
        }

        yield return new WaitForSeconds(timeStunned);

        arcadeKart.baseStats.TopSpeed += initialSpeed * percentOfStun;
        arcadeKart.baseStats.ReverseSpeed += initialReverseSpeed * percentOfStun;
    }*/
    
}
