using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Vector3 Position { get { return transform.position; } }
    public bool IsWalkable { get; set; } = true;  // Por defecto, se considera caminable
    public List<Node> Connections { get; private set; } = new List<Node>();  // Lista de nodos vecinos
    public Node Parent { get; set; }  // Utilizado para el algoritmo A*

    // Puedes agregar m�s propiedades seg�n sea necesario para tu proyecto

    public int GCost { get; set; }  // Costo acumulado desde el nodo inicial hasta este nodo
    public int HCost { get; set; }  // Estimaci�n del costo desde este nodo hasta el nodo objetivo
    public int FCost { get { return GCost + HCost; } }  // Suma de GCost y HCost

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
