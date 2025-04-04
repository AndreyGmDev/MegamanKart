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

        float initialSpeed = arcadeKart.baseStats.TopSpeed;
        
        arcadeKart.baseStats.TopSpeed = initialSpeed * percentOfStun;

        yield return new WaitForSeconds(timeStunned);

        arcadeKart.baseStats.TopSpeed = initialSpeed;
    }
    //Vector3 newVelocity = new Vector3(Mathf.Clamp(arcadeKart.Rigidbody.linearVelocity.x, 0, initialSpeed), arcadeKart.Rigidbody.linearVelocity.y, Mathf.Clamp(arcadeKart.Rigidbody.linearVelocity.z, 0, initialSpeed));
}
