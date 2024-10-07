using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public List<Node> neighbors; // Vecinos del nodo
    public bool visited; // Marca si el nodo ha sido visitado o no
    
    void Start()
    {
        neighbors = new List<Node>();
        visited = false;
    }
}