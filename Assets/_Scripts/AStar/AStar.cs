using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    public static PriorityQueue closedList, openList;

    static float HeuristicEstimateCost(Node currentNode, Node goalNode)
    {
        Vector3 vectorCost = currentNode.position - goalNode.position;
        return vectorCost.magnitude;
    }

    static List<Node> CalculatePath(Node node)
    {
        List<Node> list = new();
        while(node != null)
        {
            list.Add(node);
            node = node.parent;
        }
        list.Reverse();
        return list;
    }

    public static List<Node> FindPath(Node startNode, Node endNode)
    {
        openList = new PriorityQueue();
        openList.Add(startNode);
        startNode.nodeTotalCost = 0f;
        startNode.estimateCost = HeuristicEstimateCost(startNode, endNode);

        closedList = new PriorityQueue();
        Node node = null;
        while (openList.Length != 0)
        {
            node = openList.GetFirst();
            if (node.position == endNode.position)
            {
                return CalculatePath(node);
            }

            List<Node> neighbours = new();
            GridManager.Instance.GetNeighbours(node, neighbours);

            for (int index = 0; index < neighbours.Count; index++)
            {
                Node neighbourNode = neighbours[index];
                if (!closedList.Contains(neighbourNode))
                {
                    float cost = HeuristicEstimateCost(node, neighbourNode);
                    float totalCost = node.nodeTotalCost + cost;
                    float neighbourNodeCost = HeuristicEstimateCost(neighbourNode, endNode);

                    neighbourNode.nodeTotalCost = totalCost;
                    neighbourNode.parent = node;
                    neighbourNode.estimateCost = totalCost + neighbourNodeCost;

                    if(!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }

            closedList.Add(node);
            openList.Remove(node);
        }

        if (node.position != endNode.position)
        {
            return null;
        }

        return CalculatePath(node);
    }
} 
