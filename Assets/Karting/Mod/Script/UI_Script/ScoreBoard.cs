using System.Collections;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{

    Animator scoreBoard;
    [SerializeField] WinnerKart winnerKart;
    [SerializeField] TextMeshProUGUI firstKart;
    [SerializeField] TextMeshProUGUI secondKart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreBoard = GetComponent<Animator>();

        StartCoroutine("ActivateTransition");

        if (winnerKart.winnerKart != null)
        {
            firstKart.text = winnerKart.winnerKart;
        }

        if (winnerKart.secondKart != null)
        {
            secondKart.text = winnerKart.secondKart;
        }
        
    }

    IEnumerator ActivateTransition()
    {
        yield return new WaitForSeconds(3);

        scoreBoard.enabled = true;
    }
}
