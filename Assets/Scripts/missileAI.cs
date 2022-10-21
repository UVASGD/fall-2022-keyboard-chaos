using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missileAI : MonoBehaviour
{
    public Transform player;
    public Rigidbody rb;
    public float force;
    public float rotationSpd;



    public GameObject effect0;
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(15f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider co)
    {
        explode();
    }

    void explode()
    {
        GameObject plasma0;
        plasma0 = Instantiate(effect0, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }


    // it's good 4 physics calculations framerate indenpendent
    void FixedUpdate()
    {
        rb.velocity = transform.forward * force;
        var rocketTargetRotaton = Quaternion.LookRotation(player.position - rb.position);
        //make the actual turn
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rocketTargetRotaton, rotationSpd));
    }


}
