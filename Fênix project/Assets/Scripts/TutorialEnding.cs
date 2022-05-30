using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialEnding : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject dialogo2;

    void Start()
    {
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0f, 2f).setOnComplete(Continua);
        if(tutorial != null) tutorial.SetActive(false);
    }

    void Continua() {
        dialogo2.SetActive(true);
    }
}
