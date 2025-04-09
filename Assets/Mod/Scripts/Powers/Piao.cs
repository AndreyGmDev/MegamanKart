using System.Collections;
using KartGame.KartSystems;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Piao : MonoBehaviour
{
    [Header("Transform")]
    [SerializeField] float initialScale = 0.1f;
    [SerializeField] float maxScale = 0.34f;
    [SerializeField] float turnVelocity = 360;

    [Header("Habilities")]
    [SerializeField] float speed = 30;
    [SerializeField] float kartRotation = 720;
    [SerializeField] float timerRotation = 1;

    GameObject player;
    bool canTurn = false;
    bool canScale = false;
    Vector3 scale;
    Vector3 right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine("IncressScale");
        scale = new Vector3(1,1,1) * initialScale;
    }

    IEnumerator IncressScale()
    {
        canScale = true;

        yield return new WaitForSeconds(0.7f);

        canScale = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
         transform.Rotate(0, turnVelocity * Time.fixedDeltaTime, 0);
        transform.Translate(Vector3.right * Time.fixedDeltaTime * speed);

        if (canTurn)
            player.GetComponent<Transform>().Rotate(0, kartRotation * Time.fixedDeltaTime, 0);

        if (canScale)
        {
            scale += new Vector3(1, 1, 1) * Time.fixedDeltaTime * 0.6f;
            scale = new Vector3(Mathf.Clamp(scale.x, 0.1f, 0.34f), Mathf.Clamp(scale.y, 0.1f, 0.34f), Mathf.Clamp(scale.z, 0.1f, 0.34f));
            GetComponent<Transform>().transform.localScale = scale;
        }
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

        // Desativa o objeto.
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<AudioSource>().enabled = false;
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

    
}
