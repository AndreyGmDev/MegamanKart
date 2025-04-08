using KartGame.KartSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceObjectives : MonoBehaviour
{
    [SerializeField] KartSelected kartSelected;
    [SerializeField] int lapsToComplete;

    public int[] currentLaps;

    [HideInInspector] public GameObject[] closestWaypointOfPlayer;
    [HideInInspector] public float[] distanceClosestWaypoint;

    [HideInInspector] public GameObject[] players = new GameObject[2];
    [HideInInspector] public int[] positions;

    [SerializeField] WinnerKart winnerKart; // Passa o kart vencedor da corrida para a WinScene.

    [SerializeField] Animator transitionToWinScene;
    private void Start()
    {
        positions = new int[players.Length];
        currentLaps = new int[players.Length];
        closestWaypointOfPlayer = new GameObject[players.Length];
        distanceClosestWaypoint = new float[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            players[i] = GameObject.Find("P" + (i + 1));
            closestWaypointOfPlayer[i] = null;
        }
    }
    public void CountLapsPerPlayer(GameObject player, int value)
    {
        GameObject winner = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (player.name == "P" + (i + 1))
            {
                currentLaps[i] += value;
            }

            // Se um player completou o número total de voltas.
            if (currentLaps[i] >= lapsToComplete)
            {
                players[i].GetComponent<ArcadeKart>().enabled = false;

                foreach (var kart in players)
                {
                    if (kart != winner)
                    {
                        winnerKart.secondKart = players[i].transform.GetChild(0).name;
                    }
                }
                
                if (winner != null) continue;

                winner = players[i];
                winnerKart.winnerKart = winner.transform.GetChild(0).name;
            }
        }

        // Se todos os players completarem o total de voltas, a corrida é finalizada.
        if (currentLaps.All(x => lapsToComplete - x <= 0))
        {
            StartCoroutine("WinScene");
        }
    }

    IEnumerator WinScene()
    {
        yield return new WaitForSeconds(1);

        if (transitionToWinScene != null)
        {
            transitionToWinScene.enabled = true;
        }

        yield return new WaitForSeconds(1);

        // Direciona para o mapa de vitória referente.
        string map = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(map + "WinScene");
    }

    private void Update()
    {
        if (closestWaypointOfPlayer.Any(waypoint => waypoint == null)) return;

        CalculateDistance();
    }
    
    void CalculateDistance()
    {
        // dicionário para armazenar a posição dos jogadores em cada waypoint
        Dictionary<int, List<float>> waypointAndDistance = new Dictionary<int, List<float>>();
        Dictionary<int, List<int>> waypointAndPlayers = new Dictionary<int, List<int>>();
        Dictionary<int, Dictionary<int, List<float>>> lapsAndWaypointAndDistance = new Dictionary<int, Dictionary<int, List<float>>>();

        // Preenche o dicionário de waypoints com as distâncias dos jogadores
        for (int i = 0; i < positions.Length; i++)
        {
            int waypoint = Convert.ToInt32(closestWaypointOfPlayer[i].name);
            float distance = distanceClosestWaypoint[i];

            // Adiciona as distâncias para cada waypoint
            if (!waypointAndDistance.ContainsKey(waypoint))
            {
                waypointAndDistance[waypoint] = new List<float>();
                waypointAndPlayers[waypoint] = new List<int>();
            }

            waypointAndDistance[waypoint].Add(distance);
            waypointAndPlayers[waypoint].Add(i); // Armazena o índice do jogador
        }

        // Preenche o dicionário com as voltas, waypoints e distâncias dos jogadores.
        for (int i = 0; i < positions.Length; i++)
        {
            int lap = currentLaps[i];
            int waypoint = Convert.ToInt32(closestWaypointOfPlayer[i].name);

            if (!lapsAndWaypointAndDistance.ContainsKey(lap))
            {
                lapsAndWaypointAndDistance[lap] = new Dictionary<int, List<float>>();
            }

            /*if (!lapsAndWaypointAndDistance[lap].ContainsKey(waypoint))
            {
                // Criando uma nova lista apenas para essa volta e waypoint
                lapsAndWaypointAndDistance[lap][waypoint] = new List<float>(waypointAndDistance[waypoint]);
            }*/

            if (!lapsAndWaypointAndDistance[lap].ContainsKey(waypoint))
            {
                lapsAndWaypointAndDistance[lap].Add(waypoint, waypointAndDistance[waypoint]);
            }
            
        }

        // Organiza as voltas em ordem decrescente.
        var sortedLaps = lapsAndWaypointAndDistance.Keys.OrderByDescending(k => k).ToList();

        // Lista para armazenar os jogadores e sua posição.
        List<(int playerIndex, int rank)> playersPosition = new List<(int, int)>();

        int rank = 1;

        foreach (var lap in sortedLaps)
        {
            var waypoints = lapsAndWaypointAndDistance[lap].Keys.OrderByDescending(x => x);

            foreach (var waypoint in waypoints)
            {

                var sortedDistances = lapsAndWaypointAndDistance[lap][waypoint].
                Select((distance, index) => new { distance, playerIndex = waypointAndPlayers[waypoint][index]}).
                OrderBy(x => x.distance).
                ToList();

                // Para cada waypoint, ordenamos os jogadores pela distância crescente
                /*var sortedDistances = waypointAndDistance[waypoint]
                    .Select((distance, index) => new { distance, playerIndex = waypointAndPlayers[waypoint][index] })
                    .OrderBy(x => x.distance).ToList();*/

                foreach (var player in sortedDistances)
                {
                    playersPosition.Add((player.playerIndex, rank)); // Adiciona as posições dos jogadores.
                    rank++;
                }
            }
        }

        // Atualiza o array de posições de acordo com as posições calculadas.
        foreach (var playerRank in playersPosition)
        {
            positions[playerRank.playerIndex] = playerRank.rank;
        }
    }
   
}
