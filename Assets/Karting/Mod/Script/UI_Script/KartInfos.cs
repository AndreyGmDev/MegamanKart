using KartGame.KartSystems;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(KartSelection))] // Não ocorrer erro na utilização de variáveis.
public class KartInfos : MonoBehaviour
{
    [SerializeField] Slider[] slider;
    [SerializeField] TextMeshProUGUI[] textMeshProUGUI;

    KartSelection kartSelection;
    ArcadeKart arcadeKartP1;
    ArcadeKart arcadeKartP2;
    void Start()
    {
        kartSelection = GetComponent<KartSelection>();

        ChangeValue(); // Chama a função de alterar valores da UI.
    }

    public void ChangeValue()
    {
        // Pega o gameObject do kart selecionado.
        arcadeKartP1 = kartSelection.realKart[kartSelection.kartSelectedP1 - 1].GetComponent<ArcadeKart>();
        arcadeKartP2 = kartSelection.realKart[kartSelection.kartSelectedP2 - 1].GetComponent<ArcadeKart>();

        // Estatísticas dos karts do P1.
        slider[0].value = arcadeKartP1.baseStats.TopSpeed;
        slider[1].value = arcadeKartP1.DriftControl;
        slider[2].value = arcadeKartP1.baseStats.Acceleration;

        // Nome dos karts do P1.
        textMeshProUGUI[0].text = arcadeKartP1.gameObject.name;

        // Estatísticas dos karts do P2.
        slider[3].value = arcadeKartP2.baseStats.TopSpeed;
        slider[4].value = arcadeKartP2.DriftControl;
        slider[5].value = arcadeKartP2.baseStats.Acceleration;

        // Nome dos karts do P2.
        textMeshProUGUI[1].text = arcadeKartP2.gameObject.name;
    }
}
