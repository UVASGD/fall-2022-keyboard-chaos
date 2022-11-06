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
        this.triggerName = triggerName;
        animator = player.GetComponent<Animator>();
    }

    public void useAbility(Destructible target)
    {
        //do the ability
        target.GetComponent<Enemy>().TakeDamage(damageToEnemy);
        if (triggerName != "") animator.SetTrigger(triggerName);

        //reset the cooldown
        timeOfLastUse = Time.realtimeSinceStartup;
    }

    public void useAbility(Destructible[] targets)
    {
        //do the ability
        foreach (Destructible t in targets)
        {
            t.GetComponent<Enemy>().TakeDamage(damageToEnemy);
        }

        if (triggerName != "") animator.SetTrigger(triggerName);

        //reset the cooldown
        timeOfLastUse = Time.realtimeSinceStartup;
    }

    public void resetCooldown()
    {
        timeOfLastUse = Time.realtimeSinceStartup;
    }

    public bool isCoolAndUnlocked()
    {
        return unlocked && Time.realtimeSinceStartupAsDouble - timeOfLastUse > coolDown;
    }

    public void unlockAbility()
    {
        unlocked = true;
    }

    public void lockAbility()
    {
        unlocked = false;
    }

    public bool isUnlocked()
    {
        return unlocked;
    }

    public float timeSinceLastUse()
    {
        return (float)Time.realtimeSinceStartupAsDouble - timeOfLastUse;
    }

}
