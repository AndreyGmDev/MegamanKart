using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Napalm : MonoBehaviour
{
    [SerializeField] float speed;

    GameObject player;
    Rigidbody rigidbody;
    Vector3 forward;
    bool canTurn = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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
            player.GetComponent<Transform>().Rotate(0, 720 * Time.fixedDeltaTime, 0);
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
        // Desativa o objeto.
        GetComponent<MeshRenderer>().enabled = false;
        foreach (var cld in GetComponents<Collider>())
            cld.enabled = false;

        player = collision.gameObject;

        collision.GetComponent<Rigidbody>().isKinematic = true;

        canTurn = true;

        yield return new WaitForSeconds(1f);

        collision.GetComponent<Rigidbody>().isKinematic = false;

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
