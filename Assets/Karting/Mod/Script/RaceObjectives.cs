using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceObjectives : MonoBehaviour
{
    [SerializeField] KartSelected kartSelected;

    [SerializeField] int lapsToComplete;

    int currentLapsP1 = 0;
    int currentLapsP2 = 0;

    public void CountLapsPerPlayer(GameObject player)
    {
        if (player.name == "P1")
        {
            currentLapsP1++;
        }
        else if (player.name == "P2")
        {
            currentLapsP2++;
        }
        else
            return;

        int lapsRemainingP1 = lapsToComplete - currentLapsP1;
        int lapsRemainingP2 = lapsToComplete - currentLapsP2;

        if (lapsRemainingP1 <= 0 || lapsRemainingP2 <= 0)
        {
            SceneManager.LoadScene("WinScene");
        }
        
    }
}
