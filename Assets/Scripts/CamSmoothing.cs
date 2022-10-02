using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSmoothing : MonoBehaviour
{

    public GameObject camSmoother;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaInEuler = gameObject.transform.rotation.eulerAngles + Time.deltaTime * 2 * (camSmoother.transform.rotation.eulerAngles - gameObject.transform.rotation.eulerAngles);
        gameObject.transform.rotation.eulerAngles.Set(deltaInEuler.x, deltaInEuler.y, deltaInEuler.z);
    }
}
