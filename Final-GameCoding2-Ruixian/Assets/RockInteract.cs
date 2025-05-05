using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockInteract : MonoBehaviour
{

    public float liftHeight = 1.5f;    
    public float liftSpeed = 2.5f;       
    public float floatDuration = 2f;

    private Vector3 originalPosition;
    private bool isMoving = false;


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact()
    {
        if (!isMoving)
        {
            StartCoroutine(LiftRock());
        }
    }

    private System.Collections.IEnumerator LiftRock()
    {
        isMoving = true;

        Vector3 targetPosition = originalPosition + Vector3.up * liftHeight;
        float elapsed = 0f;

        // slowly lift up
        while (elapsed < 1f)
        {
            transform.position = Vector3.Lerp(originalPosition, targetPosition, elapsed);
            elapsed += Time.deltaTime * liftSpeed;
            yield return null;
        }
        transform.position = targetPosition;

    
        yield return new WaitForSeconds(floatDuration);

        elapsed = 0f;

    
        while (elapsed < 1f)
        {
            transform.position = Vector3.Lerp(targetPosition, originalPosition, elapsed);
            elapsed += Time.deltaTime * liftSpeed;
            yield return null;
        }
        transform.position = originalPosition;

        isMoving = false;
    }

}
