using KartGame.KartSystems;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(KartSelection))] // Não ocorrer erro na utilização de variáveis.
public class KartInfos : MonoBehaviour
{
    [SerializeField] Slider[] slider;

    KartSelection kartSelection;
    ArcadeKart arcadeKartP1;
    ArcadeKart arcadeKartP2;
    void Start()
    {
        kartSelection = GetComponent<KartSelection>();

        ChangeValue();
    }

    public void ChangeValue()
    {
        arcadeKartP1 = kartSelection.realKart[kartSelection.kartSelectedP1 - 1].GetComponent<ArcadeKart>();
        arcadeKartP2 = kartSelection.realKart[kartSelection.kartSelectedP2 - 1].GetComponent<ArcadeKart>();

        slider[0].value = arcadeKartP1.baseStats.TopSpeed;
        slider[1].value = arcadeKartP1.DriftControl;
        slider[2].value = arcadeKartP1.baseStats.Acceleration;

        slider[3].value = arcadeKartP2.baseStats.TopSpeed;
        slider[4].value = arcadeKartP2.DriftControl;
        slider[5].value = arcadeKartP2.baseStats.Acceleration;
    }
}
