using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    [SerializeField] Image flashImage; // Referência do Flash.
    [HideInInspector] public float timeFlashed; // Tempo do Flash ativo.
    
    private void Update()
    {
        // Deixa o player cego enquanto timeFlashed > 0.
        if (timeFlashed > 0)
            flashImage.enabled = true;
        else
            flashImage.enabled = false;

        timeFlashed -= Time.deltaTime;
    }
    
}
