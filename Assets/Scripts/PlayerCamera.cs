using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if (player.isTargeting)
        {
            transform.LookAt(player.target.transform);
        }
        else
        {
            transform.LookAt(player.gameObject.transform);
        }
        
    }
}
