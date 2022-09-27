using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Restart : MonoBehaviour
{
    public void RestartGame() {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void Jogar() {
        GameObject painel = GameObject.FindWithTag("FadeToBlack");
        painel.GetComponent<Image>().raycastTarget = true;
        LeanTween.alpha(painel.GetComponent<RectTransform>(), 1f, 1.5f).setOnComplete(Jogo);
    }

    void Jogo() {
        SceneManager.LoadScene("PuzzleScene1");
    }

    public void Livre1() {
        SceneManager.LoadScene("1PGameScene");
    }

    public void Livre2() {
        SceneManager.LoadScene("2PGameScene");
    }

    public void Menu() {
        SceneManager.LoadScene("Menu");
    }
}
