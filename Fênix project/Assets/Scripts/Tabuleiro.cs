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

    bool esperandoAnimacoes;

    // meaning of spaces:
    // 0 -> empty
    // 1 -> player / player 1
    // 2 -> enemy / player 2

    [Header("Peças dos Jogadores")]
    [SerializeField] GameObject peca1;
    [SerializeField] GameObject peca2;

    [Header("Multiplayer?")]
    [SerializeField] bool multiplayer;

    [Header("Puzzle?")]
    [SerializeField] bool puzzle;

    [Header("Espaços do Tabuleiro")]
    [SerializeField] RectTransform[] colunas = new RectTransform[7];
    [SerializeField] RectTransform[] linhas = new RectTransform[6];

    void Awake()
    {
        esperandoAnimacoes = false;
        figurasNaTelaP1.Clear();
        figurasNaTelaP2.Clear();

        if (multiplayer)
            FindObjectOfType<OnePlayerInput>().enabled = false;
        else
            FindObjectOfType<TwoPlayerInput>().enabled = false;

        // making all spaces empty
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                tabuleiro[j,i] = 0;
            }
        }
    }

    public int[,] TabuleiroNow() {
        return tabuleiro;
    }

    public List<int> FigurasP1() {
        return figurasNaTelaP1;
    }

    public List<int> FigurasP2() {
        return figurasNaTelaP2;
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
        pecaNova.GetComponent<Peca>().OndeEstou(linhaPraCair, coluna);
        LeanTween.moveY(pecaNova.GetComponent<RectTransform>(), linhas[linhaPraCair].anchoredPosition.y-325f, 0.1f + linhaPraCair*0.1f).setOnComplete(ChecaAPosicao);
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
        pecaNova.GetComponent<Peca>().OndeEstou(linhaPraCair, coluna);
        LeanTween.moveY(pecaNova.GetComponent<RectTransform>(), linhas[linhaPraCair].anchoredPosition.y-325f, 0.1f + linhaPraCair*0.1f).setOnComplete(ChecaAPosicao);
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

    // remove a combination
    void RemoverFiguraPronta (int qualFigura) {
        switch (qualFigura)
        {
            case 0: ProcurandoAFigura3(new int[] {0,0,0,1,0,2,0,-1,0,0,0,1,0,-2,0,-1,0,0}); break;
            case 1: ProcurandoAFigura3(new int[] {0,0,1,0,2,0,-1,0,0,0,1,0,-2,0,-1,0,0,0}); break;
            case 2: ProcurandoAFigura3(new int[] {0,0,-1,1,-2,2,1,-1,0,0,-1,1,2,-2,1,-1,0,0}); break;
            case 3: ProcurandoAFigura3(new int[] {0,0,1,1,2,2,-1,-1,0,0,1,1,-2,-2,-1,-1,0,0}); break;
            case 4: ProcurandoAFigura3(new int[] {0,0,0,1,-1,1,0,-1,0,0,-1,0,1,-1,1,0,0,0}); break;
            case 5: ProcurandoAFigura3(new int[] {0,0,1,0,1,1,-1,0,0,0,0,1,-1,-1,0,-1,0,0}); break;
            case 6: ProcurandoAFigura3(new int[] {0,0,0,1,1,1,0,-1,0,0,1,0,-1,-1,-1,0,0,0}); break;
            case 7: ProcurandoAFigura3(new int[] {0,0,-1,0,-1,1,1,0,0,0,0,1,1,-1,0,-1,0,0}); break;
            case 8: ProcurandoAFigura4(new int[] {-1,1,-1,2,-1,3,1,-1,0,1,0,2,1,-2,0,-1,0,1,1,-3,0,-2,0,-1}); break;
            case 9: ProcurandoAFigura4(new int[] {0,1,0,2,0,3,0,-1,0,1,0,2,0,-2,0,-1,0,1,0,-3,0,-2,0,-1}); break;
            case 10: ProcurandoAFigura4(new int[] {1,1,2,2,3,3,-1,-1,1,1,2,2,-2,-2,-1,-1,1,1,-3,-3,-2,-2,-1,-1}); break;
            case 11: ProcurandoAFigura4(new int[] {0,1,-1,1,-1,2,0,-1,-1,0,-1,1,1,-1,1,0,0,1,1,-2,1,-1,0,-1}); break;
            case 12: ProcurandoAFigura4(new int[] {-1,1,0,2,-1,3,1,-1,1,1,0,2,0,-2,-1,-1,-1,1,1,-3,0,-2,1,-1}); break;
            case 13: ProcurandoAFigura4(new int[] {1,0,2,0,3,1,-1,0,1,0,2,1,-2,0,-1,0,1,1,-3,-1,-2,-1,-1,-1}); break;
            case 14: ProcurandoAFigura4(new int[] {1,0,2,0,3,0,-1,0,1,0,2,0,-2,0,-1,0,1,0,-3,0,-2,0,-1,0}); break;
            case 15: ProcurandoAFigura4(new int[] {-1,1,-2,2,-3,3,1,-1,-1,1,-2,2,2,-2,1,-1,-1,1,3,-3,2,-2,1,-1}); break;
            case 16: ProcurandoAFigura4(new int[] {-1,0,-1,1,-2,1,1,0,0,1,-1,1,1,-1,0,-1,-1,0,2,-1,1,-1,1,0}); break;
            case 17: ProcurandoAFigura4(new int[] {-1,1,0,2,0,3,1,-1,1,1,1,2,0,-2,-1,-1,0,1,0,-3,-1,-2,0,-1}); break;
            case 18: ProcurandoAFigura4(new int[] {1,-1,2,0,3,0,-1,1,1,1,2,1,-2,0,-1,-1,1,0,-3,0,-2,-1,-1,0}); break;
            case 19: ProcurandoAFigura4(new int[] {1,-1,1,0,1,1,-1,1,0,1,0,2,-1,0,0,-1,0,1,-1,-1,0,-2,0,-1}); break;
            case 20: ProcurandoAFigura4(new int[] {-1,1,0,1,1,1,1,-1,1,0,2,0,0,-1,-1,0,1,0,-1,-1,-2,0,-1,0}); break;
            case 21: ProcurandoAFigura4(new int[] {0,1,0,2,1,2,0,-1,0,1,1,1,0,-2,0,-1,1,0,-1,-2,-1,-1,-1,0}); break;
            case 22: ProcurandoAFigura4(new int[] {1,1,2,1,3,2,-1,-1,1,0,2,1,-2,-1,-1,0,1,1,-3,-2,-2,-1,-1,-1}); break;
            case 23: ProcurandoAFigura4(new int[] {-1,0,-2,0,-2,1,1,0,-1,0,-1,1,-2,0,1,0,0,1,-2,-1,1,-1,0,-1}); break;
            case 24: ProcurandoAFigura4(new int[] {0,1,1,0,1,1,0,-1,1,-1,1,0,-1,0,-1,1,0,1,-1,-1,-1,0,0,-1}); break;
            default: break;
        }

        // give points on a multiplayer match
        if (qualFigura > 7 && multiplayer && !puzzle) FindObjectOfType<TwoPlayerInput>().Dano(playerAtual,4);
        else if (qualFigura <= 7 && multiplayer && !puzzle) FindObjectOfType<TwoPlayerInput>().Dano(playerAtual,3);

        if (qualFigura > 7 && !multiplayer && !puzzle) FindObjectOfType<OnePlayerInput>().Dano(playerAtual,4);
        else if (qualFigura <= 7 && !multiplayer && !puzzle) FindObjectOfType<OnePlayerInput>().Dano(playerAtual,3);

        // complete the puzzle on a puzzle section
        if (puzzle) FindObjectOfType<OnePlayerInput>().PuzzleConcluido();

        if (multiplayer) FindObjectOfType<TwoPlayerInput>().Completou();
        if (!multiplayer) FindObjectOfType<OnePlayerInput>().Completou();
        StartCoroutine("Continua");        
    }

    IEnumerator Continua() {
        yield return new WaitForSeconds(1.75f);
        ReposicionarPecas();
    }

    void ProcurandoAFigura3 (int[] i) {
        if (Valor(i[0],i[1]) == playerAtual && Valor(i[2],i[3]) == playerAtual && Valor(i[4],i[5]) == playerAtual) {
            Tira(i[0],i[1]); Tira(i[2],i[3]); Tira(i[4],i[5]);
        } else if (Valor(i[6],i[7]) == playerAtual && Valor(i[8],i[9]) == playerAtual && Valor(i[10],i[11]) == playerAtual) {
            Tira(i[6],i[7]); Tira(i[8],i[9]); Tira(i[10],i[11]);
        } else if (Valor(i[12],i[13]) == playerAtual && Valor(i[14],i[15]) == playerAtual && Valor(i[16],i[17]) == playerAtual) {
            Tira(i[12],i[13]); Tira(i[14],i[15]); Tira(i[16],i[17]);
        }
    }

    void ProcurandoAFigura4 (int[] i) {
        if (Valor(0,0) == playerAtual && Valor(i[0],i[1]) == playerAtual && Valor(i[2],i[3]) == playerAtual && Valor(i[4],i[5]) == playerAtual) {
            Tira(0,0); Tira(i[0],i[1]); Tira(i[2],i[3]); Tira(i[4],i[5]);
        } else if (Valor(i[6],i[7]) == playerAtual && Valor(0,0) == playerAtual && Valor(i[8],i[9]) == playerAtual && Valor(i[10],i[11]) == playerAtual) {
            Tira(i[6],i[7]); Tira(0,0); Tira(i[8],i[9]); Tira(i[10],i[11]);
        } else if (Valor(i[12],i[13]) == playerAtual && Valor(i[14],i[15]) == playerAtual && Valor(0,0) == playerAtual && Valor(i[16],i[17]) == playerAtual) {
            Tira(i[12],i[13]); Tira(i[14],i[15]); Tira(0,0); Tira(i[16],i[17]);
        } else if (Valor(i[18],i[19]) == playerAtual && Valor(i[20],i[21]) == playerAtual && Valor(i[22],i[23]) == playerAtual && Valor(0,0) == playerAtual) {
            Tira(i[18],i[19]); Tira(i[20],i[21]); Tira(i[22],i[23]); Tira(0,0);
        }
    }

    void Tira (int linha, int coluna) {
        int linhaFinal = linhaAtual + linha;
        int colunaFinal = colunaAtual + coluna;

        tabuleiro[linhaFinal,colunaFinal] = 0;
        
        int[] posicaoNoTabuleiro = new int[2];

        foreach (Peca peca in FindObjectsOfType<Peca>())
        {
            posicaoNoTabuleiro = peca.Posicao();

            if (posicaoNoTabuleiro[0] == linhaFinal && posicaoNoTabuleiro[1] == colunaFinal) {
                peca.CompletouFigura();
            }
        }
    }

    // deal with holes in the board
    void ReposicionarPecas() {
        int buraco;
        bool acheiPotencialBuraco;

        for (int i = 0; i < 7; i++)
        {
            buraco = -1;
            acheiPotencialBuraco = false;

            for (int j = 5; j >= 0 ; j--)
            {
                if (tabuleiro[j,i] == 0 && !acheiPotencialBuraco) {
                    buraco = j;
                    acheiPotencialBuraco = true;
                } else if (tabuleiro[j,i] != 0 && acheiPotencialBuraco) {
                    tabuleiro[buraco,i] = tabuleiro[j,i];
                    tabuleiro[j,i] = 0;

                    int[] posicaoNoTabuleiro = new int[2];
                    foreach (Peca peca in FindObjectsOfType<Peca>())
                    {
                        posicaoNoTabuleiro = peca.Posicao();

                        if (posicaoNoTabuleiro[0] == j && posicaoNoTabuleiro[1] == i) {
                            peca.OndeEstou(buraco,i);
                            LeanTween.moveY(peca.GetComponent<RectTransform>(), linhas[buraco].anchoredPosition.y-325f, (buraco-j)*0.1f);
                        }
                    }

                    i--;
                    j = -1;
                }
            }
        }
        esperandoAnimacoes = true;
    }

    void Update() {
        if (esperandoAnimacoes && LeanTween.tweensRunning == 0) {
            esperandoAnimacoes = false;

            if (playerAtual == 1) playerAtual = 2;
            else if (playerAtual == 2) playerAtual = 1;

            bool acabou = false;
            
            for (int i = 0; i < 7; i++)
            {
                if (acabou) break;

                for (int j = 0; j < 6; j++)
                {
                    if (acabou) break;

                    if (tabuleiro[j,i] == playerAtual) {
                        linhaAtual = j;
                        colunaAtual = i;

                        if (ApenasChecaAPosicao()) acabou = true;
                    }
                }
            }

            if (acabou) {
                ChecaAPosicao();
            } else {
                if (playerAtual == 1) playerAtual = 2;
                else if (playerAtual == 2) playerAtual = 1;

                for (int i = 0; i < 7; i++)
                {
                    if (acabou) break;

                    for (int j = 0; j < 6; j++)
                    {
                        if (acabou) break;

                        if (tabuleiro[j,i] == playerAtual) {
                            linhaAtual = j;
                            colunaAtual = i;

                            if (ApenasChecaAPosicao()) acabou = true;
                        }
                    }
                }

                if(acabou) ChecaAPosicao();
            }
        }
    }

    // function to just check if there is a combination
    bool ApenasChecaAPosicao () {
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

        if (figurasFormadas.Count > 0) {
            return true;
        } else {
            return false;
        }
    }
}
