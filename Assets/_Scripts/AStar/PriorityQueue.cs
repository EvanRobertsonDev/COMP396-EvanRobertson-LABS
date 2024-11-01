using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue
{
    private List<Node> nodes = new();

    public int Length => nodes.Count;

    public bool Contains(Node node) => nodes.Contains(node);

    public Node GetFirst()
    {
        if (nodes.Count > 0)
        {
            return nodes[0];
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
