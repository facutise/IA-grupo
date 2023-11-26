using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Graph graph;
    public Transform[] patrolPoints;
    private int currentPatrolIndex = 0;
    public float speed = 2f;
    public Transform[] patrolPoints = new Transform[0];

    void Start()
    {
        SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
{
    if (patrolPoints.Length > 0)
    {
        if (Vector3.Distance(transform.position, patrolPoints[currentPatrolIndex].position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }
    else
    {
        Debug.LogWarning("No patrol points assigned to the enemy.");
    }
}

    void SetDestination(Vector3 destination)
    {
        Vector3 currentPosition = transform.position;
        var path = graph.FindPath(currentPosition, destination);

        foreach (Node node in path)
        {
            Vector3 nodePosition = node.Position;
            transform.Translate((nodePosition - currentPosition).normalized * Time.deltaTime * speed);
            currentPosition = transform.position;
        }
    }
}
