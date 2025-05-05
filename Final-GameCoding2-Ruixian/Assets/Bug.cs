using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{

    public string bugName = "Beetle"; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Catch()
    {
        Debug.Log("You caught a " + bugName + "!");
        // 

        Destroy(gameObject); 
    }

}
