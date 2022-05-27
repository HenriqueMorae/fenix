using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peca : MonoBehaviour
{
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
}
