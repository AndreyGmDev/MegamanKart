using Cinemachine;
using KartGame.KartSystems;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnKartSelected : MonoBehaviour
{
    GameObject kartP1;
    GameObject kartP2;
    [SerializeField] Transform spawnPointP1;
    [SerializeField] Transform spawnPointP2;
    [SerializeField] KartSelected kartSelected;
    [SerializeField] GameObject CamP1;
    [SerializeField] GameObject CamP2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (kartSelected.P1 == null || kartSelected.P2 == null) return;

        // Pega os karts da cena de seleção.
        kartP1 = kartSelected.P1;
        kartP2 = kartSelected.P2;

        // Spawn karts.
        GameObject P1 = Instantiate(kartP1,spawnPointP1.transform.position, Quaternion.Euler(0, spawnPointP1.eulerAngles.y, 0));
        GameObject P2 = Instantiate(kartP2,spawnPointP2.transform.position, Quaternion.Euler(0, spawnPointP2.eulerAngles.y, 0));

        // Nomes são usados no Script RaceObjectives para saber as voltas dadas por cada kart. 
        // Esses nomes estão sendo utilizados nos Scripts(RaceObjectives, CountLaps, PlayerPowers). Cuidado ao alterar.
        P1.name = "P1"; // Altera o nome do kart do Player 1 para P1. 
        P2.name = "P2"; // Altera o nome do kart do Player 2 para P2.

        // Altera os Inputs do Player 1 e Player 2.
        P1.GetComponent<KeyboardInput>().TurnInputName = "HorizontalP1";
        P1.GetComponent<KeyboardInput>().AccelerateButtonName = "VerticalUpP1";
        P1.GetComponent<KeyboardInput>().BrakeButtonName = "VerticalDownP1";
        P1.GetComponent<PlayerPowers>().inputToUsePower = "UsePowerP1";

        P2.GetComponent<KeyboardInput>().TurnInputName = "HorizontalP2";
        P2.GetComponent<KeyboardInput>().AccelerateButtonName = "VerticalUpP2";
        P2.GetComponent<KeyboardInput>().BrakeButtonName = "VerticalDownP2";
        P2.GetComponent<PlayerPowers>().inputToUsePower = "UsePowerP2";

        // Coloca as câmeras para seguirem os karts.
        CamP1.GetComponent<CinemachineVirtualCamera>().Follow = P1.transform;
        CamP1.GetComponent<CinemachineVirtualCamera>().LookAt = P1.transform;
        CamP2.GetComponent<CinemachineVirtualCamera>().Follow = P2.transform;
        CamP2.GetComponent<CinemachineVirtualCamera>().LookAt = P2.transform;
    }

}
