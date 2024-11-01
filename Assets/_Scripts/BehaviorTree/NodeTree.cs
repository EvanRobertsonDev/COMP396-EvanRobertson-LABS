using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeStates
{
    FAILURE, SUCCESS, RUNNING
}

public abstract class NodeTree
{
    public delegate NodeStates NodeReturn();
    protected NodeStates nodeState;

    public NodeStates nodeStates { get { return nodeState; } }

    public NodeTree()
    {

    }

    public abstract NodeStates Evaluate();
}

public class Selector : NodeTree
{
    protected List<NodeTree> nodes = new();
    public Selector(List<NodeTree> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeStates Evaluate()
    {
        foreach (var node in nodes)
        {
            switch(node.Evaluate())
            {
                case NodeStates.FAILURE:
                    continue;
                case NodeStates.SUCCESS:
                    nodeState = NodeStates.SUCCESS;
                    return nodeState;
                case NodeStates.RUNNING:
                    nodeState = NodeStates.RUNNING;
                    return nodeState;
                default:
                    continue;
            }
        }

        nodeState = NodeStates.FAILURE;
        return nodeState;
    }
}

public class Sequence : NodeTree
{
    protected List<NodeTree> nodes = new();
    public Sequence(List<NodeTree> nodes)
    {
        this.nodes = nodes;
    }

    public override NodeStates Evaluate()
    {
        bool anyChildStillRunning = false;

        foreach (var node in nodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.FAILURE:
                    nodeState = NodeStates.FAILURE;
                    return nodeState;
                case NodeStates.SUCCESS:
                    continue;
                case NodeStates.RUNNING:
                    anyChildStillRunning = true;
                    continue;
                default:
                    nodeState = NodeStates.SUCCESS;
                    return nodeState;
            }
        }

        nodeState = anyChildStillRunning ? NodeStates.RUNNING : NodeStates.SUCCESS;
        return nodeState;
    }
}

public class Inverter : NodeTree
{
    private NodeTree node;
    public NodeTree Node { get { return node; } }

    public Inverter(NodeTree node)
    {
        this.node = node;
    }

    public override NodeStates Evaluate()
    {
        switch (node.Evaluate())
        {
            case NodeStates.FAILURE:
                nodeState = NodeStates.SUCCESS;
                break;
            case NodeStates.SUCCESS:
                nodeState = NodeStates.FAILURE;
                break;
            case NodeStates.RUNNING:
                nodeState = NodeStates.RUNNING;
                break;
            default:
                nodeState = NodeStates.SUCCESS;
                break;
        }
        return nodeState;
    }
}

public class ActionNode : NodeTree
{
    public delegate NodeStates ActionNodeDelegate();
    private ActionNodeDelegate nodeAction;

    public ActionNode (ActionNodeDelegate nodeAction)
    {
        this.nodeAction = nodeAction;
    }

    public override NodeStates Evaluate()
    {
        switch (nodeAction())
        {
            case NodeStates.FAILURE:
                nodeState = NodeStates.FAILURE;
                break;
            case NodeStates.SUCCESS:
                nodeState = NodeStates.SUCCESS;
                break;
            case NodeStates.RUNNING:
                nodeState = NodeStates.RUNNING;
                break;
            default:
                nodeState = NodeStates.FAILURE;
                break;
        }

        return nodeState;
    }
}
