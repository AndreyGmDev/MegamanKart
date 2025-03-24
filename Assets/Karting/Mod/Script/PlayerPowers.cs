using KartGame.KartSystems;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerPowers : MonoBehaviour
{
    [Header("Spawn Power Positions")]
    [SerializeField] Transform frontSpawn;
    [SerializeField] Transform leftSpawn;
    [SerializeField] Transform rightSpawn;
    [SerializeField] Transform backSpawn;

    [Header("Powers Prefab")]
    [SerializeField] GameObject napalm; // Poder Napalm.
    [SerializeField] GameObject piao; // Poder Pìão.

    ArcadeKart arcadeKart;

    int numberOfPowers = 3; // Número total de poderes.
    int selectPower; // Número do poder selecionado.
    bool turnOn = false;
    float initialSpeed;
    private void Start()
    {
        if (GetComponent<ArcadeKart>() != null)
        {
            arcadeKart = GetComponent<ArcadeKart>();
            initialSpeed = arcadeKart.baseStats.TopSpeed;
        }
    }
    // Ativado quando player pega o coletável no cenário.
    void OnEnable()
    {
        // Randomiza um poder.
        selectPower = Random.Range(0, numberOfPowers);
        turnOn = true;
    }

    
    private void Update()
    {
        // Ativa o poder.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject obj;

            // Seleciona o poder.
            switch (selectPower)
            {
                case 0:
                    StopCoroutine("IncressSpeed");
                    StartCoroutine("IncressSpeed");
                    print("Speed");
                    break;
                case 1:
                    obj = Instantiate(napalm, frontSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    print("napalm");
                    break;
                case 2:
                    obj = Instantiate(piao, backSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);
                    print("piao");
                    break;
                case 3:

                    break;
                case 4:

                    break;
                case 5:

                    break;
            }

           enabled = false;
        }
    }
    
    IEnumerator IncressSpeed()
    {
        if (GetComponent<ArcadeKart>() != null)
        {
            arcadeKart.baseStats.TopSpeed = 1.3f * initialSpeed;

            yield return new WaitForSeconds(5);

            arcadeKart.baseStats.TopSpeed = initialSpeed;
        }
        
        enabled = false; // Desativa o código esperando a próxima ativação.
    }

    void InstantiatePower(GameObject power, GameObject spawnObject)
    {
        GameObject obj = Instantiate(power, spawnObject.transform.position, Quaternion.identity);
        obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
    }
    
}
