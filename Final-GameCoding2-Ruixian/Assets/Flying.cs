using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : MonoBehaviour
{

    public float flyRadius = 5f;    // 水平半径
    public float minHeight = 1f;    // 最低飞行高度
    public float maxHeight = 3f;    // 最高飞行高度
    public float flySpeed = 2f;     // 飞行速度

    private Vector3 startPoint;
    private Vector3 targetPoint;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = transform.position;
        PickNewTarget();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint, flySpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPoint) < 0.1f)
        {
            PickNewTarget();
        }
    }


    void PickNewTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * flyRadius;
        float randomHeight = Random.Range(minHeight, maxHeight);
        targetPoint = startPoint + new Vector3(randomCircle.x, randomHeight, randomCircle.y);

    }
}
