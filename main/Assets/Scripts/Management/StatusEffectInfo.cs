using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/**
    An object that contains information about a given status effect. 
*/
public class StatusEffectInfo {
    ///Coroutine managing the over time effect.
    public Coroutine coroutine;
        
    ///The PowerUpCard containing the information about the effect.
    public PowerUpCard card;

    ///The list this struct belongs to. 
    public List<StatusEffectInfo> list;

    //The character the effect is being applied to. 
    public Character character;

    public StatusEffectInfo(PowerUpCard card, List<StatusEffectInfo> list) {
        this.card = card;
        this.list = list;
        coroutine = null;
    }

    public override string ToString()
    {
        return string.Format("Status effect: {0}, Running coroutine: {1}, on character: {2} inside list: {3}", 
            card, coroutine, character, list);
    }
}
