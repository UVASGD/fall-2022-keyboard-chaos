using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    //test message
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(target);
    }
}
