using UnityEngine;

public class ActivateWnningKart : MonoBehaviour
{
    [SerializeField] WinnerKart winnerKart;
    [SerializeField] GameObject[] karts;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameObject winner = GameObject.Find(winnerKart.winnerKart);
        
        foreach (GameObject kart in karts)
        {
            if (kart != winner)
            {
                kart.SetActive(false);
            }
        }
    }

    
}
