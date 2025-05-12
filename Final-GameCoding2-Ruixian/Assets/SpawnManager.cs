using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnManager : MonoBehaviour
{
    public int maxActiveBugs = 4;
    private List<GameObject> activeBugs = new List<GameObject>();
    private List<BugSpawner> allSpawners;

    private bool isWaitingToRespawn = false;

    void Start()
    {
        allSpawners = FindObjectsOfType<BugSpawner>().ToList();
        SpawnInitialBugs();
    }

    void Update()
    {
        activeBugs.RemoveAll(bug => bug == null);

        if (activeBugs.Count == 0 && !isWaitingToRespawn)
        {
            StartCoroutine(RespawnBugAfterDelay());
        }
    }

    void SpawnInitialBugs()
    {
        TrySpawnBugs(maxActiveBugs);
    }

    public void OnBugCaught(GameObject bug, Transform spawnPoint)
    {
        activeBugs.Remove(bug);

        // 找到它属于哪个 spawner 并移除点
        foreach (var spawner in allSpawners)
        {
            if (spawner.spawnPoints.Contains(spawnPoint))
            {
                spawner.RemoveSpawnPoint(spawnPoint);
                break;
            }
        }

        if (!isWaitingToRespawn)
        {
            StartCoroutine(RespawnBugAfterDelay());
        }
    }

    IEnumerator RespawnBugAfterDelay()
    {
        isWaitingToRespawn = true;

        yield return new WaitForSeconds(1f);

        TrySpawnBugs(1);
        isWaitingToRespawn = false;
    }

    void TrySpawnBugs(int count)
    {
        float time = Time.time;
        List<(BugSpawner spawner, Transform point)> validPoints = new List<(BugSpawner, Transform)>();

        foreach (var spawner in allSpawners)
        {
            var points = spawner.GetValidSpawnPoints(time);
            foreach (var point in points)
            {
                validPoints.Add((spawner, point));
            }
        }

        // 打乱顺序
        validPoints = validPoints.OrderBy(x => Random.value).ToList();
        int spawnCount = Mathf.Min(count, validPoints.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            var (spawner, point) = validPoints[i];
            GameObject bug = spawner.SpawnAtPoint(point);
            activeBugs.Add(bug);

            // 初始化 bug，记录它来自哪个点
            Bug bugScript = bug.GetComponent<Bug>();
            if (bugScript != null)
            {
                bugScript.Init(this, point);
            }


            Butterfly butterfly = bug.GetComponent<Butterfly>();
            if (butterfly != null)
            {
                butterfly.Init(this, point);
            }

            Grasshopper grasshopper = bug.GetComponent<Grasshopper>();
            if (grasshopper != null)
            {
                grasshopper.Init(this, point);
            }

        }



    }

    public void OnBugEscaped(GameObject bug)
    {
        if (bug != null && activeBugs.Contains(bug))
    {
        activeBugs.Remove(bug);
    }

    if (!isWaitingToRespawn)
    {
        StartCoroutine(RespawnBugAfterDelay());
    }

    }
}
