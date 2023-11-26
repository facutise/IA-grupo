using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 Position { get { return transform.position; } }
    public bool IsWalkable { get; set; } = true;  // Por defecto, se considera caminable
    public List<Node> Connections { get; private set; } = new List<Node>();  // Lista de nodos vecinos
    public Node Parent { get; set; }  // Utilizado para el algoritmo A*

    public int GCost { get; set; }
    public int HCost { get; set; }
    public int FCost { get { return GCost + HCost; } }

    void OnDrawGizmos()
    {
        Gizmos.color = IsWalkable ? Color.green : Color.red;
        Gizmos.DrawSphere(transform.position, 0.2f);

        foreach (Node connection in Connections)
        {
            Gizmos.DrawLine(transform.position, connection.transform.position);
        }
    }
}
