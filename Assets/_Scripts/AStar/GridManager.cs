using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    static GridManager instance;
    public static GridManager Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindObjectOfType(typeof(GridManager)) as GridManager;
            return instance;
        }
    }

    public int NumOfRows, NumOfColumns;
    public float GridCellSize;
    public bool ShowGrid = true, showObstacleBlocks = true;

    private Vector3 origin = new Vector3(0, 0, 0);
    public Vector3 Origin { get { return origin; } }

    private GameObject[] obstacles;
    public Node[,] Nodes { get; set; }


    private void Awake()
    {
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        CalculateObstacles();
    }

    public int GetRow(int index)
    {
        return index / NumOfColumns;
    }

    public int GetColumn(int index)
    {
        return index % NumOfRows;
    }

    public int GetGridIndex(Vector3 position)
    {
        if (!IsBounds(position))
        {
            return -1;
        }
        position -= Origin;
        int col = (int)(position.x / GridCellSize);
        int row = (int)(position.z / GridCellSize);
        return row * NumOfColumns + col;
    }

    public bool IsBounds(Vector3 position)
    {
        float width = NumOfColumns * GridCellSize;
        float height = NumOfRows * GridCellSize;
        return (position.x >= Origin.x && position.x <= Origin.x + width
            && position.z <= Origin.z + height && position.z >= Origin.z);
    }

    void CalculateObstacles()
    {
        Nodes = new Node[NumOfColumns, NumOfRows];
        int index = 0;

        for (int j = 0; j < NumOfRows; j++)
        {
            for (int i = 0; i < NumOfColumns; i++)
            {
                Vector3 cellPosition = GetGridCellCenter(index);
                Node node = new Node(cellPosition);
                Nodes[i, j] = node;
                index++;

            }
        }
        if (obstacles != null && obstacles.Length > 0)
        {
            foreach (GameObject obstacle in obstacles)
            {
                int indexCell = GetGridIndex(obstacle.transform.position);
                int col = GetColumn(indexCell);
                int row = GetRow(indexCell);
                Nodes[col, row].MarkAsObstacle();
            }
        }
    }

    public Vector3 GetGridCellCenter(int index)
    {
        Vector3 cellPosition = GetGridCellPosition(index);
        cellPosition.x += (GridCellSize / 2.0f);
        cellPosition.z += (GridCellSize / 2.0f);
        return cellPosition;
    }

    public Vector3 GetGridCellPosition(int index)
    {
        int row = GetRow(index);
        int col = GetColumn(index);
        float xPosInGrid = col * GridCellSize;
        float zPosInGrid = row * GridCellSize;
        return Origin + new Vector3(xPosInGrid, 0, zPosInGrid);
    }

    public void GetNeighbours(Node node, List<Node> neighbours)
    {
        Vector3 neighbourPosition = node.position;
        int neighbourIndex = GetGridIndex(neighbourPosition);

        int row = GetRow(neighbourIndex);
        int col = GetColumn(neighbourIndex);

        int leftNodeRow = row - 1;
        int leftNodeCol = col;
        AssignNeighbour(leftNodeRow, leftNodeCol, neighbours);

        leftNodeRow = row + 1;
        leftNodeCol = col;
        AssignNeighbour(leftNodeRow, leftNodeCol, neighbours);

        leftNodeRow = row;
        leftNodeCol = col - 1;
        AssignNeighbour(leftNodeRow, leftNodeCol, neighbours);

        leftNodeRow = row;
        leftNodeCol = col + 1;
        AssignNeighbour(leftNodeRow, leftNodeCol, neighbours);

    }

    void AssignNeighbour(int row, int col, List<Node> neighbours)
    {
        if (row != -1 && col != -1 && row < NumOfRows && col < NumOfColumns)
        {
            Node nodeToAdd = Nodes[col, row]; ;
            if (!nodeToAdd.obstacle)
            {
                neighbours.Add(nodeToAdd);
            }
        } 
    }

    private void OnDrawGizmos()
    {
        if (ShowGrid)
        {
            DebugDrawGrid(transform.position, NumOfRows, NumOfColumns, GridCellSize, Color.blue);
        }

        Gizmos.DrawSphere(transform.position, 0.5f);

        if (showObstacleBlocks)
        {
            Vector3 cellSize = new Vector3(GridCellSize, 1.0f, GridCellSize);
            if (obstacles != null && obstacles.Length > 0)
            {
                foreach (GameObject obstacle in obstacles)
                {
                    Gizmos.DrawCube(GetGridCellCenter(GetGridIndex(obstacle.transform.position)), cellSize);
                }
            }
        }
    }

    public void DebugDrawGrid(Vector3 position, int rows, int cols, float cellSize, Color color)
    {
        float width = cols * cellSize;
        float height = rows * cellSize;

        for (int i = 0; i < rows + 1; i++)
        {
            Vector3 startPos = position + i * cellSize * Vector3.forward;
            Vector3 endPos = startPos + width * Vector3.right;
            Debug.DrawLine(startPos, endPos, color);
        }
        for (int i = 0; i < cols + 1; i++)
        {
            Vector3 startPos = position + i * cellSize * Vector3.right;
            Vector3 endPos = startPos + height * Vector3.forward;
            Debug.DrawLine(startPos, endPos, color);
        }
    }
    
}
