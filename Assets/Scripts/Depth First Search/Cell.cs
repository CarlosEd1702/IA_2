using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector2 position;
    public Vector2 connection;
    public int fCost;
    public int gCost;
    public int hCost;
    public bool isWall;

    public Cell(Vector2 pos)
    {
        
    }
}
