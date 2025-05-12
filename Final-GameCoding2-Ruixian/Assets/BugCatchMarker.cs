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

    // ���������ط������� PlayerController��ץ��ɹ�ʱ���ã�
    public void OnCaught()
    {
        manager.OnBugCaught(gameObject, originPoint);
        Destroy(gameObject);
    }

}
