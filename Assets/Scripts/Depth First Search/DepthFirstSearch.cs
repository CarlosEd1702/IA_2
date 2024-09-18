using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Depth_First_Search
{
    public class DepthFirstSearch : MonoBehaviour
    {
        public Node startNode;  // Nodo inicial
        public Node endNode;    // Nodo final
        public float moveSpeed = 2f;  // Velocidad de movimiento del objeto 3D

        private bool _reachedEnd = false;  // Para saber si se alcanzó el nodo final
        private Stack<Node> _path;         // Pila para seguir el camino recorrido
        private Transform _transform;      // El transform del objeto 3D

        void Start()
        {
            // Obtenemos el componente Transform del objeto 3D
            _transform = GetComponent<Transform>();

            // Inicializamos la pila y comenzamos en el nodo inicial
            _path = new Stack<Node>();
            StartCoroutine(MoveAlongPath());
        }

        // Método Depth-First Search (con convención de nombres ajustada)
        private void Dfs(Node node)
        {
            // Si ya hemos alcanzado el nodo final o si el nodo ya fue visitado, no seguimos buscando
            if (_reachedEnd || node.visited)
                return;

            // Marcamos el nodo como visitado
            node.visited = true;
            _path.Push(node);  // Guardamos el nodo en la pila para saber el camino
            Debug.Log("Visitando nodo: " + node.gameObject.name);

            // Si hemos llegado al nodo final, detenemos la búsqueda
            if (node == endNode)
            {
                _reachedEnd = true;
                return;
            }

            // Recorremos los vecinos
            foreach (Node neighbor in node.neighbors)
            {
                if (!neighbor.visited)
                {
                    Dfs(neighbor);  // Llamada recursiva al vecino
                }
            }

            // Si no es el nodo final, sacamos el nodo de la pila
            if (!_reachedEnd)
                _path.Pop();
        }

        private IEnumerator MoveAlongPath()
        {
            // Ejecutamos la búsqueda en profundidad desde el nodo inicial
            Dfs(startNode);

            // Mientras haya nodos en el camino y no hayamos llegado al nodo final
            while (_path.Count > 0 && !_reachedEnd)
            {
                Node nextNode = _path.Pop();
                yield return StartCoroutine(MoveToNode(nextNode));

                // Si llegamos al nodo final, salimos del bucle
                if (nextNode == endNode)
                {
                    _reachedEnd = true;
                    Debug.Log("Nodo final alcanzado: " + nextNode.gameObject.name);
                    break;
                }
            }
        }

        private IEnumerator MoveToNode(Node targetNode)
        {
            // Movemos el objeto 3D hacia el siguiente nodo
            while (Vector3.Distance(_transform.position, targetNode.transform.position) > 0.1f)
            {
                // Movemos el Transform usando MoveTowards
                _transform.position = Vector3.MoveTowards(
                    _transform.position, 
                    targetNode.transform.position, 
                    moveSpeed * Time.deltaTime
                );
                yield return null;
            }
        }
    }
}
