using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    [Header("Tabuleiro")]
    [SerializeField] Tabuleiro board;

    // the board that the player 2 will see to make a decision
    int[,] tabuleiroDeAgora = new int[6,7];

    // list of combinations on screen for each player
    List<int> figurasNaTelaP1 = new List<int>();
    List<int> figurasNaTelaP2 = new List<int>();

    int linhaAtual;
    int colunaAtual;
    int playerAtual;
    int[] queroJogarAqui = new int[7];

    public void EscolhendoUmaColuna() {
        tabuleiroDeAgora = board.TabuleiroNow();
        figurasNaTelaP1 = board.FigurasP1();
        figurasNaTelaP2 = board.FigurasP2();
        int maiorCoeficiente = -1;

        for (int i = 0; i < 7; i++)
        {
            queroJogarAqui[i] = 0;
            colunaAtual = i;
            LinhaMaisBaixa(i);

            if (tabuleiroDeAgora[0,colunaAtual] != 0) {
                queroJogarAqui[i] = -200;
                continue;
            }

            queroJogarAqui[i] += 2*FazerMinhasFiguras();
            queroJogarAqui[i] += BloquearFigurasInimigas();

            if (queroJogarAqui[i] > maiorCoeficiente) maiorCoeficiente = queroJogarAqui[i];
        }

        List<int> colunasComPotencial = new List<int>();

        for (int i = 0; i < 7; i++)
        {
            if (queroJogarAqui[i] == maiorCoeficiente) colunasComPotencial.Add(i);
        }

        FindObjectOfType<Tabuleiro>().ColocaPecaPlayer2(colunasComPotencial[Random.Range(0,colunasComPotencial.Count)]);
    }

    void LinhaMaisBaixa (int coluna) {
        linhaAtual = 0;

        for (int i = 0; i < 6; i++)
        {
            if (tabuleiroDeAgora[i,coluna] == 0) linhaAtual = i;
        }
    }

    int FazerMinhasFiguras() {
        int coeficiente = 0;
        playerAtual = 2;

        foreach (int figuraID in figurasNaTelaP2)
        {
            switch (figuraID)
            {
                case 0: coeficiente += CompletandoAFigura3(new int[] {0,0,0,1,0,2,0,-1,0,0,0,1,0,-2,0,-1,0,0}); break;
                case 1: coeficiente += CompletandoAFigura3(new int[] {0,0,1,0,2,0,-1,0,0,0,1,0,-2,0,-1,0,0,0}); break;
                case 2: coeficiente += CompletandoAFigura3(new int[] {0,0,-1,1,-2,2,1,-1,0,0,-1,1,2,-2,1,-1,0,0}); break;
                case 3: coeficiente += CompletandoAFigura3(new int[] {0,0,1,1,2,2,-1,-1,0,0,1,1,-2,-2,-1,-1,0,0}); break;
                case 4: coeficiente += CompletandoAFigura3(new int[] {0,0,0,1,-1,1,0,-1,0,0,-1,0,1,-1,1,0,0,0}); break;
                case 5: coeficiente += CompletandoAFigura3(new int[] {0,0,1,0,1,1,-1,0,0,0,0,1,-1,-1,0,-1,0,0}); break;
                case 6: coeficiente += CompletandoAFigura3(new int[] {0,0,0,1,1,1,0,-1,0,0,1,0,-1,-1,-1,0,0,0}); break;
                case 7: coeficiente += CompletandoAFigura3(new int[] {0,0,-1,0,-1,1,1,0,0,0,0,1,1,-1,0,-1,0,0}); break;
                case 8: coeficiente += CompletandoAFigura4(new int[] {-1,1,-1,2,-1,3,1,-1,0,1,0,2,1,-2,0,-1,0,1,1,-3,0,-2,0,-1}); break;
                case 9: coeficiente += CompletandoAFigura4(new int[] {0,1,0,2,0,3,0,-1,0,1,0,2,0,-2,0,-1,0,1,0,-3,0,-2,0,-1}); break;
                case 10: coeficiente += CompletandoAFigura4(new int[] {1,1,2,2,3,3,-1,-1,1,1,2,2,-2,-2,-1,-1,1,1,-3,-3,-2,-2,-1,-1}); break;
                case 11: coeficiente += CompletandoAFigura4(new int[] {0,1,-1,1,-1,2,0,-1,-1,0,-1,1,1,-1,1,0,0,1,1,-2,1,-1,0,-1}); break;
                case 12: coeficiente += CompletandoAFigura4(new int[] {-1,1,0,2,-1,3,1,-1,1,1,0,2,0,-2,-1,-1,-1,1,1,-3,0,-2,1,-1}); break;
                case 13: coeficiente += CompletandoAFigura4(new int[] {1,0,2,0,3,1,-1,0,1,0,2,1,-2,0,-1,0,1,1,-3,-1,-2,-1,-1,-1}); break;
                case 14: coeficiente += CompletandoAFigura4(new int[] {1,0,2,0,3,0,-1,0,1,0,2,0,-2,0,-1,0,1,0,-3,0,-2,0,-1,0}); break;
                case 15: coeficiente += CompletandoAFigura4(new int[] {-1,1,-2,2,-3,3,1,-1,-1,1,-2,2,2,-2,1,-1,-1,1,3,-3,2,-2,1,-1}); break;
                case 16: coeficiente += CompletandoAFigura4(new int[] {-1,0,-1,1,-2,1,1,0,0,1,-1,1,1,-1,0,-1,-1,0,2,-1,1,-1,1,0}); break;
                case 17: coeficiente += CompletandoAFigura4(new int[] {-1,1,0,2,0,3,1,-1,1,1,1,2,0,-2,-1,-1,0,1,0,-3,-1,-2,0,-1}); break;
                case 18: coeficiente += CompletandoAFigura4(new int[] {1,-1,2,0,3,0,-1,1,1,1,2,1,-2,0,-1,-1,1,0,-3,0,-2,-1,-1,0}); break;
                case 19: coeficiente += CompletandoAFigura4(new int[] {1,-1,1,0,1,1,-1,1,0,1,0,2,-1,0,0,-1,0,1,-1,-1,0,-2,0,-1}); break;
                case 20: coeficiente += CompletandoAFigura4(new int[] {-1,1,0,1,1,1,1,-1,1,0,2,0,0,-1,-1,0,1,0,-1,-1,-2,0,-1,0}); break;
                case 21: coeficiente += CompletandoAFigura4(new int[] {0,1,0,2,1,2,0,-1,0,1,1,1,0,-2,0,-1,1,0,-1,-2,-1,-1,-1,0}); break;
                case 22: coeficiente += CompletandoAFigura4(new int[] {1,1,2,1,3,2,-1,-1,1,0,2,1,-2,-1,-1,0,1,1,-3,-2,-2,-1,-1,-1}); break;
                case 23: coeficiente += CompletandoAFigura4(new int[] {-1,0,-2,0,-2,1,1,0,-1,0,-1,1,-2,0,1,0,0,1,-2,-1,1,-1,0,-1}); break;
                case 24: coeficiente += CompletandoAFigura4(new int[] {0,1,1,0,1,1,0,-1,1,-1,1,0,-1,0,-1,1,0,1,-1,-1,-1,0,0,-1}); break;
                default: break;
            }
        }

        return coeficiente;
    }

    int CompletandoAFigura3 (int[] i) {
        int coeficienteDaFigura = 0;

        if (Valor(i[2],i[3]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[4],i[5]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[2],i[3]) == playerAtual && Valor(i[4],i[5]) == playerAtual) coeficienteDaFigura += 5;
        if (Valor(i[6],i[7]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[10],i[11]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[6],i[7]) == playerAtual && Valor(i[10],i[11]) == playerAtual) coeficienteDaFigura += 5;
        if (Valor(i[12],i[13]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[14],i[15]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[12],i[13]) == playerAtual && Valor(i[14],i[15]) == playerAtual) coeficienteDaFigura += 5;

        return coeficienteDaFigura;
    }

    int CompletandoAFigura4 (int[] i) {
        int coeficienteDaFigura = 0;

        if (Valor(i[0],i[1]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[2],i[3]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[4],i[5]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[0],i[1]) == playerAtual && Valor(i[2],i[3]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[2],i[3]) == playerAtual && Valor(i[4],i[5]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[0],i[1]) == playerAtual && Valor(i[4],i[5]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[0],i[1]) == playerAtual && Valor(i[2],i[3]) == playerAtual && Valor(i[4],i[5]) == playerAtual) coeficienteDaFigura += 10;
        if (Valor(i[6],i[7]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[8],i[9]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[10],i[11]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[6],i[7]) == playerAtual && Valor(i[8],i[9]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[10],i[11]) == playerAtual && Valor(i[6],i[7]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[8],i[9]) == playerAtual && Valor(i[10],i[11]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[6],i[7]) == playerAtual && Valor(i[8],i[9]) == playerAtual && Valor(i[10],i[11]) == playerAtual) coeficienteDaFigura += 10;
        if (Valor(i[12],i[13]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[14],i[15]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[16],i[17]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[12],i[13]) == playerAtual && Valor(i[14],i[15]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[16],i[17]) == playerAtual && Valor(i[12],i[13]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[14],i[15]) == playerAtual && Valor(i[16],i[17]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[12],i[13]) == playerAtual && Valor(i[14],i[15]) == playerAtual && Valor(i[16],i[17]) == playerAtual) coeficienteDaFigura += 10;
        if (Valor(i[18],i[19]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[20],i[21]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[22],i[23]) == playerAtual) coeficienteDaFigura++;
        if (Valor(i[18],i[19]) == playerAtual && Valor(i[20],i[21]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[22],i[23]) == playerAtual && Valor(i[18],i[19]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[20],i[21]) == playerAtual && Valor(i[22],i[23]) == playerAtual) coeficienteDaFigura += 3;
        if (Valor(i[18],i[19]) == playerAtual && Valor(i[20],i[21]) == playerAtual && Valor(i[22],i[23]) == playerAtual) coeficienteDaFigura += 10;

        return coeficienteDaFigura;
    }

    int BloquearFigurasInimigas() {
        int coeficiente = 0;
        playerAtual = 1;

        foreach (int figuraID in figurasNaTelaP1)
        {
            switch (figuraID)
            {
                case 0: coeficiente += CompletandoAFigura3(new int[] {0,0,0,1,0,2,0,-1,0,0,0,1,0,-2,0,-1,0,0}); break;
                case 1: coeficiente += CompletandoAFigura3(new int[] {0,0,1,0,2,0,-1,0,0,0,1,0,-2,0,-1,0,0,0}); break;
                case 2: coeficiente += CompletandoAFigura3(new int[] {0,0,-1,1,-2,2,1,-1,0,0,-1,1,2,-2,1,-1,0,0}); break;
                case 3: coeficiente += CompletandoAFigura3(new int[] {0,0,1,1,2,2,-1,-1,0,0,1,1,-2,-2,-1,-1,0,0}); break;
                case 4: coeficiente += CompletandoAFigura3(new int[] {0,0,0,1,-1,1,0,-1,0,0,-1,0,1,-1,1,0,0,0}); break;
                case 5: coeficiente += CompletandoAFigura3(new int[] {0,0,1,0,1,1,-1,0,0,0,0,1,-1,-1,0,-1,0,0}); break;
                case 6: coeficiente += CompletandoAFigura3(new int[] {0,0,0,1,1,1,0,-1,0,0,1,0,-1,-1,-1,0,0,0}); break;
                case 7: coeficiente += CompletandoAFigura3(new int[] {0,0,-1,0,-1,1,1,0,0,0,0,1,1,-1,0,-1,0,0}); break;
                case 8: coeficiente += CompletandoAFigura4(new int[] {-1,1,-1,2,-1,3,1,-1,0,1,0,2,1,-2,0,-1,0,1,1,-3,0,-2,0,-1}); break;
                case 9: coeficiente += CompletandoAFigura4(new int[] {0,1,0,2,0,3,0,-1,0,1,0,2,0,-2,0,-1,0,1,0,-3,0,-2,0,-1}); break;
                case 10: coeficiente += CompletandoAFigura4(new int[] {1,1,2,2,3,3,-1,-1,1,1,2,2,-2,-2,-1,-1,1,1,-3,-3,-2,-2,-1,-1}); break;
                case 11: coeficiente += CompletandoAFigura4(new int[] {0,1,-1,1,-1,2,0,-1,-1,0,-1,1,1,-1,1,0,0,1,1,-2,1,-1,0,-1}); break;
                case 12: coeficiente += CompletandoAFigura4(new int[] {-1,1,0,2,-1,3,1,-1,1,1,0,2,0,-2,-1,-1,-1,1,1,-3,0,-2,1,-1}); break;
                case 13: coeficiente += CompletandoAFigura4(new int[] {1,0,2,0,3,1,-1,0,1,0,2,1,-2,0,-1,0,1,1,-3,-1,-2,-1,-1,-1}); break;
                case 14: coeficiente += CompletandoAFigura4(new int[] {1,0,2,0,3,0,-1,0,1,0,2,0,-2,0,-1,0,1,0,-3,0,-2,0,-1,0}); break;
                case 15: coeficiente += CompletandoAFigura4(new int[] {-1,1,-2,2,-3,3,1,-1,-1,1,-2,2,2,-2,1,-1,-1,1,3,-3,2,-2,1,-1}); break;
                case 16: coeficiente += CompletandoAFigura4(new int[] {-1,0,-1,1,-2,1,1,0,0,1,-1,1,1,-1,0,-1,-1,0,2,-1,1,-1,1,0}); break;
                case 17: coeficiente += CompletandoAFigura4(new int[] {-1,1,0,2,0,3,1,-1,1,1,1,2,0,-2,-1,-1,0,1,0,-3,-1,-2,0,-1}); break;
                case 18: coeficiente += CompletandoAFigura4(new int[] {1,-1,2,0,3,0,-1,1,1,1,2,1,-2,0,-1,-1,1,0,-3,0,-2,-1,-1,0}); break;
                case 19: coeficiente += CompletandoAFigura4(new int[] {1,-1,1,0,1,1,-1,1,0,1,0,2,-1,0,0,-1,0,1,-1,-1,0,-2,0,-1}); break;
                case 20: coeficiente += CompletandoAFigura4(new int[] {-1,1,0,1,1,1,1,-1,1,0,2,0,0,-1,-1,0,1,0,-1,-1,-2,0,-1,0}); break;
                case 21: coeficiente += CompletandoAFigura4(new int[] {0,1,0,2,1,2,0,-1,0,1,1,1,0,-2,0,-1,1,0,-1,-2,-1,-1,-1,0}); break;
                case 22: coeficiente += CompletandoAFigura4(new int[] {1,1,2,1,3,2,-1,-1,1,0,2,1,-2,-1,-1,0,1,1,-3,-2,-2,-1,-1,-1}); break;
                case 23: coeficiente += CompletandoAFigura4(new int[] {-1,0,-2,0,-2,1,1,0,-1,0,-1,1,-2,0,1,0,0,1,-2,-1,1,-1,0,-1}); break;
                case 24: coeficiente += CompletandoAFigura4(new int[] {0,1,1,0,1,1,0,-1,1,-1,1,0,-1,0,-1,1,0,1,-1,-1,-1,0,0,-1}); break;
                default: break;
            }
        }

        return coeficiente;
    }

    int Valor (int linha, int coluna) {
        int linhaFinal = linhaAtual + linha;
        int colunaFinal = colunaAtual + coluna;

        if (linhaFinal > 5 || linhaFinal < 0 || colunaFinal > 6 || colunaFinal < 0)
            return 0;
        else
            return tabuleiroDeAgora[linhaFinal,colunaFinal];
    }
}
