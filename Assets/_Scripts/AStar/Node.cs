using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : IComparable
{
    public float nodeTotalCost, estimateCost;
    public bool obstacle;
    public Node parent;
    public Vector3 position;

    public Node(Vector3 position)
    {
        estimateCost = 0;
        nodeTotalCost = 0;
        obstacle = false;
        parent = null;
        this.position = position;
    }

    public void MarkAsObstacle()
    {
        obstacle = true;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;
        if (estimateCost < node.estimateCost) return -1;
        if (estimateCost > node.estimateCost) return 1;
        return 0;
    }
}
