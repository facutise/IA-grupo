using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    // Esta lista contendr� todos los nodos presentes en la escena
    public List<Node> Nodes { get; private set; }

    void Start()
    {
        // Al inicio, obten todos los nodos presentes en la escena
        Nodes = new List<Node>(FindObjectsOfType<Node>());
    }

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        // Implementa el algoritmo A* para encontrar el camino
        // desde la posici�n inicial hasta la posici�n objetivo

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();

        Node startNode = FindClosestNode(startPos);
        Node targetNode = FindClosestNode(targetPos);

        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
                {
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (Node neighbor in currentNode.Connections)
            {
                if (!neighbor.IsWalkable || closedSet.Contains(neighbor))
                {
                    continue;
                }

                int newMovementCostToNeighbor = currentNode.GCost + GetDistance(currentNode, neighbor);

                if (newMovementCostToNeighbor < neighbor.GCost || !openSet.Contains(neighbor))
                {
                    neighbor.GCost = newMovementCostToNeighbor;
                    neighbor.HCost = GetDistance(neighbor, targetNode);
                    neighbor.Parent = currentNode;

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        return null; // No se encontr� un camino v�lido
    }

    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }

        path.Reverse();
        return path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        // Retorna la distancia en nodos entre dos nodos
        return Mathf.RoundToInt(Vector3.Distance(nodeA.Position, nodeB.Position));
    }

    Node FindClosestNode(Vector3 position)
    {
        // Encuentra el nodo m�s cercano a la posici�n dada
        Node closestNode = null;
        float closestDistance = float.MaxValue;

        foreach (Node node in Nodes)
        {
            float distance = Vector3.Distance(position, node.Position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestNode = node;
            }
        }

        return closestNode;
    }
}
