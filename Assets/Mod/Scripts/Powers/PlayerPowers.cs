using KartGame.KartSystems;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPowers : MonoBehaviour
{
    [Header("Spawn Power Positions")]
    [SerializeField] Transform frontSpawn;
    [SerializeField] Transform piaoSpawn;


    [Header("Powers Prefab")]
    [SerializeField] GameObject napalm; // Poder Napalm.
    [SerializeField] GameObject piao; // Poder Pìão.
    [SerializeField] GameObject oil; // Poder Oil.

    [Header("Input")]
    public string inputToUsePower; // Input para usar os poderes.

    [Header("References")]
    [SerializeField] Material normalMaterial;
    [SerializeField] Material lavaMaterial;
    [SerializeField] GameObject[] wheels;
    ArcadeKart arcadeKart;
    RaceObjectives raceObjectives;
    Flash[] flash = new Flash[2];

    [Header("Statistics Of Power")]
    [Min(0), Tooltip("Poncentagem que deixa o player mais rápido, Exemplo: speed = speed + speed * percentOfIncressSpeed")]
    public float percentOfIncressSpeed = 0.3f;

    [Tooltip("Tempo que os players adversários ficam cegos")]
    [SerializeField] float timeFlashed = 3;

    [SerializeField] float timeStunned = 3;

    const int numberOfPowers = 5; // Número total de poderes.
    public int selectPower; // Número do poder selecionado.
    [HideInInspector] public float initialTopSpeed; // Valor de velocidade máxima do player.
    [HideInInspector] public float initialReverseSpeed; // Valor de velocidade reversa máxima do player.
    [HideInInspector] public float oilPercentOfStun; // Pega do script Oil, quando o player passa por cima do OilPrefab;

    private void Start()
    {
        if (GetComponent<ArcadeKart>() != null)
        {
            arcadeKart = GetComponent<ArcadeKart>();
            initialTopSpeed = arcadeKart.baseStats.TopSpeed;
            initialReverseSpeed = arcadeKart.baseStats.ReverseSpeed;
        }

        raceObjectives = FindAnyObjectByType<RaceObjectives>();

        for (int i = 0; i < flash.Length; i++)
        {
            flash[i] = GameObject.Find("InterfaceP" + (i+1)).GetComponent<Flash>();
        }
        
        enabled = false;
    }

    // Ativado quando player pega o coletável no cenário.
    void OnEnable()
    {
        // Randomiza um poder.
        selectPower = UnityEngine.Random.Range(0, numberOfPowers);
    }

    
    private void Update()
    {
        // Ativa o poder.
        if (Input.GetButtonDown(inputToUsePower))
        {
            GameObject obj;

            // Seleciona o poder.
            switch (selectPower)
            {
                case 0:
                    StopCoroutine("IncressSpeed"); // Para todas a coroutine IncressSpeed anterior, se houver, para evitar interromper na nova.
                    StartCoroutine("IncressSpeed"); // Inicia coroutine de aumentar velocidade do Player.
                    break;
                case 1:
                    obj = Instantiate(napalm, frontSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    break;
                case 2:
                    obj = Instantiate(piao, piaoSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    break;
                case 3:
                    Flashing();
                    break;
                case 4:
                    obj = Instantiate(oil, piaoSpawn.transform.position - transform.forward, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    break;
                case 5:

                    break;
            }

           enabled = false;
        }

    }

    public IEnumerator IncressSpeed()
    {
        if (GetComponent<ArcadeKart>() != null)
        {
            // Confere se o carro está sendo afetado por algum poder, ou seja, se sofreu alguma perda na velocidade máxima.
            if (arcadeKart.baseStats.TopSpeed >= initialTopSpeed)
            {
                arcadeKart.baseStats.TopSpeed += initialTopSpeed * percentOfIncressSpeed;
                arcadeKart.baseStats.TopSpeed = Mathf.Clamp(arcadeKart.baseStats.TopSpeed, 0, (1 + percentOfIncressSpeed) * initialTopSpeed);

                arcadeKart.baseStats.ReverseSpeed += initialReverseSpeed * percentOfIncressSpeed;
                arcadeKart.baseStats.ReverseSpeed = Mathf.Clamp(arcadeKart.baseStats.ReverseSpeed, 0, (1 + percentOfIncressSpeed) * initialReverseSpeed);
            }
            else
            {
                arcadeKart.baseStats.TopSpeed += initialTopSpeed * percentOfIncressSpeed;
                arcadeKart.baseStats.TopSpeed = Mathf.Clamp(arcadeKart.baseStats.TopSpeed, 0, ((1 - oilPercentOfStun) * initialTopSpeed) + (percentOfIncressSpeed * initialTopSpeed));

                arcadeKart.baseStats.ReverseSpeed += initialReverseSpeed * percentOfIncressSpeed;
                arcadeKart.baseStats.ReverseSpeed = Mathf.Clamp(arcadeKart.baseStats.ReverseSpeed, 0, ((1 - oilPercentOfStun) * initialReverseSpeed) + percentOfIncressSpeed * initialReverseSpeed);
            }

            if (lavaMaterial != null)
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].GetComponent<Renderer>().material = lavaMaterial;
                }
            }
            
            yield return new WaitForSeconds(5);

            arcadeKart.Rigidbody.linearVelocity *= 1 - ((initialTopSpeed * percentOfIncressSpeed) / arcadeKart.baseStats.TopSpeed);
            arcadeKart.baseStats.TopSpeed -= initialTopSpeed * percentOfIncressSpeed;
            arcadeKart.baseStats.ReverseSpeed -= initialReverseSpeed * percentOfIncressSpeed;

            if (lavaMaterial != null)
            {
                for (int i = 0; i < wheels.Length; i++)
                {
                    wheels[i].GetComponent<Renderer>().material = normalMaterial;
                }
            }
        }

    }

    public IEnumerator Teste()
    {
        arcadeKart.Rigidbody.linearVelocity *= 1 - ((initialTopSpeed * oilPercentOfStun) / arcadeKart.baseStats.TopSpeed);

        // Confere se o carro está sendo afetado por algum poder, ou seja, se sofreu alguma perda na velocidade máxima.
        if (arcadeKart.baseStats.TopSpeed <= initialTopSpeed)
        {
            arcadeKart.baseStats.TopSpeed -= initialTopSpeed * oilPercentOfStun;
            arcadeKart.baseStats.TopSpeed = Mathf.Clamp(arcadeKart.baseStats.TopSpeed, (1 - oilPercentOfStun) * initialTopSpeed, arcadeKart.baseStats.TopSpeed);

            arcadeKart.baseStats.ReverseSpeed -= initialReverseSpeed * oilPercentOfStun;
            arcadeKart.baseStats.ReverseSpeed = Mathf.Clamp(arcadeKart.baseStats.ReverseSpeed, (1 - oilPercentOfStun) * initialReverseSpeed, arcadeKart.baseStats.ReverseSpeed);
        }
        else
        {
            arcadeKart.baseStats.TopSpeed -= initialTopSpeed * oilPercentOfStun;
            arcadeKart.baseStats.TopSpeed = Mathf.Clamp(arcadeKart.baseStats.TopSpeed, ((1 - oilPercentOfStun) * initialTopSpeed) + (percentOfIncressSpeed * initialTopSpeed), arcadeKart.baseStats.TopSpeed);

            arcadeKart.baseStats.ReverseSpeed -= initialReverseSpeed * oilPercentOfStun;
            arcadeKart.baseStats.ReverseSpeed = Mathf.Clamp(arcadeKart.baseStats.ReverseSpeed, ((1 - oilPercentOfStun) * initialReverseSpeed) + percentOfIncressSpeed * initialReverseSpeed, arcadeKart.baseStats.ReverseSpeed);
        }

        yield return new WaitForSeconds(timeStunned);

        arcadeKart.baseStats.TopSpeed += initialTopSpeed * oilPercentOfStun;
        arcadeKart.baseStats.ReverseSpeed += initialReverseSpeed * oilPercentOfStun;
    }

    void Flashing()
    {
        if (flash.Any(x => x == null)) return;

        string playerStringNumber = Regex.Replace(gameObject.name, @"[^\d]", ""); // Retira todos os caracteres exceto números.
        int playerNumber = (Convert.ToInt32(playerStringNumber)) - 1;

        for (int i = 0; i < raceObjectives.positions.Length; i++)
        {
            if (raceObjectives.positions[i] > raceObjectives.positions[playerNumber])
            {
                flash[i].timeFlashed = timeFlashed;
            }
        }
    }
}
