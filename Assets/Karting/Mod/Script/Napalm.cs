using System.Collections;
using KartGame.KartSystems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Napalm : MonoBehaviour
{
    [Header("Habilities")]
    [SerializeField] float speed = 20;
    [SerializeField] float kartRotation = 720;
    [SerializeField] float timerRotation = 1;

    GameObject player;
    Vector3 forward;
    bool canTurn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("Disappear");
        forward = new Vector3(0,0,1f);
        print(forward);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate (forward * Time.fixedDeltaTime * speed);
        //transform.Rotate(0f, 0f, -30f * Time.fixedDeltaTime);
        if (canTurn)
            player.GetComponent<Transform>().Rotate(0, kartRotation * Time.fixedDeltaTime, 0);
    }

    

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine("Effect", collision);
        }
    }

    IEnumerator Effect(Collider collision)
    {
        player = collision.gameObject;

        // Ativa o som.
        GetComponent<AudioSource>().enabled = true;

        // Desativa o objeto.
        GetComponent<MeshRenderer>().enabled = false;
        foreach (var cld in GetComponents<Collider>())
            cld.enabled = false;


        collision.GetComponent<ArcadeKart>().enabled = false; // Desativa a movimentação do player acertado.
        canTurn = true; // Faz o player acertado ficar girando.

        yield return new WaitForSeconds(timerRotation); // Tempo do player acertado fica girando.

        canTurn = false;
        collision.GetComponent<ArcadeKart>().enabled = true; // Ativa a movimentação do player novamente.

        yield return new WaitForSeconds(2f); // Espera o som terminar de tocar.

        Destroy(gameObject); // Destrói o objeto depois de colidir.
    }

    
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(5f);

        // Desativa os colliders pro objeto cair 5 segundos depois de ser criado.
        foreach (var cld in GetComponents<Collider>())
           cld.enabled = false;

        yield return new WaitForSeconds(2f);

        Destroy(gameObject); // Destrói o objeto 7 segundos depois de ser criado.
    }
}
