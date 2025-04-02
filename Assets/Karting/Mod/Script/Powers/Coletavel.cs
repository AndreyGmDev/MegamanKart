using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Coletavel : MonoBehaviour
{
    [SerializeField] float timeToReapear = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ativa o som.
            if (GetComponent<AudioSource>() != null)
                GetComponent<AudioSource>().enabled = true;

            // Ativa o poder do player que pegou o coletável.
            if (other.GetComponent<PlayerPowers>() != null)
            {
                PlayerPowers power = other.GetComponent<PlayerPowers>();
                power.enabled = true;
            }

            StartCoroutine("Cooldown");
        }
    }

    IEnumerator Cooldown()
    {
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Coletavel>().enabled = false;

        yield return new WaitForSeconds(timeToReapear);

        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Coletavel>().enabled = true;
    }
}
