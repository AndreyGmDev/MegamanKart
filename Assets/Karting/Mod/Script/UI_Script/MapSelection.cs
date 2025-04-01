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
        lavaMap.onClick.AddListener(() => mapSelected = "LavaMap");
        desertMap.onClick.AddListener(() => mapSelected = "DesertMap");
    }

    // Seleciona o Mapa.
    void ChangeMap(string map)
    {
       
        
    }

    void InteractButton()
    {
        lavaMap.interactable = false;
        desertMap.interactable = false;
    }

    private void Update()
    {
        // Para o funcionamento de qualquer Input quando a animação de passar de cena começar.
        if (letsGo1.enabled && letsGo2.enabled) return;

        // Chama a coroutine de passar de cena quando os dois karts forem selecionados.
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Backspace)) && mapSelected != null)
        {
            InteractButton();

            StartCoroutine("GoingToLevel");
        }
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

    IEnumerator GoingToLevel()
    {
        letsGo1.enabled = true;
        letsGo2.enabled = true;

        yield return new WaitForSeconds(0.8f);

        passScene = true;

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(mapSelected);
    }

    
}
