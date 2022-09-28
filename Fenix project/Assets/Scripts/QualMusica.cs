using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualMusica : MonoBehaviour
{
    [Header("Qual Música?")]
    [SerializeField] bool batalha1;
    [SerializeField] bool batalha2;
    [SerializeField] bool conversaMenu;
    [SerializeField] bool puzzle;
    [SerializeField] bool final;

    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().StopAll();

        if (conversaMenu) FindObjectOfType<AudioManager>().Play("Menu");
        if (puzzle) FindObjectOfType<AudioManager>().Play("Puzzle");
        if (final) FindObjectOfType<AudioManager>().Play("Ending");
        if (batalha1) FindObjectOfType<AudioManager>().Play("Batalha1");
        if (batalha2) FindObjectOfType<AudioManager>().Play("Batalha2");
    }
}
