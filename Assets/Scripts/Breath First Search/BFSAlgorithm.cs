using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BFSAlgorithm : MonoBehaviour
{
    public Node startNode;  // Nodo inicial
    public Node endNode;    // Nodo final
    public float moveSpeed = 2f;  // Velocidad de movimiento del objeto

    private Queue<Node> _queue;   // Cola para el BFS
    private Dictionary<Node, Node> _parentMap;  // Mapa de padres para reconstruir el camino
    private bool _reachedEnd = false;  // Para saber si se alcanzó el nodo final
    private Transform _transform;      // Transform del objeto que se mueve

    void Start()
    {
        _transform = GetComponent<Transform>();
        StartCoroutine(MoveAlongPath());
    }

    private void Bfs(Node start)
    {
        _queue = new Queue<Node>();
        _parentMap = new Dictionary<Node, Node>();
        HashSet<Node> visited = new HashSet<Node>();

        // Inicializar BFS
        visited.Add(start);
        _queue.Enqueue(start);

        while (_queue.Count > 0 && !_reachedEnd)
        {
            Node current = _queue.Dequeue();

            // Si llegamos al nodo final, detenemos la búsqueda
            if (current == endNode)
            {
                _reachedEnd = true;
                break;
            }

            // Recorremos los vecinos del nodo actual
            foreach (Node neighbor in current.neighbors)
            {
                if (!visited.Contains(neighbor))
                {
                    visited.Add(neighbor);
                    _queue.Enqueue(neighbor);
                    _parentMap[neighbor] = current;  // Guardamos el camino
                }
            }
        }
    }

    private IEnumerator MoveAlongPath()
    {
        // Ejecutamos el algoritmo BFS
        Bfs(startNode);

        // Reconstruir el camino desde el nodo final
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = _parentMap[currentNode];
        }

        path.Add(startNode);
        path.Reverse();  // Invertir el camino para que sea de inicio a fin

        // Moverse a lo largo del camino
        foreach (Node node in path)
        {
            yield return StartCoroutine(MoveToNode(node));
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
