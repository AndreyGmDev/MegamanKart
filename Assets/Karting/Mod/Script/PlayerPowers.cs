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
    [SerializeField] GameObject napalm; // Poderes lançados.
    
    

    int numberOfPowers = 2; // Número total de poderes.
    int selectPower; // Número do poder selecionado.
    bool turnOn = false;

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
            // Seleciona o poder.
            switch (selectPower)
            {
                case 0:
                    StartCoroutine("IncressSpeed");
                    print("Speed");
                    break;
                case 1:
                    GameObject obj = Instantiate(napalm, backSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    print("napalm");
                    break;
                case 2:

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
            ArcadeKart arcadeKart = GetComponent<ArcadeKart>();
            arcadeKart.baseStats.TopSpeed *= 1.3f;

            yield return new WaitForSeconds(5);

            arcadeKart.baseStats.TopSpeed = 15f;
        }

        enabled = false; // Desativa o código esperando a próxima ativação.
    }

    void InstantiatePower(GameObject power, Transform transform)
    {
        Instantiate(power, transform.parent.position, Quaternion.identity);
    }
    
}
