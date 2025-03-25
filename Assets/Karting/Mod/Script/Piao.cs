using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Piao : MonoBehaviour
{
    GameObject player;
    bool canTurn = false;
    bool canScale = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("IncressScale");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(0, 360f * Time.fixedDeltaTime, 0);

        if (canTurn)
            player.GetComponent<Transform>().Rotate(0, 720 * Time.fixedDeltaTime, 0);

        if (canScale)
            GetComponent<Transform>().transform.localScale += new Vector3(1, 1, 1) * Time.fixedDeltaTime * 0.4f;
    }



    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            print("afeta");
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


        collision.GetComponent<Rigidbody>().isKinematic = true; // Desativa a movimentação do player acertado.
        canTurn = true; // Faz o player acertado ficar girando.

        yield return new WaitForSeconds(1f); // Tempo do player acertado fica girando.

        collision.GetComponent<Rigidbody>().isKinematic = false;
        canTurn = false;

        yield return new WaitForSeconds(2f); // Espera o som terminar de tocar.

        Destroy(gameObject); // Destrói o objeto depois de colidir.
    }

    IEnumerator IncressScale()
    {
        float timer = 0;
        canScale = true;
        while (timer < 0.25f)
        {   
            timer += Time.deltaTime;
            yield return new WaitForNextFrameUnit();
        }

        canScale = false;
    }
}
