using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combination : MonoBehaviour
{
    [Header("Meio")]
    [SerializeField] Image meio;

    [Header("Tipo de Combinação")]
    [SerializeField] bool de3;
    [SerializeField] bool de4;

    [Header("Jogador")]
    [Range(1,2)]
    [SerializeField] int player;

    [Header("Combinações")]
    [SerializeField] Sprite[] combinacoes = new Sprite[25];

    int id;
    bool repetido;

    // it sends its id to the board
    void Start()
    {
        repetido = true;

        while (repetido)
        {
            id = -1;
            if (de3) id = Random.Range(0,8);
            if (de4) id = Random.Range(8,26);
            repetido = FindObjectOfType<Tabuleiro>().ExisteFigura(id);
        }

        if (id != -1) {
            meio.sprite = combinacoes[id];
            FindObjectOfType<Tabuleiro>().AdicionandoFigura(player,id);
        }
    }
}
