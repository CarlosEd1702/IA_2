using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarAlgorithm : MonoBehaviour
{
    public Node startNode;  // Nodo inicial
    public Node endNode;    // Nodo final
    public float moveSpeed = 2f;  // Velocidad de movimiento del objeto

    private Transform _transform;      // Transform del objeto que se mueve
    private List<Node> _openList;      // Lista de nodos a explorar
    private HashSet<Node> _closedList; // Lista de nodos ya explorados
    private Dictionary<Node, Node> _parentMap;  // Mapa de padres para reconstruir el camino

    void Start()
    {
        _transform = GetComponent<Transform>();
        StartCoroutine(MoveAlongPath());
    }

    // Calcula la distancia heurística entre dos nodos (usamos distancia euclidiana)
    private float Heuristic(Node a, Node b)
    {
        return Vector3.Distance(a.transform.position, b.transform.position);
    }

    private List<Node> AStarSearch(Node start, Node goal)
    {
        // Inicializamos listas y diccionarios
        _openList = new List<Node>();
        _closedList = new HashSet<Node>();
        _parentMap = new Dictionary<Node, Node>();

        Dictionary<Node, float> gCost = new Dictionary<Node, float>();  // Costo desde el nodo inicial
        Dictionary<Node, float> fCost = new Dictionary<Node, float>();  // Costo total estimado (G + H)

        _openList.Add(start);
        gCost[start] = 0f;
        fCost[start] = Heuristic(start, goal);

        while (_openList.Count > 0)
        {
            // Obtener el nodo con el menor F cost
            Node currentNode = GetLowestFCostNode(_openList, fCost);

            // Si llegamos al nodo final
            if (currentNode == goal)
            {
                return ReconstructPath(goal);
            }

            _openList.Remove(currentNode);
            _closedList.Add(currentNode);

            // Explorar vecinos
            foreach (Node neighbor in currentNode.neighbors)
            {
                if (_closedList.Contains(neighbor))
                {
                    continue;  // Si ya está en la lista cerrada, lo ignoramos
                }

                float tentativeGCost = gCost[currentNode] + Vector3.Distance(currentNode.transform.position, neighbor.transform.position);

                if (!_openList.Contains(neighbor))
                {
                    _openList.Add(neighbor);  // Añadimos el vecino a la lista abierta
                }
                else if (tentativeGCost >= gCost[neighbor])
                {
                    continue;  // Si este no es un mejor camino, lo ignoramos
                }

                // Este es el mejor camino por ahora, actualizamos los costos y el mapa de padres
                _parentMap[neighbor] = currentNode;
                gCost[neighbor] = tentativeGCost;
                fCost[neighbor] = gCost[neighbor] + Heuristic(neighbor, goal);
            }
        }

        return null;  // Si no se encuentra un camino
    }

    // Encuentra el nodo en la lista abierta con el menor F cost
    private Node GetLowestFCostNode(List<Node> openList, Dictionary<Node, float> fCost)
    {
        Node lowestNode = openList[0];
        float lowestFCost = fCost[lowestNode];

        foreach (Node node in openList)
        {
            float currentFCost = fCost[node];
            if (currentFCost < lowestFCost)
            {
                lowestFCost = currentFCost;
                lowestNode = node;
            }
        }

        return lowestNode;
    }

    // Reconstruye el camino desde el nodo final hasta el inicial usando el mapa de padres
    private List<Node> ReconstructPath(Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (_parentMap.ContainsKey(currentNode))
        {
            path.Add(currentNode);
            currentNode = _parentMap[currentNode];
        }

        path.Add(startNode);
        path.Reverse();  // Invertimos el camino para que sea de inicio a fin
        return path;
    }

    private IEnumerator MoveAlongPath()
    {
        // Ejecutamos el algoritmo A*
        List<Node> path = AStarSearch(startNode, endNode);

        if (path != null)
        {
            // Moverse a lo largo del camino
            foreach (Node node in path)
            {
                yield return StartCoroutine(MoveToNode(node));
            }
        }
    }

    private IEnumerator MoveToNode(Node targetNode)
    {
        // Movemos el objeto 3D hacia el siguiente nodo
        while (Vector3.Distance(_transform.position, targetNode.transform.position) > 0.1f)
        {
            _transform.position = Vector3.MoveTowards(
                _transform.position,
                targetNode.transform.position,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }
    }
}
