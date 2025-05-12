using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bug : MonoBehaviour
{

    public string bugName = "Beetle";

    private SpawnManager manager;
    private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(SpawnManager mgr, Transform point)
    {
        manager = mgr;
        spawnPoint = point;
    }

    public void Catch()
    {
        Debug.Log("You caught a " + bugName + "!");


        if (BugTracker.Instance != null)
        {
            BugTracker.Instance.AddBug(bugName);
        }

        if (manager != null && spawnPoint != null)
        {
            manager.OnBugCaught(gameObject, spawnPoint);
        }

        Destroy(gameObject);
    }

}
