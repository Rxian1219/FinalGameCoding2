using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flying : MonoBehaviour
{

    public float flyRadius = 5f;    // ˮƽ�뾶
    public float minHeight = 1f;    // ��ͷ��и߶�
    public float maxHeight = 3f;    // ��߷��и߶�
    public float flySpeed = 2f;     // �����ٶ�

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
