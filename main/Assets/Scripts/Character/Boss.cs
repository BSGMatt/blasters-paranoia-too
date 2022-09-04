using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Boss : Character
{
    public int state;
    protected int prevState;
    public float[] stateDurations;
    public WeaponCard[] weapons;
    protected AIController aiController;

    public UnityEvent bossDied;

    protected void Awake() {
        bossDied = new UnityEvent();
        
    }

    protected IEnumerator RunThroughStates() {
        
        //Keep looping until boss is dead
        while (hp > 0) {
            prevState = state;
            state = (state + 1) % stateDurations.Length;
            yield return new WaitForSeconds(stateDurations[state]);
        }

        yield return null;
    }

    protected void Start() {
        aiController = GetComponent<AIController>();
        state = -1;
        hp = maxHP;
        isEnemy = true;
        onlyHasHealth = true;
        isLevelable = false;
        StartCoroutine(RunThroughStates());
    }

    protected void Update() {
        switch (state) {
            default:
                CalmState();
                break;
            case 1:
                ChargingState();
                break;
            case 2:
                ReleaseState();
                break;
        }
    }

    public abstract void CalmState();
    public abstract void ChargingState();
    public abstract void ReleaseState();


    
}
