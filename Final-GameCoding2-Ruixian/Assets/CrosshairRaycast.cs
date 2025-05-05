using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairRaycast : MonoBehaviour
{
    // Ray length
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
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactableDistance, interactLayer))
        {
            // check tag Bug
            if (hit.collider.CompareTag("Bug"))
            {
                Debug.Log("Looking at bug: " + hit.collider.name);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    Bug bug = hit.collider.GetComponent<Bug>();
                    if (bug != null)
                    {
                        bug.Catch();
                    }
                }
            }

            // check tag Rock
            else if (hit.collider.CompareTag("Rock"))
            {
                Debug.Log("Looking at rock: " + hit.collider.name);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    RockInteract rock = hit.collider.GetComponent<RockInteract>();
                    if (rock != null)
                    {
                        rock.Interact();
                    }
                }
            }
        }
    }
}

