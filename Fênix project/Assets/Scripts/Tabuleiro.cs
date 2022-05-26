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

    // row, column and player to verify
    int linhaAtual;
    int colunaAtual;
    int playerAtual;

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
        linhaAtual = linhaPraCair;
        colunaAtual = coluna;
        playerAtual = 1;

        GameObject pecaNova = Instantiate(peca1, colunas[coluna]);
        LeanTween.moveY(pecaNova.GetComponent<RectTransform>(), linhas[linhaPraCair].anchoredPosition.y-325f, 0.2f + linhaPraCair*0.2f).setOnComplete(ChecaAPosicao);
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
        linhaAtual = linhaPraCair;
        colunaAtual = coluna;
        playerAtual = 2;

        GameObject pecaNova = Instantiate(peca2, colunas[coluna]);
        LeanTween.moveY(pecaNova.GetComponent<RectTransform>(), linhas[linhaPraCair].anchoredPosition.y-325f, 0.2f + linhaPraCair*0.2f).setOnComplete(ChecaAPosicao);
    }

    void ChecaAPosicao() {
        List<int> figurasParaVerificar = new List<int>();
        List<int> figurasFormadas = new List<int>();

        if (playerAtual == 1)
            figurasParaVerificar = figurasNaTelaP1;
        else if (playerAtual == 2)
            figurasParaVerificar = figurasNaTelaP2;

        // verify what combinations were made
        foreach (int figuraID in figurasParaVerificar)
        {
            switch (figuraID)
            {
                case 0:
                    if (Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(0,2) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(0,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(0);
                    break;
                case 1:
                    if (Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(2,0) == playerAtual ||
                        Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual ||
                        Valor(-2,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(1);
                    break;
                case 2:
                    if (Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(-2,2) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(2,-2) == playerAtual && Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(2);
                    break;
                case 3:
                    if (Valor(0,0) == playerAtual && Valor(1,1) == playerAtual && Valor(2,2) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(-2,-2) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(3);
                    break;
                case 4:
                    if (Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,0) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(4);
                    break;
                case 5:
                    if (Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(5);
                    break;
                case 6:
                    if (Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(6);
                    break;
                case 7:
                    if (Valor(0,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(7);
                    break;
                case 8:
                    if (Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(-1,2) == playerAtual && Valor(-1,3) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(0,2) == playerAtual ||
                        Valor(1,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(1,-3) == playerAtual && Valor(0,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(8);
                    break;
                case 9:
                    if (Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(0,2) == playerAtual && Valor(0,3) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(0,2) == playerAtual ||
                        Valor(0,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(0,-3) == playerAtual && Valor(0,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(9);
                    break;
                case 10:
                    if (Valor(0,0) == playerAtual && Valor(1,1) == playerAtual && Valor(2,2) == playerAtual && Valor(3,3) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual && Valor(2,2) == playerAtual ||
                        Valor(-2,-2) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(-3,-3) == playerAtual && Valor(-2,-2) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(10);
                    break;
                case 11:
                    if (Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(-1,1) == playerAtual && Valor(-1,2) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(1,-2) == playerAtual && Valor(1,-1) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(11);
                    break;
                case 12:
                    if (Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(0,2) == playerAtual && Valor(-1,3) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual && Valor(0,2) == playerAtual ||
                        Valor(0,-2) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(1,-3) == playerAtual && Valor(0,-2) == playerAtual && Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(12);
                    break;
                case 13:
                    if (Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(2,0) == playerAtual && Valor(3,1) == playerAtual ||
                        Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(2,1) == playerAtual ||
                        Valor(-2,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(-3,-1) == playerAtual && Valor(-2,-1) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(13);
                    break;
                case 14:
                    if (Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(2,0) == playerAtual && Valor(3,0) == playerAtual ||
                        Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(2,0) == playerAtual ||
                        Valor(-2,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual ||
                        Valor(-3,0) == playerAtual && Valor(-2,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(14);
                    break;
                case 15:
                    if (Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(-2,2) == playerAtual && Valor(-3,3) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(-2,2) == playerAtual ||
                        Valor(2,-2) == playerAtual && Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(3,-3) == playerAtual && Valor(2,-2) == playerAtual && Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(15);
                    break;
                case 16:
                    if (Valor(0,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(-2,1) == playerAtual ||
                        Valor(1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,0) == playerAtual ||
                        Valor(2,-1) == playerAtual && Valor(1,-1) == playerAtual && Valor(1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(16);
                    break;
                case 17:
                    if (Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(0,2) == playerAtual && Valor(0,3) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual && Valor(1,2) == playerAtual ||
                        Valor(0,-2) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(0,-3) == playerAtual && Valor(-1,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(17);
                    break;
                case 18:
                    if (Valor(0,0) == playerAtual && Valor(1,-1) == playerAtual && Valor(2,0) == playerAtual && Valor(3,0) == playerAtual ||
                        Valor(-1,1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual && Valor(2,1) == playerAtual ||
                        Valor(-2,0) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual ||
                        Valor(-3,0) == playerAtual && Valor(-2,-1) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(18);
                    break;
                case 19:
                    if (Valor(0,0) == playerAtual && Valor(1,-1) == playerAtual && Valor(1,0) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(-1,1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(0,2) == playerAtual ||
                        Valor(-1,0) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(0,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(19);
                    break;
                case 20:
                    if (Valor(0,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(0,1) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(2,0) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(-2,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(20);
                    break;
                case 21:
                    if (Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(0,2) == playerAtual && Valor(1,2) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(0,-2) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual ||
                        Valor(-1,-2) == playerAtual && Valor(-1,-1) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(21);
                    break;
                case 22:
                    if (Valor(0,0) == playerAtual && Valor(1,1) == playerAtual && Valor(2,1) == playerAtual && Valor(3,2) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,0) == playerAtual && Valor(2,1) == playerAtual ||
                        Valor(-2,-1) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(-3,-2) == playerAtual && Valor(-2,-1) == playerAtual && Valor(-1,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(22);
                    break;
                case 23:
                    if (Valor(0,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(-2,0) == playerAtual && Valor(-2,1) == playerAtual ||
                        Valor(1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(-1,0) == playerAtual && Valor(-1,1) == playerAtual ||
                        Valor(2,0) == playerAtual && Valor(1,0) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(2,-1) == playerAtual && Valor(1,-1) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(23);
                    break;
                case 24:
                    if (Valor(0,0) == playerAtual && Valor(0,1) == playerAtual && Valor(1,0) == playerAtual && Valor(1,1) == playerAtual ||
                        Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual && Valor(1,-1) == playerAtual && Valor(1,0) == playerAtual ||
                        Valor(-1,0) == playerAtual && Valor(-1,1) == playerAtual && Valor(0,0) == playerAtual && Valor(0,1) == playerAtual ||
                        Valor(-1,-1) == playerAtual && Valor(-1,0) == playerAtual && Valor(0,-1) == playerAtual && Valor(0,0) == playerAtual)
                            figurasFormadas.Add(24);
                    break;
                default: break;
            }
        }

        // choose what combination to use
        if (figurasFormadas.Count > 0) {
            int maiorNumero = -1;

            foreach (int figura in figurasFormadas)
            {
                if (figura > maiorNumero) maiorNumero = figura;
            }

            RemoverFiguraPronta(maiorNumero);
        }
    }

    int Valor (int linha, int coluna) {
        int linhaFinal = linhaAtual + linha;
        int colunaFinal = colunaAtual + coluna;

        if (linhaFinal > 5 || linhaFinal < 0 || colunaFinal > 6 || colunaFinal < 0)
            return 0;
        else
            return tabuleiro[linhaFinal,colunaFinal];
    }

    void RemoverFiguraPronta (int qualFigura) {
        // verificar qual é a figura
        // encontrar as peças no tabuleiro
        // fazer elas ficarem vazias
    }
}
