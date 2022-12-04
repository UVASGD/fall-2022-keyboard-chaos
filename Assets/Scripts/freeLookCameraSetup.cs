using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class freeLookCameraSetup : MonoBehaviour
{

    // This is the script to update the free look camera to point at the player when the game starts 
    // this script is pretty much just for convenience, as it means you don't have to set this in the inspector
    void Start()
    {
        Transform player = FindObjectOfType<Player>().gameObject.transform;
        CinemachineFreeLook cm = GetComponent<CinemachineFreeLook>();
        cm.Follow = player;
        cm.LookAt = player;
    }
}
