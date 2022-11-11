using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetingArrow : MonoBehaviour
{
    private Renderer arrowRenderer;
    public bool targeting = true;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        arrowRenderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = target.transform.position + new Vector3(0, 4f, 0);
        if (targeting)
        {
            arrowRenderer.material.SetColor("_Color", new Color(255f, 54f, 0f, 150f));
        }
        else
        {
            arrowRenderer.material.SetColor("_Color", new Color(255f, 255f, 255f, 150f));
        }
    }
}
