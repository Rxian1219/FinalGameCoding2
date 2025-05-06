using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugSpawner : MonoBehaviour
{

    public GameObject bugPrefab;  // the bug('s prefab)  you want to spawn
    public Transform[] spawnPoints; // spawn position list

    public bool requireRain = false;     // 是否要求下雨
    private bool hasSpawned = false;

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;

        if (!hasSpawned)
        {
            if (requireRain)
            {
                if (IsRainTime(time))
                {
                    SpawnBugs();
                    hasSpawned = true;
                }
            }
            else
            {
                SpawnBugs();
                hasSpawned = true;
            }
        }
    }

    void SpawnBugs()
    {
        foreach (Transform point in spawnPoints)
        {
            Instantiate(bugPrefab, point.position, point.rotation);
        }
    }


    bool IsRainTime(float time)
    {
        // 只在 120–180s 或 300–360s 下雨
        return (time >= 120f && time < 180f) || (time >= 300f && time < 360f);
    }

}
