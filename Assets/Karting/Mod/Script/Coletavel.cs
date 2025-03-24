using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Coletavel : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Ativa o poder do player que pegou o coletável.
            if (other.GetComponent<PlayerPowers>() != null)
            {
                PlayerPowers power = other.GetComponent<PlayerPowers>();
                power.enabled = true;
            }
            Destroy(gameObject);
        }
    }
}
