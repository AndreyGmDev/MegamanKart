using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class InterfacePlayers : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI position;

    RaceObjectives raceObjectives;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        raceObjectives = FindAnyObjectByType<RaceObjectives>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.name == "InterfaceP1")
            position.text = raceObjectives.positions[0].ToString();
        else if(gameObject.name == "InterfaceP2")
            position.text = raceObjectives.positions[1].ToString();
    }
}
