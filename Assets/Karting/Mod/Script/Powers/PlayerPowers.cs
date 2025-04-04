using KartGame.KartSystems;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] GameObject oil; // Poder Oil.

    [Header("Input")]
    public string inputToUsePower;

    ArcadeKart arcadeKart;
    RaceObjectives raceObjectives;
    [SerializeField] Flash[] flash = new Flash[2];
    
    const int numberOfPowers = 4; // Número total de poderes.
    int selectPower; // Número do poder selecionado.
    [HideInInspector] public float initialSpeed;
    
    private void Start()
    {
        if (GetComponent<ArcadeKart>() != null)
        {
            arcadeKart = GetComponent<ArcadeKart>();
            initialSpeed = arcadeKart.baseStats.TopSpeed;
        }

        raceObjectives = FindAnyObjectByType<RaceObjectives>();

        for (int i = 0; i < flash.Length; i++)
        {
            flash[i] = GameObject.Find("InterfaceP" + (i+1)).GetComponent<Flash>();
        }
        
    }

    // Ativado quando player pega o coletável no cenário.
    void OnEnable()
    {
        // Randomiza um poder.
        selectPower = UnityEngine.Random.Range(0, numberOfPowers);
        //selectPower = 3;
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
                    print("Speed");
                    break;
                case 1:
                    obj = Instantiate(napalm, frontSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    print("napalm");
                    break;
                case 2:
                    obj = Instantiate(piao, backSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    print("piao");
                    break;
                case 3:
                    Flashing();
                    print("Flashing");
                    break;
                case 4:
                    /*obj = Instantiate(oil, backSpawn.transform.position, Quaternion.identity);
                    obj.transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                    print("Oil");*/
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

    void Flashing()
    {
        string playerStringNumber = Regex.Replace(gameObject.name, @"[^\d]", ""); // Retira todos os caracteres exceto números.
        int playerNumber = (Convert.ToInt32(playerStringNumber)) - 1;

        for (int i = 0; i < raceObjectives.positions.Length; i++)
        {
            if (raceObjectives.positions[i] > raceObjectives.positions[playerNumber])
            {
                    flash[i].timeFlashed = 5;
            }
        }
    }
}
