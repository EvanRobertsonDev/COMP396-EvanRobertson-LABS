using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private ArrayList nodes = new ArrayList();

    public int Length => nodes.Count;

    public bool Contains(object node) => nodes.Contains(node);

    public Node GetFirst()
    {
        if (nodes.Count > 0)
        {
            return (Node)nodes[0];
        }
        return null;
    }

    public void Add(Node node)
    {
        nodes.Add(node);
        nodes.Sort();
    }

    public void Remove(Node node)
    {
        nodes.Remove(node);
        nodes.Sort();
    }
}
