using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolPoints : MonoBehaviour
{
    [SerializeField] private Transform[] points;

    private int currentPointIndex;

    [SerializeField] public float speed;
    // Start is called before the first frame update
    void Start()
    {
        currentPointIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position != points[currentPointIndex].position)
        {
            transform.position = Vector3.MoveTowards(transform.position, points[currentPointIndex].position,
                speed * Time.deltaTime);
        }

        else
        {
            currentPointIndex = (currentPointIndex + 1) % points.Length;
        }
    }
}
