using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEnemy : Enemy
{
    Animator animator;
    public Player player;
    Ability slash, flatten, buffer;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        hitPoints = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        alive = true;
        slash = new Ability("slash", gameObject, 30, 5, "attack2");
        flatten = new Ability("flatten", gameObject, 15, 4, "attack1");
        
        //this makes it so abilities aren't  cast back to back
        buffer = new Ability("buffer", gameObject, 0, 2, "");
    }

    // Update is called once per frame
    void Update()
    {
        //idk how else to just do this condition lol
        if(alive && !isDizzy && slash.tryUseAbility(player) && buffer.tryUseAbility(player));
        if(alive && !isDizzy && flatten.tryUseAbility(player) && buffer.tryUseAbility(player));
    }

    public override void Die(){
        alive = false;
        animator.SetTrigger("die");
    }

    public override void TakeDamage(float amount){
        this.hitPoints -= amount; //not sure why I'm using "this" but this code works lmao
        animator.SetTrigger("getHit");
        if(healthBar){
            healthBar.SetHealth(this.hitPoints);
        }
        if (hitPoints <= 0)
        {
            Die();
        }
    }

    public override void MakeDizzy(){
        isDizzy = true;
        animator.SetBool("isDizzy",true);
        StartCoroutine(BeDizzy());
    }

    IEnumerator BeDizzy(){
        yield return new WaitForSeconds(8);
        isDizzy = false;
        animator.SetBool("isDizzy", false);
    }
}