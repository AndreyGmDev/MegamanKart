using UnityEngine;

public class SpawnKartSelected : MonoBehaviour
{
    GameObject kartP1;
    GameObject kartP2;
    [SerializeField] Transform spawnPointP1;
    [SerializeField] Transform spawnPointP2;
    [SerializeField] KartSelected kartSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (kartSelected.P1 == null || kartSelected.P2 == null) return;

        // Pega os karts da cena de seleção.
        kartP1 = kartSelected.P1;
        kartP2 = kartSelected.P2;

        // Spawn karts.
        Instantiate(kartP1,spawnPointP1.transform.position,Quaternion.Euler(0,180,0));
        Instantiate(kartP2,spawnPointP2.transform.position, Quaternion.Euler(0, 180, 0));
    }

}
