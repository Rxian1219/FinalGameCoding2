using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Butterfly : MonoBehaviour
{

    public float scareDistance = 5f;
    public float flyAwaySpeed = 3f;
    public Transform player;

    private bool hasFlownAway = false;
    private PlayerController playerController;

    private SpawnManager manager;
    private Transform spawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerController = player.GetComponent<PlayerController>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFlownAway || player == null || playerController == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < scareDistance && !playerController.isCrouching)
        {
            StartCoroutine(FlyAway());
        }
    }

    System.Collections.IEnumerator FlyAway()
    {
        hasFlownAway = true;

        float duration = 2f;  
        float timer = 0f;

        Vector3 direction = (transform.position - player.position).normalized + Vector3.up;

        while (timer < duration)
        {
            transform.position += direction * flyAwaySpeed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        
        }

        if (manager != null)
        {
            manager.OnBugEscaped(gameObject); // 新增这行
        }

        Destroy(gameObject);
    }

    public void Init(SpawnManager mgr, Transform point)
    {
        manager = mgr;
        spawnPoint = point;
    }

    public void Catch()
    {
        if (manager != null && spawnPoint != null)
        {
            manager.OnBugCaught(gameObject, spawnPoint);
        }
        Destroy(gameObject); // 抓到才销毁 + 通知 manager
    }

}
