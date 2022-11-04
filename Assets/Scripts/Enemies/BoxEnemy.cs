using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEnemy : Enemy
{
    Animator animator;
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        hitPoints = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(alive & !isDizzy){
            pass;
            // TODO get ability stuff working
        }
    }

    public override void Die(){
        alive = false;
        animator.SetTrigger("die");
    }

    public override void TakeDamage(float amount)
    {
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