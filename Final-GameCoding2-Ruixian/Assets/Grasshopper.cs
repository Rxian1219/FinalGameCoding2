using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grasshopper : MonoBehaviour
{

    public float scareDistance = 5f;
    public float moveSpeed = 3f;

    private Transform player;
    private PlayerController playerController;
    private bool isFleeing = false;

    private SpawnManager manager;
    private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null || playerController == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isFleeing && distance < scareDistance)
        {
            isFleeing = true;
        }

        if (isFleeing)
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        //distory distance
        if (distance > 20f)
        {
            if (manager != null)
            {
                manager.OnBugEscaped(gameObject); 
            }

            Destroy(gameObject);
        }


    }

    public void Catch()
    {
        if (manager != null && spawnPoint != null)
        {
            manager.OnBugCaught(gameObject, spawnPoint);
        }
        Destroy(gameObject);
    }

    public void Init(SpawnManager mgr, Transform point)
    {
        manager = mgr;
        spawnPoint = point;
    }
}
