using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plasmaDestruct : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SelfDestruct());
    }
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider co)
    {
        harm();
    }

    void harm()
    {
        Debug.Log("Ouch!");
    }
}
