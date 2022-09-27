using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoPlayerInput : MonoBehaviour
{
    [Header("Inputs")]
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    [Header("Setas")]
    [SerializeField] GameObject setaP1;
    [SerializeField] GameObject setaP2;

    [Header("Vida")]
    [SerializeField] Slider vida1;
    [SerializeField] Slider vida2;

    [Header("Coisas Pra Piscar Vermelho")]
    [SerializeField] RectTransform[] imagensPraPiscarP1 = new RectTransform[1];
    [SerializeField] RectTransform[] imagensPraPiscarP2 = new RectTransform[1];

    [Header("Ganhou!")]
    [SerializeField] GameObject venceu1;
    [SerializeField] GameObject venceu2;

    [Header("Peças de Preview")]
    [SerializeField] GameObject[] pecasPreview = new GameObject[14];

    int health1;
    int health2;
    bool completouAlgo;
    bool fim;

    void Start()
    {
        fim = false;
        completouAlgo = false;
        player1.SetActive(true);
        player2.SetActive(false);
        setaP1.SetActive(true);
        setaP2.SetActive(false);
        DesligaBolinhas();
        health1 = 10;
        health2 = 10;
    }

    public void Completou() {
        completouAlgo = true;
    }

    // this player did this damage
    public void Dano (int player, int qtd) {
        if (fim) return;

        switch (player)
        {
            case 1: health2 -= qtd; vida2.value = health2/10f; PiscaVermelhoP2(); break;
            case 2: health1 -= qtd; vida1.value = health1/10f; PiscaVermelhoP1(); break;
            default: break;
        }

        if (health2 <= 0) {
            venceu1.SetActive(true);
            FindObjectOfType<Tabuleiro>().enabled = false;
            fim = true;
        }

        if (health1 <= 0) {
            venceu2.SetActive(true);
            FindObjectOfType<Tabuleiro>().enabled = false;
            fim = true;
        }
    }

    void PiscaVermelhoP1() {
        foreach (RectTransform imagem in imagensPraPiscarP1)
        {
            imagem.GetComponent<Image>().color = Color.red;
            LeanTween.color(imagem, Color.white, 1f);
        }
    }

    void PiscaVermelhoP2() {
        foreach (RectTransform imagem in imagensPraPiscarP2)
        {
            imagem.GetComponent<Image>().color = Color.red;
            LeanTween.color(imagem, Color.white, 1f);
        }
    }

    public void VezDoPlayer1() {
        player1.SetActive(false);
        player2.SetActive(false);
        DesligaBolinhas();
        StartCoroutine("Espera1");
    }

    public void VezDoPlayer2() {
        player1.SetActive(false);
        player2.SetActive(false);
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
        player2.SetActive(false);
        setaP1.SetActive(true);
        setaP2.SetActive(false);
    }

    IEnumerator Espera2 () {
        yield return new WaitForSeconds(1f);

        while (completouAlgo) {
            completouAlgo = false;
            yield return new WaitForSeconds(2f);
        }

        player1.SetActive(false);
        player2.SetActive(true);
        setaP1.SetActive(false);
        setaP2.SetActive(true);
    }

    void DesligaBolinhas() {
        foreach (GameObject preview in pecasPreview)
        {
            preview.SetActive(false);
        }
    }
}
