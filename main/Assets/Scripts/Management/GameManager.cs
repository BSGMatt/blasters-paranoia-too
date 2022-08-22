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


    public GameObject shopUI;
    public GameObject mainUI;
    public GameObject builderUI;
    public GameObject builder;
    public GameObject eventDisplay;
    public Phase phase = Phase.IDLE;
    public int bossWave = 5;
    public int wave = 0;
    public int preptime = 300;
    public int cash = 0;
    public Text timerDisplay;
    public Text commentary;
    public Text waveText;
    public bool shopEnabled = false;
    public bool builderEnabled = false;
    public bool movementEnabled = true;
    public bool bossDefeated = false;

    public SpawnManager spawnManager;
    public InventoryManager im;
    public Crystal crystal;
    public Boss boss;

    private int time;
    private Coroutine timer; 


    public void Start() {
        eventDisplay.SetActive(false);
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

        timerDisplay.text = "TIME: " + time.ToString();
        waveText.text = "WAVE: " + wave.ToString();

        if (Input.GetKeyDown(KeyCode.F) && !builderEnabled) {
            ToggleShop();
        }

        if (Input.GetKeyDown(KeyCode.B) && !shopEnabled) {
            ToggleBuilder();
        }

        //Press the enter key to go to next phase. 
        if (Input.GetKeyDown(KeyCode.Return)) {

            //Don't let player continue to next phase unless they have a weapon.
            if (im.weaponCards.Count == 0) {
                StartCoroutine(ShowEventText("GET A WEAPON FIRST!!", 2f));
                return;
            }

            if (shopEnabled) ToggleShop();
            if (builderEnabled) ToggleBuilder();

            StopCoroutine(timer);
            timer = null;
        }
    }

    private void ToSwarmPhase() {      
        spawnManager.ActivateSpawner();
        phase = Phase.SWARM;
    }

    private void Swarm() {
        timerDisplay.text = "FIGHT!";
        commentary.text = spawnManager.enemiesLeft + " ENEMIES LEFT";

        if (spawnManager.allEnemiesDead()) {
            if (IsBossWave()) {
                ToBossPhase();
            }
            else {
                StartCoroutine(ShowEventText("YOU SURVIVED!", 2));
                ToIdlePhase();
            }
        }
    }

    /// <summary>
    /// Sets up the boss phase of a wave.
    /// </summary>
    private void ToBossPhase() {
        boss = spawnManager.SpawnBoss();
        StartCoroutine(ShowEventText("LARGE HOSTILE INCOMING!", 2));
        timerDisplay.text = "DEFEAT THE BOSS!";
        bossDefeated = false;
    }

    private void Boss() {
        if (boss.dead) {
            StartCoroutine(ShowEventText("THAT'LL TEACH 'EM!", 2));
            Destroy(boss);
            ToIdlePhase();
        }
    }

    private void ToIdlePhase() {
        phase = Phase.IDLE;
        shopEnabled = false;
        builderEnabled = false;
        shopUI.SetActive(shopEnabled);
        mainUI.SetActive(!shopEnabled);
        builder.SetActive(builderEnabled);
        builderUI.SetActive(builderEnabled);

        if (crystal.dead) {
            crystal.ReviveCrystal();
        }

        crystal.hp = crystal.maxHP;

        waveText.text = "";
        timerDisplay.text = "TIME: " + preptime;
        wave++;
        if (wave > 1) FindObjectOfType<XPBonusManager>().ApplyXPBonuses();
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

    /// <summary>
    /// Displays a message over the commentary bar for a period of time.
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private IEnumerator ShowEventText(string message, float time) {
        eventDisplay.SetActive(true);
        eventDisplay.GetComponentInChildren<Text>().text = message;
        yield return new WaitForSeconds(time);
        eventDisplay.GetComponentInChildren<Text>().text = "";
        eventDisplay.SetActive(false);
    }

    /// <summary>
    /// Displays a series of messages over the commentary bar for a period of time. 
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    private IEnumerator ShowEventText(string[] messages, float time) {
        eventDisplay.SetActive(true);

        foreach(string m in messages) {
            eventDisplay.GetComponentInChildren<Text>().text = m;
            yield return new WaitForSeconds(time);
        }
        eventDisplay.GetComponentInChildren<Text>().text = "";
        eventDisplay.SetActive(false);
    }

    public bool IsBossWave() {
        return wave % bossWave == 0;
    }

    private void ToggleShop() {
        mainUI.SetActive(shopEnabled);
        shopUI.SetActive(!shopEnabled);
        shopEnabled = !shopEnabled;

        if (movementEnabled) {
            DisableAllCharacterMovement();
            
        }
        else {
            EnableAllCharacterMovement();
        }

        movementEnabled = !movementEnabled;
    }

    private void ToggleBuilder() {
        mainUI.SetActive(builderEnabled);
        builder.SetActive(!builderEnabled);
        builderUI.SetActive(!builderEnabled);

        builderEnabled = !builderEnabled;
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
