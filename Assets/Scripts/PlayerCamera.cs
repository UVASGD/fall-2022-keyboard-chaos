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
    
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(target);
    }
}
