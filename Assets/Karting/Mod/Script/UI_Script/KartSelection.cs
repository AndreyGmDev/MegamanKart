using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(KartInfos))]
public class KartSelection : MonoBehaviour
{
    [SerializeField] Button nextKartP1;
    [SerializeField] Button previousKartP1;
    [SerializeField] Button nextKartP2;
    [SerializeField] Button previousKartP2;

    [SerializeField] GameObject[] kartsP1;
    [SerializeField] GameObject[] kartsP2;
    public GameObject[] realKart;

    [SerializeField] Image readyP1;
    [SerializeField] Image readyP2;
    [SerializeField] Image letsGo1;
    [SerializeField] Image letsGo2;

    Vector2 letsGoPosition1 = new Vector2(-1442, 0);
    Vector2 letsGoPosition2 = new Vector2(1442, 0);

    [HideInInspector] public int kartSelectedP1 = 1;
    [HideInInspector] public int kartSelectedP2 = 1;

    bool passScene;

    [Header("ScriptableObject")]
    [SerializeField] KartSelected kartSelected;

    KartInfos kartInfos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        nextKartP1.onClick.AddListener(() => ChangeKartP1("Next"));
        previousKartP1.onClick.AddListener(() => ChangeKartP1("Previous"));
        nextKartP2.onClick.AddListener(() => ChangeKartP2("Next"));
        previousKartP2.onClick.AddListener(() => ChangeKartP2("Previous"));
        InteractButtonP1();
        InteractButtonP2();
    }

    private void Start()
    {
        kartInfos = GetComponent<KartInfos>();
    }

    // Seleciona o kart do P1.
    void ChangeKartP1(string order)
    {
        // Pega o indíce do kartP1 selecionado a partir dos botões.
        kartSelectedP1 += order == "Next" ? 1 : -1;
        kartSelectedP1 = Mathf.Clamp(kartSelectedP1, 1, kartsP1.Length);

        // Chama a função de mostrar na tela o kartP1 selecionado.
        ShowKartSelectedP1();

        // Chama a função de conferir se o botão ainda é interativo.
        InteractButtonP1();

        // Troca o valor das informações do kartP1 na UI.
        kartInfos.ChangeValue();
    }

    // Mostra o kart do P1 selecionado.
    void ShowKartSelectedP1()
    {
        // Desativa todos os karts do P1.
        foreach (var kart in kartsP1)
        {
            kart.SetActive(false);

            Transform transformKart = kart.GetComponent<Transform>();
            transformKart.eulerAngles = new Vector3(0,45,0);
        }

        // Ativa o kart selecionado do P1.
        kartsP1[kartSelectedP1 - 1].SetActive(true);
    }

    // Seleciona o kart do P2.
    void ChangeKartP2(string order)
    {
        // Pega o indíce do kartP2 selecionado a partir dos botões.
        kartSelectedP2 += order == "Next" ? 1 : -1;
        kartSelectedP2 = Mathf.Clamp(kartSelectedP2, 1, kartsP2.Length);

        // Chama a função de mostrar na tela o kartP2 selecionado.
        ShowKartSelectedP2();

        // Chama a função de conferir se o botão ainda é interativo.
        InteractButtonP2();

        // Troca o valor das informações do kartP2 na UI.
        kartInfos.ChangeValue();
    }

    // Mostra o kart do P2 selecionado.
    void ShowKartSelectedP2()
    {
        // Desativa todos os karts do P2.
        foreach (var kart in kartsP2)
        {
            kart.SetActive(false);

            Transform transformKart = kart.GetComponent<Transform>();
            transformKart.eulerAngles = new Vector3(0, 45, 0);
        }

        // Ativa o kart selecionado do P2.
        kartsP2[kartSelectedP2 - 1].SetActive(true);
    }

    // Interação dos botões do P1.
    void InteractButtonP1()
    {
        nextKartP1.interactable = kartSelectedP1 < kartsP1.Length;
        previousKartP1.interactable = kartSelectedP1 > 1;
    }

    // Interação dos botões do P1.
    void InteractButtonP2()
    {
        nextKartP2.interactable = kartSelectedP2 < kartsP2.Length;
        previousKartP2.interactable = kartSelectedP2 > 1;
    }

    private void Update()
    {
        // Para o funcionamento de qualquer Input quando a animação de passar de cena começar.
        if (letsGo1.enabled && letsGo2.enabled) return;

        // Inputs Gerais.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Volta para o menu.
            SceneManager.LoadScene("IntroMenu");
        }

        // Inputs P1.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (readyP1.enabled)
            {
                // Confere quais botões podem ser interagidos pelo P1.
                InteractButtonP1();

                // Desativa a imagem do readyP1.
                readyP1.enabled = false;
            }
            else
            {
                // Desativa a interatividade com os botões do P1.
                nextKartP1.interactable = false;
                previousKartP1.interactable = false;

                // Ativa a imagem do readyP1.
                readyP1.enabled = true;
            }
        }

        // Inputs P2.
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (readyP2.enabled)
            {
                // Confere quais botões podem ser interagidos pelo P2.
                InteractButtonP2();

                // Desativa a imagem do readyP2.
                readyP2.enabled = false;
            }
            else
            {
                // Desativa a interatividade com os botões do P2.
                nextKartP2.interactable = false;
                previousKartP2.interactable = false;

                // Ativa a imagem do readyP2.
                readyP2.enabled = true;
            }

        }

        // Chama a coroutine de passar de cena quando os dois karts forem selecionados.
        if (readyP1.enabled && readyP2.enabled)
        {
            StartCoroutine("GoingToLevel");
        }
    }
    private void FixedUpdate()
    {
        Transform turnKartP1 = kartsP1[kartSelectedP1 - 1].GetComponent<Transform>();
        turnKartP1.Rotate(0, 180 * Time.fixedDeltaTime, 0);

        Transform turnKartP2 = kartsP2[kartSelectedP2 - 1].GetComponent<Transform>();
        turnKartP2.Rotate(0, 180 * Time.fixedDeltaTime, 0);

        if (passScene)
        {
            letsGoPosition1 += new Vector2(1, 0) * Time.fixedDeltaTime * 3500;
            letsGoPosition1 = new Vector2(Mathf.Clamp(letsGoPosition1.x, -1442, -479), 0);
            letsGo1.GetComponent<RectTransform>().localPosition = letsGoPosition1;

            letsGoPosition2 += new Vector2(-1, 0) * Time.fixedDeltaTime * 3500;
            letsGoPosition2 = new Vector2(Mathf.Clamp(letsGoPosition2.x,479, 1442), 0);
            letsGo2.GetComponent<RectTransform>().localPosition = letsGoPosition2;
        }
    }

    IEnumerator GoingToLevel()
    {
        letsGo1.enabled = true;
        letsGo2.enabled = true;

        kartSelected.P1 = realKart[kartSelectedP1 - 1];
        kartSelected.P2 = realKart[kartSelectedP2 - 1];
       
        yield return new WaitForSeconds(0.8f);

        passScene = true;

        yield return new WaitForSeconds(2);

        SceneManager.LoadScene("MegamanKart");
    }

}
