using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : Character
{
    public int state;
    public float[] stateDurations;
    public GameObject[] weapons;
    protected AIController aiController;

    protected IEnumerator RunThroughStates() {
        
        //Keep looping until boss is dead
        while (hp > 0) {
            state = (state + 1) % stateDurations.Length;
            yield return new WaitForSeconds(stateDurations[state]);
        }

        yield return null;
    }

    protected void Start() {
        aiController = GetComponent<AIController>();
        state = -1;
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
