using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBox : Building
{

    public DetectionTrigger trigger;
    public PowerUpCard healBoxCard;

    public List<StatusEffectInfo> effectList;

    public bool applyToAllies = true;
    public bool applyToEnemies = false;

    public new void Awake() {
        base.Awake();
        trigger.listAddEvent.AddListener(ApplyHealingEffect);
        trigger.listRemoveEvent.AddListener(RemoveHealingEffect);
    }

    // Start is called before the first frame update
    void Start()
    {
        BaseInit();
        effectList = new List<StatusEffectInfo>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public override void EnableMovement()
    {
        
    }

    public override void Die() {

        trigger.listAddEvent.RemoveListener(ApplyHealingEffect);
        trigger.listRemoveEvent.RemoveListener(RemoveHealingEffect);
        Destroy(gameObject);
    }

    public override void DisableMovement()
    {
        
    }

    public override void Passive()
    {

    }

    private void RemoveHealingEffect() {

        Debug.Log("Removing healing effect");
        List<StatusEffectInfo> newList = new List<StatusEffectInfo>();

        /*
            Find an effect associated with each character. If one is 
            found, move it from the old list to the new list. 
        */
        foreach (Character c in trigger.characterList) {

            if (!applyToAllies && !c.IsEnemy()) continue;
            if (!applyToEnemies && c.IsEnemy()) continue;

            Debug.Log(c);

            foreach (StatusEffectInfo s in effectList) {

                Debug.Log(s + " " + effectList.Count);

                if (c == s.character) {
                    newList.Add(s);
                    effectList.Remove(s);
                    break;
                }
            }
        }

        //Stop the effect of those in the old list. 
        foreach(StatusEffectInfo s in effectList) {
            s.character.statsManager.StopCoroutine(s.coroutine);
            s.coroutine = null;
            s.list.Remove(s);

            s.character.statsManager.RevaluateModiferValue(s.card.effect);
        }

        effectList = newList;
    }

    private void ApplyHealingEffect() {

        Debug.Log("Adding healing effect");
        List<Character> characterListCopy = new List<Character>(trigger.characterList);

        /*
            Find an effect associated with each character. If one is 
            found, move it from the old list to the new list. 
        */
        foreach (StatusEffectInfo s in effectList) {

            Debug.Log(s + " " + effectList.Count);

            foreach (Character c in characterListCopy) {

                Debug.Log(c);

                if (c == s.character) {
                    characterListCopy.Remove(c);
                    break;
                }
            }
        }

        //Stop the effect of those in the old list. 
        foreach(Character c in characterListCopy) {

            if (!applyToAllies && !c.IsEnemy()) continue;
            if (!applyToEnemies && c.IsEnemy()) continue;

            effectList.Add(c.statsManager.ApplyPWEffect(healBoxCard));
        }
    }

}
