using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OnePlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] GameObject player1;
    [SerializeField] Player2 player2;

    [Header("Setas")]
    [SerializeField] GameObject setaP1;
    [SerializeField] GameObject setaP2;

    [Header("Placar")]
    [SerializeField] TextMeshProUGUI placar1;
    [SerializeField] TextMeshProUGUI placar2;

    [Header("Ganhou!")]
    [SerializeField] GameObject venceu1;
    [SerializeField] GameObject venceu2;

    [Header("Peças de Preview")]
    [SerializeField] GameObject[] pecasPreview = new GameObject[7];

    int pontos1;
    int pontos2;
    bool completouAlgo;
    bool fim;

    void Start()
    {
        fim = false;
        completouAlgo = false;
        player1.SetActive(true);
        setaP1.SetActive(true);
        setaP2.SetActive(false);
        DesligaBolinhas();
        pontos1 = 0;
        pontos2 = 0;
    }

    public void Completou() {
        completouAlgo = true;
    }

    public void MaisPontos (int player, int qtd) {
        if (fim) return;

        switch (player)
        {
            case 1: pontos1 += qtd; placar1.text = pontos1.ToString(); break;
            case 2: pontos2 += qtd; placar2.text = pontos2.ToString(); break;
            default: break;
        }

        if (pontos1 >= 10) {
            venceu1.SetActive(true);
            FindObjectOfType<Tabuleiro>().enabled = false;
            FindObjectOfType<Player2>().enabled = false;
            fim = true;
        }

        if (pontos2 >= 10) {
            venceu2.SetActive(true);
            FindObjectOfType<Tabuleiro>().enabled = false;
            FindObjectOfType<Player2>().enabled = false;
            fim = true;
        }
    }

    public void VezDoPlayer1() {
        player1.SetActive(false);
        DesligaBolinhas();
        StartCoroutine("Espera1");
    }

    public void VezDoPlayer2() {
        player1.SetActive(false);
        DesligaBolinhas();
        StartCoroutine("Espera2");
    }

    IEnumerator Espera1 () {
        yield return new WaitForSeconds(1f);
        
        while (completouAlgo) {
            completouAlgo = false;
            yield return new WaitForSeconds(2f);
        }
        
        player1.SetActive(true);
        setaP1.SetActive(true);
        setaP2.SetActive(false);
    }

    IEnumerator Espera2 () {
        yield return new WaitForSeconds(1f);

        while (completouAlgo) {
            completouAlgo = false;
            yield return new WaitForSeconds(2.25f);
        }

        player1.SetActive(false);
        player2.EscolhendoUmaColuna();
        setaP1.SetActive(false);
        setaP2.SetActive(true);
        VezDoPlayer1();
    }

    void DesligaBolinhas() {
        foreach (GameObject preview in pecasPreview)
        {
            preview.SetActive(false);
        }
    }
}
