using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triangleEnemy : Enemy
{
    public Player player;
    [SerializeField] GameObject defaultSpellBallPrefab;
    Ability triangleAttack;
    public float range;
    Rigidbody rb;
    GameObject go;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = maxHealth;
        alive = true;
        triangleAttack = new Ability("triangleAttack", gameObject, 0, 1, "");
        range = 15;
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody>();
        go = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        bool inRange = Vector3.Distance(transform.position, player.gameObject.transform.position) <= range;

        // When in range, enemy should face the player.
        // This is used for aiming the projectile
        if (inRange && !isDizzy)
        {
            Quaternion OriginalRot = transform.rotation;
            transform.LookAt(player.transform);
            Quaternion NewRot = transform.rotation;
            transform.rotation = OriginalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, speed * Time.deltaTime);
        }
           

        if (alive && inRange && !isDizzy && triangleAttack.tryUseAbility(player))
        {
            GameObject tempSpellBall = Instantiate(defaultSpellBallPrefab, gameObject.transform.position + gameObject.transform.forward * 5, transform.rotation);
            tempSpellBall.GetComponent<Rigidbody>().velocity = transform.forward * 10;
        }
    }

    public override void Die()
    {
        alive = false;
        rb.useGravity = true;
        player = null;
    }

    public override void TakeDamage(float amount)
    {
        this.hitPoints -= amount;
        if (healthBar)
        {
            healthBar.SetHealth(this.hitPoints);
        }
        if (hitPoints <= 0)
        {
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