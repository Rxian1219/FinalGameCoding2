using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{

    public GameObject bugPrefab;  // the bug('s prefab)  you want to spawn
    public Transform[] spawnPoints; // spawn position list

    // Start is called before the first frame update
    void Start()
    {
        SpawnBugs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBugs()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(bugPrefab, point.position, point.rotation);
        }
    }

}
