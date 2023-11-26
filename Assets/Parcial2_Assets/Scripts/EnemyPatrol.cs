using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public Graph graph; // Referencia al script del grafo
    public Transform[] patrolPoints; // Puntos de patrulla asignados al enemigo
    private int currentPatrolIndex = 0;
    public float speed = 2f;  //definir la velocidad del enemigo
    public Transform[] patrolPoints = new Transform[0]; // Inicializado como un array vacío

    void Start()
    {
        // Inicializar el primer punto de patrulla
        SetDestination(patrolPoints[currentPatrolIndex].position);
    }

    void Update()
{
    // Implementa tu lógica de patrulla aquí
    // ...

    // Verifica si hay al menos un punto de patrulla
    if (patrolPoints.Length > 0)
    {
        // Ejemplo simple: Cambia de destino cuando el enemigo alcanza su destino actual
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
        // Utiliza el algoritmo A* para encontrar el camino desde la posición actual
        // del enemigo hasta el próximo punto de patrulla
        Vector3 currentPosition = transform.position;
        // Llama a la función FindPath del grafo para obtener el camino
        var path = graph.FindPath(currentPosition, destination);

        // Implementa el movimiento a lo largo del camino
        // (Esto puede requerir interpolación suave para un movimiento más natural)
        // ...

        // Por ejemplo, simplemente puedes seguir el camino con transform.Translate:
        foreach (Node node in path)
        {
            Vector3 nodePosition = node.Position;
            transform.Translate((nodePosition - currentPosition).normalized * Time.deltaTime * speed);
            currentPosition = transform.position;

            // Implementa aquí la lógica de rotación o cualquier otra acción
            // que desees realizar durante el movimiento
            // ...
        }
    }
}
