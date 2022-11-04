using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float damage = 0;
    [SerializeField] float timeToDisappear = 0;

    private bool created = false;
    private float timeOfCreation;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Destructible d = other.gameObject.GetComponent<Destructible>();
        if (d != null)
        {
            d.TakeDamage(damage);
        }
        destroy();
    }

    private void Update()
    {
        if (!created)
        {
            created = true;
            timeOfCreation = Time.realtimeSinceStartup;
        }

        if (Time.realtimeSinceStartup-timeOfCreation > timeToDisappear)
        {
            destroy();
        }
    }

    private void destroy()
    {
        //animator.playanimation("destroy")?????
        Destroy(gameObject);
    }
}
