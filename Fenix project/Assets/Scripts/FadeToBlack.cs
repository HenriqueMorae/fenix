using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeToBlack : MonoBehaviour
{
    [Header("Fade Element")]
    [SerializeField] GameObject elemento;

    void Start()
    {
        LeanTween.alpha(elemento.GetComponent<RectTransform>(), 0f, 1.5f).setOnComplete(Fim);
    }

    void Fim() {
        elemento.GetComponent<Image>().raycastTarget = false;
    }
}
