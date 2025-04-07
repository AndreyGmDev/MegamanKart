using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterfacePlayers : MonoBehaviour
{
    
    [SerializeField] TextMeshProUGUI position;
    [SerializeField] Image power;
    [SerializeField] Sprite[] powers;
    [SerializeField] Sprite withoutPower;
    RaceObjectives raceObjectives;
    PlayerPowers playerPowers;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raceObjectives = FindAnyObjectByType<RaceObjectives>();
        
        if (gameObject.name == "InterfaceP1")
        {
            playerPowers = GameObject.Find("P1").GetComponent<PlayerPowers>();
        }
        else if (gameObject.name == "InterfaceP2")
        {
            playerPowers = GameObject.Find("P2").GetComponent<PlayerPowers>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (power == null) return;

        if (gameObject.name == "InterfaceP1")
            position.text = raceObjectives.positions[0].ToString();
        else if(gameObject.name == "InterfaceP2")
            position.text = raceObjectives.positions[1].ToString();

        if (playerPowers.enabled && powers.All(s => s != null))
        {
            print("Ativo");
            power.sprite = powers[playerPowers.selectPower];
        }
        else if (withoutPower != null)
            power.sprite = withoutPower;
    }
}
