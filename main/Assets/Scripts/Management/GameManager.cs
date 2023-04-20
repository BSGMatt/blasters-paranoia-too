using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public GameObject cashUI;
    public Phase phase = Phase.IDLE;
    public int bossWave = 1;
    public int wave = 0;
    public int preptime = 300;
    public int cash = 0;
    public int cashRewardPerWave = 200;
    public int respawnTime = 3;
    public Text timerDisplay;
    public Text commentary;
    public Text waveText;
    public bool shopEnabled = false;
    public bool builderEnabled = false;
    public bool movementEnabled = true;
    public bool bossDefeated = false;

    public float defaultCameraSize = 5;

    public AudioManager audioManager;
    public SpawnManager spawnManager;
    public InventoryManager im;
    public Crystal[] crystals;
    public Boss boss;
    public CameraMan cameraMan;
    public Character player;

    private int time;
    private Coroutine timer; 

    private Coroutine playerWaitingToSpawn = null;


    public void Start() {
        eventDisplay.SetActive(false);
        ToIdlePhase();
    }

    public void Update() {

        cashUI.GetComponentInChildren<Text>().text = "CASH: " + cash;
        CheckForLoseCondition();

        //Do all of the work related to the appropriate phase. 
        switch (phase) {
            default: //Phase.IDLE
                Idle();
                break;
            case Phase.PREP:
                Prep();
                break;
            case Phase.SWARM:
                Swarm();
                break;
            case Phase.BOSS:
                Boss();
                break;
        }
    }

    private void CheckForLoseCondition() {
        if (player.dead && playerWaitingToSpawn == null) {
            //Check to see if at least one of the crystals are alive.
            bool allCrystalsDead = true;
            foreach (Crystal c in crystals) {
                if (!c.dead) {
                    allCrystalsDead = false;
                    break;
                }
            }

            //If all crystals are dead, restart the game. 
            if (allCrystalsDead) {
                SceneManager.LoadScene("mvp");
            }
            else {
                playerWaitingToSpawn = StartCoroutine(WaitToSpawn((int) respawnTime));
            }

        }
    }

    private IEnumerator WaitToSpawn(int seconds) {

        int timePast = 0;
        player.transform.position = Vector3.zero;
        player.invincible = true;
        Color oldColor = player.GetComponent<SpriteRenderer>().color;
        player.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
        player.DisableMovement();

        timePast = seconds;
        while (timePast > 0) {
            StartCoroutine(ShowEventText("RESPAWNING IN: " + timePast, 1f));

            timePast--;
            yield return new WaitForSeconds(1f);
        }
        
        player.EnableMovement();
        player.ReplenishHPAndStamina();
        player.GetComponent<SpriteRenderer>().color = oldColor;
        player.dead = false; 
        player.invincible = false;
        playerWaitingToSpawn = null;

    }


    #region phase_methods
    private void ToPrepPhase() {
        audioManager.StopAll();
        audioManager.Play("Prep");
        commentary.text = "PREPARE YOUR DEFENSES";
        cashUI.SetActive(true);
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
        audioManager.StopAll();
        audioManager.Play("Attack");    
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
        audioManager.StopAll();
        audioManager.Play("Boss");
        boss = spawnManager.SpawnBoss();
        boss.bossDied.AddListener(BossDefeated);
        cameraMan.CreateFocalPoint(player.transform, boss.transform, 0.5f);
        StartCoroutine(ShowEventText("LARGE HOSTILE INCOMING!", 2));
        commentary.text = "DEFEAT THE BOSS!";
        bossDefeated = false;
        phase = Phase.BOSS;
    }

    private void Boss() {
        timerDisplay.text = "BOSS HP: " + boss.hp + " / " + boss.maxHP;
    }

    private void BossDefeated() {
        StartCoroutine(ShowEventText("THAT'LL TEACH 'EM!", 2));
        cameraMan.DestroyLastFocalPoint();
        cameraMan.GetCamera().orthographicSize = defaultCameraSize;
        ToIdlePhase();
    }

    private void ToIdlePhase() {
        audioManager.StopAll();
        phase = Phase.IDLE;
        shopEnabled = false;
        builderEnabled = false;
        shopUI.SetActive(shopEnabled);
        mainUI.SetActive(!shopEnabled);
        builder.SetActive(builderEnabled);
        builderUI.SetActive(builderEnabled);
        cashUI.SetActive(false);

        

        waveText.text = "";
        timerDisplay.text = "TIME: " + preptime;
        if (wave > 0) {

            //Destroy any active projectiles 
            foreach (Pellet p in FindObjectsOfType<Pellet>()) {
                Destroy(p.gameObject);
            }
            foreach (Hazard p in FindObjectsOfType<Hazard>()) {
                Destroy(p.gameObject);
            }

            FindObjectOfType<XPBonusManager>().ApplyXPBonuses();
            cash += cashRewardPerWave;
            ReviveAllCrystals();
            player.ReplenishHPAndStamina();
            audioManager.Play("Victory");
        }
        wave++;
    }

    private void Idle() {

        commentary.text = "PRESS ENTER TO START";

        //Press the enter key to go to next phase. 
        if (Input.GetKeyDown(KeyCode.Return)) {
            ToPrepPhase();
        }
    }

    #endregion

    private void ReviveAllCrystals() {
        foreach (Crystal c in crystals) {
            c.ReviveCrystal();
        }
    }

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
    public IEnumerator ShowEventText(string message, float time) {
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
    public IEnumerator ShowEventText(string[] messages, float time) {
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
        cashUI.SetActive(shopEnabled);
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

        if (builderEnabled)
            builder.GetComponent<Builder>().builderWindow.gameObject.SetActive(!builderEnabled);

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
