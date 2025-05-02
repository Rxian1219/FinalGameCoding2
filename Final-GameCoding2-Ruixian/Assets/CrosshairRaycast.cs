using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairRaycast : MonoBehaviour
{

    public float interactableDistance = 5f;
    public LayerMask interactLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
    }
}
