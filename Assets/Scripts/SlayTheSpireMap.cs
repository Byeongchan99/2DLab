using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEdge
{
    public Vector2Int StartPoint { get; set; }
    public Vector2Int EndPoint { get; set; }

    public MapEdge(Vector2Int startPoint, Vector2Int endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
    }
}

public class Path
{
    public List<MapEdge> Edges { get; set; }

    public Path()
    {
        Edges = new List<MapEdge>();
    }

    public void AddEdge(MapEdge edge)
    {
        Edges.Add(edge);
    }
}

public class SlayTheSpireMap : MonoBehaviour
{
    [SerializeField] int width = 7, height = 15; // ���� ���� ����, ���� ���� ����
    [SerializeField] int startPointCount; // 2���� 4 ������ ������ ��� ���� ����
    [SerializeField] HashSet<int> startPoints; // ��� ����
    [SerializeField] Path[] paths = new Path[6]; // ��� 6��

    public GameObject lineRendererPrefab; // ���� ������ ������
    [SerializeField] private Color[] pathColors; // �ν����Ϳ��� ������ ���� �迭

    public GameObject pointPrefab; // ���� ������
    [SerializeField] private Transform pointHoler; // ���� �������� ��� �θ� ���� ������Ʈ
    public int[,] points = new int[7, 15]; // ���� ��� ��ǥ�� ���� ������ ��� �迭

    private HashSet<int> GenerateStartPoints(int numberOfPoints)
    {
        HashSet<int> startPoints = new HashSet<int>();
        while (startPoints.Count < numberOfPoints)
        {
            int randomIndex = Random.Range(0, 7); // 7���� ������ ��� ���� �� ���� ����
            startPoints.Add(randomIndex);
        }
        return startPoints;
    }

    // ��� ������ �������� ����
    private int choiceRandomStartPoint()
    {
        int randomIndex = Random.Range(0, startPointCount); // 0���� startPointCount - 1 ������ ���� ���� ����
        int currentIndex = 0;

        foreach (int startPoint in startPoints)
        {
            if (currentIndex == randomIndex)
            {
                return startPoint; // ���õ� ���� �ε����� �ش��ϴ� ��� ���� ��ȯ
            }
            currentIndex++;
        }

        return -1; // ���� ��Ȳ, �Ϲ������� �߻����� ����
    }

    // path���� ��ȸ�ϸ鼭 Edge�� �ִ��� Ȯ��
    private bool checkCross(Vector2Int start, Vector2Int end)
    {
        Debug.Log("checkCross: " + start + ", " + end);
        MapEdge newEdge = new MapEdge(start, end);

        // ��� ��ο� ���� newEdge�� �����ϴ��� Ȯ��
        foreach (Path path in paths)
        {
            if (path != null && path.Edges != null)
            {
                foreach (MapEdge pathEdge in path.Edges)
                {
                    // �������� ������ ��� ������ Ȯ��
                    if (pathEdge.StartPoint == newEdge.StartPoint && pathEdge.EndPoint == newEdge.EndPoint)
                    {
                        Debug.Log("checkCross: true");
                        return true; // ������ Edge �߰�
                    }
                }
            }
        }

        Debug.Log("checkCross: false");
        return false; // ������ Edge ����
    }

    // ���� ���� ����
    private Vector2Int ChooseNextPosition(int currentX, int currentY)
    {
        Debug.Log("ChooseNextPosition ���� ��ġ: " + currentX + ", " + currentY);
        List<Vector2Int> possiblePositions = new List<Vector2Int>();

        // ���� �밢�� ���� �� ���� ������ Ȯ��
        if (currentX - 1 > -1 && currentY + 1 < height) 
        {
            Debug.Log("���� �밢�� Ȯ��");
            if (!checkCross(new Vector2Int(currentX - 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX - 1, currentY + 1));
            }
        }

        // �ٷ� ���� �� ���� ������ Ȯ��
        if (currentY + 1 < height)
        {
            Debug.Log("�ٷ� �� Ȯ��");
            possiblePositions.Add(new Vector2Int(currentX, currentY + 1));
        }

        // ������ �밢�� ���� �� ���� ������ Ȯ��
        if (currentX + 1 < width && currentY + 1 < height) 
        {
            Debug.Log("������ �밢�� Ȯ��");  
            if (!checkCross(new Vector2Int(currentX + 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX + 1, currentY + 1));
            }
        }

        // �����ϰ� �ϳ��� ��ġ ����
        if (possiblePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            Debug.Log("ChooseNextPosition: " + possiblePositions[randomIndex]);
            return possiblePositions[randomIndex];
        }

        // ���� ó��: ��ȿ�� ��ġ�� ���� ���
        return new Vector2Int(-1, -1);
    }

    // ��� ����
    private void GeneratePath(int pathIndex)
    {
        Debug.Log("GeneratePath: " + pathIndex);

        // paths �迭�� �ش� �ε����� Path ��ü�� ������ ���� ����
        if (paths[pathIndex] == null)
        {
            paths[pathIndex] = new Path();
        }

        int currentX = choiceRandomStartPoint(); // ��� ���� ����
        Debug.Log("currentX: " + currentX);

        Path path = paths[pathIndex]; // ��� ��ü ����

        int currentY = 0;
        while (currentY < height - 1)
        {
            Vector2Int nextPosition = ChooseNextPosition(currentX, currentY);
            Debug.Log("nextPosition: " + nextPosition);
            if (nextPosition.x == -1 && nextPosition.y == -1) // ���� ó��
                break;

            path.AddEdge(new MapEdge(new Vector2Int(currentX, currentY), nextPosition));
            points[currentX, currentY] = 1; // points �迭�� ��ΰ� ������ ��ǥ�� 1�� ǥ��

            currentX = nextPosition.x;
            currentY = nextPosition.y;
        }
    }

    // ���� �������� ��� �׸���
    private void DrawPathsWithLineRenderer()
    {
        int colorIndex = 0;

        foreach (Path path in paths)
        {
            GameObject lineObj = Instantiate(lineRendererPrefab, this.transform);
            LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

            // ���� ����
            Color pathColor = pathColors[colorIndex % pathColors.Length];
            lineRenderer.startColor = pathColor;
            lineRenderer.endColor = pathColor;

            // ���� ���� ������ ���� �ε��� ����
            colorIndex++;

            lineRenderer.positionCount = path.Edges.Count * 2;
            int positionIndex = 0;

            foreach (MapEdge edge in path.Edges)
            {
                lineRenderer.SetPosition(positionIndex, new Vector3(edge.StartPoint.x, edge.StartPoint.y, 0));
                positionIndex++;
                lineRenderer.SetPosition(positionIndex, new Vector3(edge.EndPoint.x, edge.EndPoint.y, 0));
                positionIndex++;
            }
        }
    }

    void Start()
    {
        startPointCount = Random.Range(2, 5); // �������� ��� ���� ���� ����
        startPoints = GenerateStartPoints(startPointCount);

        // paths �迭 �ʱ�ȭ
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = new Path();
        }

        // ���� ��� ��ǥ�� pointPrefab ��ġ
        PlacePointsForMap();

        // ��� 6�� ����
        for (int i = 0; i < 6; i++)
        {
            GeneratePath(i);
        }

        // ���� �������� ��� �׸���
        DrawPathsWithLineRenderer();
    }

    // ���� ��� ��ǥ�� pointPrefab ��ġ
    private void PlacePointsForMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Instantiate(pointPrefab, new Vector3(x, y, 0), Quaternion.identity, pointHoler);
            }
        }
    }

    // ��ΰ� ������ �ʴ� ������ �ı�
    public void DestroyPoint()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (!points[x, y].Equals(1))
                {
                    Destroy(pointHoler.GetChild(x * height + y).gameObject);
                }
            }
        }
    }
}
