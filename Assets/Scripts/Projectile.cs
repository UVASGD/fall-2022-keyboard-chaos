using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float damage = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.tag);
        Destructible d = other.gameObject.GetComponent<Destructible>();
        if (d != null)
        {
            d.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
