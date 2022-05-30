using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    [Header("Dialog")]
    [SerializeField] GameObject dialogBounds;

    [Header("Texto")]
    [SerializeField] TextMeshProUGUI falaAtual;
    [SerializeField] string[] falas = new string[1];

    [Header("Fade Out")]
    [SerializeField] RectTransform[] elementos = new RectTransform[4];

    [Header("Aparecer Elementos?")]
    [SerializeField] bool aparecer;
    [SerializeField] RectTransform[] elementosPraAparecer = new RectTransform[1];
    [SerializeField] int qualFala;

    [Header("Sumir Elementos?")]
    [SerializeField] bool sumir;
    [SerializeField] RectTransform[] elementosPraSumir = new RectTransform[1];
    [SerializeField] int qualFala2;

    [Header("Mudar de Cena?")]
    [SerializeField] bool mudar;
    [SerializeField] string nomeDaProximaCena;

    [Header("Batalha?")]
    [SerializeField] bool batalha1;
    [SerializeField] bool batalha2;
    [SerializeField] bool morreu;
    [SerializeField] bool ganhou;

    int numeroDaFalaAtual;

    void Start()
    {
        if (morreu) {
            FindObjectOfType<AudioManager>().StopAll();
        }

        if (ganhou) {
            FindObjectOfType<AudioManager>().StopAll();
            FindObjectOfType<AudioManager>().Play("Menu");
        }

        numeroDaFalaAtual = 0;
        falaAtual.text = falas[numeroDaFalaAtual];
    }

    public void Proximo()
    {
        numeroDaFalaAtual++;

        if (numeroDaFalaAtual < falas.Length) {
            falaAtual.text = falas[numeroDaFalaAtual];
            AparecerElementos();
            SumirElementos();
        } else {
            if (mudar) MudarDeCena();
            else FadeOut();
        }
    }

    void MudarDeCena() {
        GameObject painel = GameObject.FindWithTag("FadeToBlack");
        painel.GetComponent<Image>().raycastTarget = true;
        LeanTween.alpha(painel.GetComponent<RectTransform>(), 1f, 1.5f).setOnComplete(MudeAgora);
    }

    void MudeAgora() {
        SceneManager.LoadScene(nomeDaProximaCena);
    }

    void AparecerElementos() {
        if (!aparecer) return;

        if (numeroDaFalaAtual == qualFala) {
            FadeInElementos();
        }
    }

    void SumirElementos() {
        if (!sumir) return;

        if (numeroDaFalaAtual == qualFala2) {
            FadeOutElementos();
        }
    }

    void FadeOutElementos() {
        foreach (RectTransform elemento in elementosPraSumir)
        {
            LeanTween.alpha(elemento, 0f, 1f);
        }
    }

    void FadeInElementos() {
        foreach (RectTransform elemento in elementosPraAparecer)
        {
            LeanTween.alpha(elemento, 1f, 1f);
        }
    }

    void FadeOut() {
        foreach (RectTransform elemento in elementos)
        {
            LeanTween.alpha(elemento, 0f, 1f);
        }

        if (aparecer) {
            foreach (RectTransform elemento in elementosPraAparecer)
            {
                LeanTween.alpha(elemento, 0f, 1f);
            }
        }

        if (batalha1) {
            FindObjectOfType<AudioManager>().StopAll();
            FindObjectOfType<AudioManager>().Play("Batalha1");
        }

        if (batalha2) {
            FindObjectOfType<AudioManager>().StopAll();
            FindObjectOfType<AudioManager>().Play("Batalha2");
        }

        StartCoroutine("Desativar");
    }

    IEnumerator Desativar() {
        yield return new WaitForSeconds(1f);
        dialogBounds.SetActive(false);
    }
}
