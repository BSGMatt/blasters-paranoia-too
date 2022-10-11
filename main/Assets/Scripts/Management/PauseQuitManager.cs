using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseQuitManager : MonoBehaviour
{
    private bool gamePause = false;
    public GameManager gm;
    private Coroutine pauseMessage;

    public void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }

        if (gamePause && Input.GetKeyDown(KeyCode.Tab)) {
            Quit();
        }
    }

    private void TogglePause() {
        if (gamePause) {
            Debug.Log("Unpaused");
            Time.timeScale = 1;
            gamePause = false;
            return;
        }

        Debug.Log("Paused");
        gamePause = true;
        DisplayPauseInfoText();
        Time.timeScale = 0;
    }

    private void Quit() {
        Debug.Log("Quit game");
        Application.Quit();
    }

    private void DisplayPauseInfoText() {
        if (pauseMessage != null) {
            StopCoroutine(pauseMessage);
            pauseMessage = null;
        }
        pauseMessage = StartCoroutine(gm.ShowEventText("GAME IS PAUSED", 1f));
    }
}
