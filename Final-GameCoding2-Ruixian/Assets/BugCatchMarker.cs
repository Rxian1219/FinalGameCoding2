using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugCatchMarker : MonoBehaviour
{

    private SpawnManager manager;
    private Transform originPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init(SpawnManager mgr, Transform spawnPoint)
    {
        manager = mgr;
        originPoint = spawnPoint;
    }

    // 你在其他地方（比如 PlayerController）抓虫成功时调用：
    public void OnCaught()
    {
        manager.OnBugCaught(gameObject, originPoint);
        Destroy(gameObject);
    }

}
