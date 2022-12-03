using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShereEnemyMovement : MonoBehaviour
{

    [SerializeField] GameObject player;
    public float speed, maxAway;
    float height = 0;
    int i = 0;
    Vector3 nextPos;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 nextRelXYPos = maxAway * Random.insideUnitCircle;
        nextPos = new Vector3(player.transform.position.x + nextRelXYPos.x, player.transform.position.y + height + 2, player.transform.position.z + nextRelXYPos.y);
    }

    // Update is called once per frame
    void Update()
    {
        i++;
        if (i == 500)
        {
            if (height < 1 && Random.Range(0,100) < 40)
            {
                height = 3;
                nextPos = new Vector3(transform.position.x, player.transform.position.y + height + 1, transform.position.z);
            }
            else if (height > 1 && Random.Range(0,100) < 70)
            {
                height = 0;
                nextPos = new Vector3(transform.position.x, player.transform.position.y + 1, transform.position.z);
            }
            else
            {
                Vector2 nextRelXYPos = maxAway * Random.insideUnitCircle;
                nextPos = new Vector3(player.transform.position.x + nextRelXYPos.x, player.transform.position.y + height + 2, player.transform.position.z + nextRelXYPos.y);
            }
            i = 0;
        }
        
        transform.position = Vector3.Lerp(transform.position, nextPos, speed*Time.deltaTime);
    }
}
