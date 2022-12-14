using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereEnemy : Enemy
{
    public Player player;
    [SerializeField] GameObject defaultSpellBallPrefab;
    Ability fireSpell;
    public float range;
    Rigidbody rb;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHealth;
        alive = true;
        fireSpell = new Ability("fireSpell", gameObject, 0, 5, "");
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool inRange = Vector3.Distance(transform.position, player.gameObject.transform.position) <= range;

        // When in range, enemy should face the player.
        // This is used for aiming the projectile
        if (inRange && !isDizzy)
            transform.LookAt(player.transform);

        if (alive && inRange && !isDizzy && fireSpell.tryUseAbility(player)) {
            GameObject tempSpellBall = Instantiate(defaultSpellBallPrefab, transform.position + transform.forward + transform.up, transform.rotation);
            tempSpellBall.GetComponent<Rigidbody>().velocity = transform.forward * speed;
            tempSpellBall.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public override void Die() {
        alive = false;
        rb.useGravity = true;
        player = null;
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