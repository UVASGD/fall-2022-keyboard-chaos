using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    //I really be using this every year
    public float maxHealth;
    public float hitPoints;
    public bool alive;
    public HealthBar healthBar;

    //TakeDamage() with no parameters does fatal damage
    public virtual void TakeDamage()
    {
        if(healthBar){
            healthBar.SetHealth(0);
        }
        Die();
    }

    public virtual void TakeDamage(float amount)
    {
        this.hitPoints -= amount; //not sure why I'm using "this" but this code works lmao
        if(healthBar){
            healthBar.SetHealth(this.hitPoints);
        }
        if (hitPoints <= 0)
        {
            Die();
        }
    }

    public virtual void Die() {
        alive = false;
		//Destroy(gameObject);
        //think with rpgs corpses exist and they dont just blip. This bool will handle that
        // ..but not in the demo
    }

    //Heal() with no parameters heals to full
    public virtual void Heal()
    {
        hitPoints = maxHealth; //same here the "this" confuses me but oh well
        if(healthBar){
            healthBar.SetHealth(hitPoints);
        }
    }

    public virtual void Heal(float amount)
    {
        hitPoints += amount;
        if(healthBar){
            healthBar.SetHealth(hitPoints);
        }
        //limit overheal
        if (hitPoints >= maxHealth)
        {
            hitPoints = maxHealth;
        }
    }
}