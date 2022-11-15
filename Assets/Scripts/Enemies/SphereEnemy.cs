using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEnemy : Enemy
{
    public Player player;
    Ability fireSpell;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHealth;
        alive = true;
    }

    // Update is called once per frame
    void Update()
    {
        // put attack code here later
    }

    public override void Die() {
        alive = false;
    }

    public override void TakeDamage(float amount)
    {
        this.hitPoints -= amount;
        if (healthBar) {
            healthBar.SetHealth(this.hitPoints);
        }
        if (hitPoints <= 0) {
            Die();
        }
    }

    public override void MakeDizzy()
    {
        isDizzy = true;
        StartCoroutine(BeDizzy());
    }
    IEnumerator BeDizzy()
    {
        yield return new WaitForSeconds(8);
        isDizzy = false;
    }
}