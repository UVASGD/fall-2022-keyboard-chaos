using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEnemy : Enemy
{
    Animator animator;
    public Player player;
    Ability slash, flatten, buffer;

    float range;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("DogPBR").GetComponent<Player>();
        animator = gameObject.GetComponent<Animator>();
        hitPoints = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        alive = true;
        range = 4;
        slash = new Ability("slash", gameObject, 10, 8, "attack2");
        flatten = new Ability("flatten", gameObject, 3, 2, "attack1");
        
        //this makes it so abilities aren't  cast back to back
        buffer = new Ability("buffer", gameObject, 0, 3, "");
    }

    // Update is called once per frame
    void Update()
    {
        bool inRange = Vector3.Distance(transform.position, player.gameObject.transform.position) <= range;
        //idk how else to just do this condition lol
        if(alive && !isDizzy && inRange && slash.checkAbility() && buffer.tryUseAbility(player)){
            slash.tryUseAbility(player);
        }
        else if(alive && !isDizzy && inRange && flatten.checkAbility() && buffer.tryUseAbility(player)){
            flatten.tryUseAbility(player);
        }
    }

    public override void Die(){
        alive = false;
        animator.SetTrigger("die");
    }

    public override void TakeDamage(float amount){
        this.hitPoints -= amount; //not sure why I'm using "this" but this code works lmao
        animator.SetTrigger("getHit");
        if(amount > 10){
            AudioManager.instance.Play("monsterDmg");
        }
        else{
            AudioManager.instance.Play("monsterDmgLight");
        }
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