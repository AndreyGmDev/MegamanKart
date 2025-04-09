using UnityEngine;

public class Laser : MonoBehaviour
{
    Vector3 initialForward;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialForward = transform.forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(Vector3.forward * Time.deltaTime);
        print(transform.forward);
    }

}
