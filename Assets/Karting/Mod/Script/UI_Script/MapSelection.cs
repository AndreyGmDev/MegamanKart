using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSelection : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] Button lavaMap;
    [SerializeField] Button desertMap;

    [Header("Images")]
    [SerializeField] Image letsGo1;
    [SerializeField] Image letsGo2;

    Vector2 letsGoPosition1 = new Vector2(-1442, 0);
    Vector2 letsGoPosition2 = new Vector2(1442, 0);

    bool passScene;

    string mapSelected;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        lavaMap.onClick.AddListener(() => StartCoroutine(GoingToLevel("LavaMap",lavaMap)));
        desertMap.onClick.AddListener(() => StartCoroutine(GoingToLevel("DesertMap", lavaMap)));
    }

    void InteractButton()
    {
        lavaMap.interactable = false;
        desertMap.interactable = false;
    }

    private void FixedUpdate()
    {
        if (passScene)
        {
            letsGoPosition1 += new Vector2(1, 0) * Time.fixedDeltaTime * 3500;
            letsGoPosition1 = new Vector2(Mathf.Clamp(letsGoPosition1.x, -1442, -479), 0);
            letsGo1.GetComponent<RectTransform>().localPosition = letsGoPosition1;

            letsGoPosition2 += new Vector2(-1, 0) * Time.fixedDeltaTime * 3500;
            letsGoPosition2 = new Vector2(Mathf.Clamp(letsGoPosition2.x, 479, 1442), 0);
            letsGo2.GetComponent<RectTransform>().localPosition = letsGoPosition2;
        }
    }

    IEnumerator GoingToLevel(string map, Button button)
    {
        InteractButton();

        letsGo1.enabled = true;
        letsGo2.enabled = true;

        yield return new WaitForSeconds(0.8f);

        passScene = true;

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(map);
    }

    
}
