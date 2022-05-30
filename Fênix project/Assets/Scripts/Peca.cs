using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Peca : MonoBehaviour
{
    [Header("Completo")]
    [SerializeField] Image completo;

    int colunaOndeEstou;
    int linhaOndeEstou;

    // the piece knows where it is on the board
    public void OndeEstou (int linha, int coluna) {
        linhaOndeEstou = linha;
        colunaOndeEstou = coluna;
    }

    public int[] Posicao () {
        int[] position = new int[2];
        position[0] = linhaOndeEstou;
        position[1] = colunaOndeEstou;
        return position;
    }

    public void CompletouFigura() {
        completo.color = Color.white;
        LeanTween.alpha(gameObject.GetComponent<RectTransform>(), 0f, 1f).setOnComplete(Sumindo);
    }

    void Sumindo() {
        Destroy(gameObject);
    }
}
