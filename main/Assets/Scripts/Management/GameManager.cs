using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles core logic, as well as the initialization and transition of phases.
/// </summary>
public class GameManager : MonoBehaviour
{
    public const int MAX_CASH = 99999999;


    public GameObject shop;
    public Phase phase = Phase.IDLE;
    public int bossWave = 5;
    public int wave = 1;
    public int preptime = 300;
    public int cash = 0;
    public Text timerDisplay;
    public Text commentary;
    public bool shopEnabled = false;
    public bool movementEnabled = true;

    private int time;
    private Coroutine timer; 

    public void Start() {
        ToIdlePhase();
    }

    public void Update() {
        switch (phase) {
            default: //Phase.IDLE
                Idle();
                return;
            case Phase.PREP:
                Prep();
                return;
            case Phase.SWARM:
                Swarm();
                return;
            case Phase.BOSS:
                Boss();
                return;
        }
    }

    #region phase_methods
    private void ToPrepPhase() {
        commentary.text = "PREPARE YOUR DEFENSES";
        if (timer != null) StopCoroutine(timer);
        timer = StartCoroutine(runTimer());
        phase = Phase.PREP;
    }

    private void Prep() {
        if (timer == null) ToSwarmPhase();

        timerDisplay.text = "Time: " + time.ToString();

        if (Input.GetKeyDown(KeyCode.F)) {
            ActivateShop();
        }


    }

    private void ToSwarmPhase() {

    }

    private void Swarm() {

    }

    private void ToBossPhase() {

    }

    private void Boss() {

    }

    private void ToIdlePhase() {
        phase = Phase.IDLE;
        shopEnabled = false;
        shop.SetActive(shopEnabled);
    }

    private void Idle() {

        commentary.text = "PRESS ENTER TO START";

        //Press the enter key to go to next phase. 
        if (Input.GetKeyDown(KeyCode.Return)) {
            ToPrepPhase();
        }
    }

    #endregion

    /// <summary>
    /// Runs the prep phase timer. 
    /// </summary>
    /// <returns></returns>
    private IEnumerator runTimer() {

        time = preptime;

        //Decrement the time every second until the timer reaches 0. 
        while (time > 0) {
            yield return new WaitForSeconds(1f);

            while (shopEnabled) {
                yield return null;
            }


            time--;
        }

        timer = null;

        yield return 0;
    }

    private bool IsBossWave() {
        return wave % bossWave == 0;
    }

    private void ActivateShop() {
        shop.SetActive(!shopEnabled);
        shopEnabled = !shopEnabled;

        if (movementEnabled) {
            DisableAllCharacterMovement();
            
        }
        else {
            EnableAllCharacterMovement();
        }

        movementEnabled = !movementEnabled;
    }

    public void DisableAllCharacterMovement() {
        foreach (Character c in FindObjectsOfType<Character>()) {
            c.DisableMovement();
        }
    }

    public void EnableAllCharacterMovement() {
        foreach (Character c in FindObjectsOfType<Character>()) {
            c.EnableMovement();
        }
    }
}
