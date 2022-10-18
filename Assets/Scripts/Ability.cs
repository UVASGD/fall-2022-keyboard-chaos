using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ability
{
    private GameObject player;
    private Animator animator;

    public string name;
    public float coolDown;
    public float damageToEnemy;
    private string triggerName;

    public float timeOfLastUse;

    public bool unlocked =  true;
    

    public Ability(string name, GameObject player, float damageToEnemy, float coolDown, string triggerName)
    {
        this.name = name;
        this.player = player;
        this.damageToEnemy = damageToEnemy;
        this.coolDown = coolDown;
        animator = player.GetComponent<Animator>();
    }

    public bool useAbility(Destructible target)
    {
        //check to see if the effect has cooled down
        if (Time.realtimeSinceStartupAsDouble - timeOfLastUse < coolDown) return false;

        //do the ability 
        target.GetComponent<Enemy>().TakeDamage(damageToEnemy);
        if (triggerName != "") animator.SetTrigger(triggerName);

        //reset the cooldown
        timeOfLastUse = Time.realtimeSinceStartup;
        return true;
    }

    public void unlockAbility()
    {
        unlocked = true;
    }

    public void lockAbility()
    {
        unlocked = false;
    }

    public float timeSinceLastUse()
    {
        return (float)Time.realtimeSinceStartupAsDouble - timeOfLastUse;
    }

}
