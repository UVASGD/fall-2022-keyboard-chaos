using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOverTime : MonoBehaviour
{
    private Destructible target;


    // Start is called before the first frame update
    void Start()
    {
        target = this.transform.parent.GetComponent<Destructible>();
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ApplyDamageEffect(float damage, int times, float delay)
    {
        StartCoroutine(Damage(damage, times, delay));
    }

    private IEnumerator Damage(float damage, int times, float delay)
    {
        for (int i = 0; i < times; i++)
        {
            yield return new WaitForSeconds(delay);
            target.TakeDamage(damage);
        }
    }
}
