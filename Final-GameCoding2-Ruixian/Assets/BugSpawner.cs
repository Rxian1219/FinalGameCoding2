using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BugSpawner : MonoBehaviour
{

    public GameObject bugPrefab;  // the bug('s prefab)  you want to spawn
    public Transform[] spawnPoints; // spawn position list

    public bool rainy = false;
    public bool cloudy = false;
    public bool morningSunny = false;
    public bool nightSunny = false;

    /*private bool hasSpawned = false; */

    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        /*float time = Time.time;

        if (!hasSpawned)
        {
            if (rainy && IsRainTime(time))
            {
                SpawnBugs();
                hasSpawned = true;
            }
            else if (cloudy && IsCloudyTime(time))
            {
                SpawnBugs();
                hasSpawned = true;
            }
            else if (morningSunny && IsMorningSunnyTime(time))
            {
                SpawnBugs();
                hasSpawned = true;
            }
            else if (nightSunny && IsNightSunnyTime(time))
            {
                SpawnBugs();
                hasSpawned = true;
            }
            else if (!rainy && !cloudy && !morningSunny && !nightSunny)
            {
                // Ĭ�����ͣ��κ�ʱ�䶼��ˢ�ĳ��ӣ��� Beetle��Ant��
                SpawnBugs();
                hasSpawned = true;
            }
        }*/
    }


    public List<Transform> GetValidSpawnPoints(float time)
    {
        List<Transform> valid = new List<Transform>();

        if (rainy && IsRainTime(time)) return spawnPoints.ToList();
        if (cloudy && IsCloudyTime(time)) return spawnPoints.ToList();
        if (morningSunny && IsMorningSunnyTime(time)) return spawnPoints.ToList();
        if (nightSunny && IsNightSunnyTime(time)) return spawnPoints.ToList();

        // Ĭ�����ͣ�ȫ����ֵĳ��ӣ������ϡ�beetle��
        if (!rainy && !cloudy && !morningSunny && !nightSunny)
            return spawnPoints.ToList();

        return valid; // Ĭ�Ͽ��б�
    }


    // ץ�����Ƴ� spawn ��
    public void RemoveSpawnPoint(Transform point)
    {
        var list = spawnPoints.ToList();
        list.Remove(point);
        spawnPoints = list.ToArray();
    }

    // ˢһ�����ӣ��� manager ���ã�
    public GameObject SpawnAtPoint(Transform point)
    {
        return Instantiate(bugPrefab, point.position, point.rotation);
    }


    private bool IsRainTime(float time)
    {
        return (time >= 120f && time < 180f) || (time >= 300f && time < 360f);
    }

    private bool IsCloudyTime(float time)
    {
        return (time >= 60f && time < 120f) || (time >= 240f && time < 300f);
    }

    private bool IsMorningSunnyTime(float time)
    {
        return (time >= 0f && time < 120f) || (time >= 180f && time < 240f);
    }

    private bool IsNightSunnyTime(float time)
    {
        return time >= 360f && time < 450f;
    }


}
