using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{

    [SerializeField] Transform startPos, endPos;
    Node startNode { get; set; }
    Node endNode { get; set; }

    List<Node> pathArray = new();
    [SerializeField] GameObject objectStart, objectEnd;

    [SerializeField] float elapsedTime = 0f, intervalTime = 1f;

    void Start()
    {
        objectStart = GameObject.FindGameObjectWithTag("Start");
        objectEnd = GameObject.FindGameObjectWithTag("End");

        FindPath();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > intervalTime)
        {
            elapsedTime = 0;
            FindPath();
            if (pathArray.Count > 0)
            {
                Move(pathArray[1].position);
            }
        }
    }

    void FindPath()
    {
        startPos = objectStart.transform;
        endPos = objectEnd.transform;

        startNode = new Node(GridManager.Instance.GetGridCellCenter(
            GridManager.Instance.GetGridIndex(startPos.position)));
        endNode = new Node(GridManager.Instance.GetGridCellCenter(
            GridManager.Instance.GetGridIndex(endPos.position)));

        pathArray = AStar.FindPath(startNode, endNode);
    }

    private void OnDrawGizmos()
    {
        if (pathArray == null) return;
        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.magenta);
                    index++;
                }
            }
        }
    }

    void Move(Vector3 position)
    {
        objectStart.transform.position = position;
    }
}
