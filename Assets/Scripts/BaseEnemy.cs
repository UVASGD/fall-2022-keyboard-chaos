using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : Enemy
{
    Animator animator;

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
        //if(alive & !dizzy){
            // attack!
            // butnot in the demo!
            // I'm not that fleshed out!
        //}
    }

    public override void Die(){
        alive = false;
        animator.SetTrigger("die");
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