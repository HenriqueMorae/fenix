using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tabuleiro : MonoBehaviour
{
    // variable that will control the game board
    int[,] tabuleiro = new int[6,7];

    // list of combinations on screen for each player
    List<int> figurasNaTelaP1 = new List<int>();
    List<int> figurasNaTelaP2 = new List<int>();

    // meaning of spaces:
    // 0 -> empty
    // 1 -> player / player 1
    // 2 -> enemy / player 2

    [Header("Peças dos Jogadores")]
    [SerializeField] GameObject peca1;
    [SerializeField] GameObject peca2;

    [Header("Espaços do Tabuleiro")]
    [SerializeField] RectTransform[] colunas = new RectTransform[7];
    [SerializeField] RectTransform[] linhas = new RectTransform[6];

    void Awake()
    {
        figurasNaTelaP1.Clear();
        figurasNaTelaP2.Clear();

        // making all spaces empty
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                tabuleiro[j,i] = 0;
            }
        }
    }

    // the combinations on screen will be added here
    public void AdicionandoFigura (int player, int id) {
        if (player == 1)
            figurasNaTelaP1.Add(id);

        if (player == 2)
            figurasNaTelaP2.Add(id);
    }

    // to avoid repetition
    public bool ExisteFigura (int id) {
        bool repetiu = false;

        if (figurasNaTelaP1.Contains(id) || figurasNaTelaP2.Contains(id))
            repetiu = true;
        
        return repetiu;
    }

    public void ColocaPecaPlayer1 (int coluna) {
        // if it is full, you cannot put a piece here
        if (tabuleiro[0,coluna] != 0) return;

        int linhaPraCair = 0;

        // if there is space, the piece will "fall" in the row with the highest number
        for (int i = 0; i < 6; i++)
        {
            if (tabuleiro[i,coluna] == 0) linhaPraCair = i;
        }

        tabuleiro[linhaPraCair,coluna] = 1;

        GameObject pecaNova = Instantiate(peca1, colunas[coluna]);
        LeanTween.moveY(pecaNova.GetComponent<RectTransform>(), linhas[linhaPraCair].anchoredPosition.y-325f, 0.2f + linhaPraCair*0.2f);
    }

    public void ColocaPecaPlayer2 (int coluna) {
        // if it is full, you cannot put a piece here
        if (tabuleiro[0,coluna] != 0) return;

        int linhaPraCair = 0;

        // if there is space, the piece will "fall" in the row with the highest number
        for (int i = 0; i < 6; i++)
        {
            if (tabuleiro[i,coluna] == 0) linhaPraCair = i;
        }

        tabuleiro[linhaPraCair,coluna] = 2;

        GameObject pecaNova = Instantiate(peca2, colunas[coluna]);
        LeanTween.moveY(pecaNova.GetComponent<RectTransform>(), linhas[linhaPraCair].anchoredPosition.y-325f, 0.2f + linhaPraCair*0.2f);
    }
}
