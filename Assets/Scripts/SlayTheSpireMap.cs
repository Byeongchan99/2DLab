using JetBrains.Annotations;
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
    public enum PointType
    {
        Empty,
        Visited,
        Monster,
        Event,
        Elite,
        Rest,
        Merchant,
        Treasure
    }
    [SerializeField] int monsterCount, eventCount, eliteCount, restCount, merchantCount; // �̺�Ʈ, ����Ʈ, �޽�, ���� �� ����
    public GameObject[] pointIconPrefabs; // ������ ������
    [SerializeField] private Transform iconHoler;

    [SerializeField] int width = 7, height = 15; // ���� ���� ����, ���� ���� ����
    [SerializeField] int startPointCount; // 2���� 4 ������ ������ ��� ���� ����
    [SerializeField] HashSet<int> startPoints; // ��� ����
    [SerializeField] Path[] paths = new Path[6]; // ��� 6��

    public GameObject lineRendererPrefab; // ���� ������ ������
    [SerializeField] private Transform lineHolder; // ���� ������ �������� ��� �θ� ���� ������Ʈ
    [SerializeField] private Color[] pathColors; // �ν����Ϳ��� ������ ���� �迭

    public GameObject pointPrefab; // ���� ������
    public int totalPointCount; // ���� ��� ��ǥ ����
    [SerializeField] private Transform pointHolder; // ���� �����յ��� ��� ������Ʈ
    [SerializeField] private GameObject pointHolderObject; // ���� �������� ��� ���� ������Ʈ
    public PointType[,] points = new PointType[7, 15]; // ���� ��� ��ǥ�� ���� ������ ��� �迭
    public PointType[,] originPoints = new PointType[7, 15];

    public bool allRulesSatisfied = false; // ��� ��Ģ�� �����Ǿ����� Ȯ��

    private HashSet<int> GenerateStartPoints(int numberOfPoints)
    {
        HashSet<int> startPoints = new HashSet<int>();
        int randomIndex;

        switch (numberOfPoints)
        {
            case 2: // ��� ������ 2���� ��� 0 ~ 2, 3 ~ 6���� �ϳ��� ����
                randomIndex = Random.Range(0, 3);
                startPoints.Add(randomIndex);
                randomIndex = Random.Range(3, 7);
                startPoints.Add(randomIndex);
                break;
            case 3: // ��� ������ 3���� ��� 0 ~ 1, 2 ~ 4, 5 ~ 6���� �ϳ��� ����
                randomIndex = Random.Range(0, 2);
                startPoints.Add(randomIndex);
                randomIndex = Random.Range(2, 5);
                startPoints.Add(randomIndex);
                randomIndex = Random.Range(2, 7);
                startPoints.Add(randomIndex);
                break;
            case 4: // ��� ������ 4���� ���
                while (startPoints.Count < numberOfPoints)
                {
                    randomIndex = Random.Range(0, 7); // 7���� ������ ��� ���� �� ���� ����
                    startPoints.Add(randomIndex);
                }
                break;
            default:
                break;
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
        //Debug.Log("checkCross: " + start + ", " + end);
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
                        //Debug.Log("checkCross: true");
                        return true; // ������ Edge �߰�
                    }
                }
            }
        }

        //Debug.Log("checkCross: false");
        return false; // ������ Edge ����
    }

    // ���� ���� ����
    private Vector2Int ChooseNextPosition(int currentX, int currentY)
    {
        //Debug.Log("ChooseNextPosition ���� ��ġ: " + currentX + ", " + currentY);
        List<Vector2Int> possiblePositions = new List<Vector2Int>();

        // ���� �밢�� ���� �� ���� ������ Ȯ��
        if (currentX - 1 > -1 && currentY + 1 < height)
        {
            //Debug.Log("���� �밢�� Ȯ��");
            if (!checkCross(new Vector2Int(currentX - 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX - 1, currentY + 1));
            }
        }

        // �ٷ� ���� �� ���� ������ Ȯ��
        if (currentY + 1 < height)
        {
            //Debug.Log("�ٷ� �� Ȯ��");
            possiblePositions.Add(new Vector2Int(currentX, currentY + 1));
        }

        // ������ �밢�� ���� �� ���� ������ Ȯ��
        if (currentX + 1 < width && currentY + 1 < height)
        {
            //Debug.Log("������ �밢�� Ȯ��");  
            if (!checkCross(new Vector2Int(currentX + 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX + 1, currentY + 1));
            }
        }

        // �����ϰ� �ϳ��� ��ġ ����
        if (possiblePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            //Debug.Log("ChooseNextPosition: " + possiblePositions[randomIndex]);
            return possiblePositions[randomIndex];
        }

        // ���� ó��: ��ȿ�� ��ġ�� ���� ���
        return new Vector2Int(-1, -1);
    }

    // ��� ����
    private void GeneratePath(int pathIndex)
    {
        //Debug.Log("GeneratePath: " + pathIndex);

        // paths �迭�� �ش� �ε����� Path ��ü�� ������ ���� ����
        if (paths[pathIndex] == null)
        {
            paths[pathIndex] = new Path();
        }

        int currentX = choiceRandomStartPoint(); // ��� ���� ����
        //Debug.Log("currentX: " + currentX);

        Path path = paths[pathIndex]; // ��� ��ü ����

        int currentY = 0;
        while (currentY < height)
        {
            Vector2Int nextPosition = ChooseNextPosition(currentX, currentY);
            //Debug.Log("nextPosition: " + nextPosition);
            if (nextPosition.x == -1 && nextPosition.y == -1) // ���� ó��
                break;

            path.AddEdge(new MapEdge(new Vector2Int(currentX, currentY), nextPosition));
            points[currentX, currentY] = PointType.Visited; // points �迭�� ��ΰ� ������ ��ǥ�� 1�� ǥ��

            currentX = nextPosition.x;
            currentY = nextPosition.y;
        }
        points[currentX, currentY] = PointType.Visited;
    }

    // ���� �������� ��� �׸���
    private void DrawPathsWithLineRenderer()
    {
        int colorIndex = 0;

        foreach (Path path in paths)
        {
            GameObject lineObj = Instantiate(lineRendererPrefab, lineHolder);
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

    // ���� ��� ��ǥ�� pointPrefab ��ġ
    private void PlacePointsForMap()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Instantiate(pointPrefab, new Vector3(x, y, 0), Quaternion.identity, pointHolder);
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
                if (points[x, y] == PointType.Empty)
                {
                    Destroy(pointHolder.GetChild(x * height + y).gameObject);
                }
                else
                {
                    totalPointCount++; // ���� ��� ��ǥ ���� ����
                }
            }
        }
    }

    // ������ ���� ��ġ
    private void AssignFixedRoom()
    {
        // 1���� ��� ���͹�
        for (int i = 0; i < 7; i++)
        {
            if (points[i, 0] == PointType.Empty)
                continue;
            if (points[i, 0] == PointType.Visited)
            {
                points[i, 0] = PointType.Monster;
                totalPointCount--; // ù �� ���͹��� ����
            }
        }

        // 9���� ��� ������
        for (int i = 0; i < 7; i++)
        {
            if (points[i, 8] == PointType.Empty)
                continue;
            if (points[i, 8] == PointType.Visited)
            {
                points[i, 8] = PointType.Treasure;
                totalPointCount--; // 9�� �������� ����
            }
        }

        // 15���� ��� �޽Ĺ�
        for (int i = 0; i < 7; i++)
        {
            if (points[i, 14] == PointType.Empty)
                continue;
            if (points[i, 14] == PointType.Visited)
            {
                points[i, 14] = PointType.Rest;
                totalPointCount--; // �������� �޽Ĺ��� ����
            }
        }
    }

    // �� �������� ���� ���
    private void calculateRoomType()
    {
        // �̺�Ʈ 22%, ����Ʈ 16%, �޽� 12%, ���� 5%
        Debug.Log("������ ���� totalPointCount: " + totalPointCount);
        eventCount = (int)Mathf.Round(totalPointCount * 0.22f);
        eliteCount = (int)Mathf.Round(totalPointCount * 0.16f);
        restCount = (int)Mathf.Round(totalPointCount * 0.12f);
        merchantCount = (int)Mathf.Round(totalPointCount * 0.05f);
        monsterCount = totalPointCount - eventCount - eliteCount - restCount - merchantCount;
        Debug.Log("eventCount: " + eventCount);
        Debug.Log("eliteCount: " + eliteCount);
        Debug.Log("restCount: " + restCount);
        Debug.Log("merchantCount: " + merchantCount);
        Debug.Log("monsterCount: " + monsterCount);
    }   

    // ���� ���� Ÿ�� ã��
    private List<PointType> FindPrevRoomType(int y, int x)
    {
        Vector2Int currentPoint = new Vector2Int(x, y);
        List<PointType> list = new List<PointType>();

        // ��� ��ο� ���� ���� ������ �����ϴ��� Ȯ��
        foreach (Path path in paths)
        {
            if (path != null && path.Edges != null)
            {
                foreach (MapEdge pathEdge in path.Edges)
                {
                    // ������ ���� �������� Ȯ��
                    if (pathEdge.EndPoint == currentPoint)
                    {
                        // �������� Ÿ���� ����Ʈ�� �߰�
                        // ���͹�� �̺�Ʈ���� ����
                        if (points[pathEdge.StartPoint.x, pathEdge.StartPoint.y] != PointType.Monster && points[pathEdge.StartPoint.x, pathEdge.StartPoint.y] != PointType.Event)
                            list.Add(points[pathEdge.StartPoint.x, pathEdge.StartPoint.y]);                                          
                    }
                }
            }
        }
        return list;
    }

    // ��Ģ�� ���� ���� ���� ��ġ
    private void AssignRestRoom()
    {
        int monsterCountTmp = monsterCount, eventCountTmp = eventCount, eliteCountTmp = eliteCount, restCountTmp = restCount, merchantCountTmp = merchantCount;

        Dictionary<PointType, int> pointTypeCounts = new Dictionary<PointType, int>
        {
            { PointType.Monster, monsterCountTmp },
            { PointType.Event, eventCountTmp },
            { PointType.Elite, eliteCountTmp },
            { PointType.Rest, restCountTmp },
            { PointType.Merchant, merchantCountTmp }
        };

        // 13�� ���� �޽Ĺ� �����ϰ� ��ġ
        for (int x = 0; x < width; x++) 
        {
            if (points[x, 13] == PointType.Empty)
                continue;

            // �̹� ���� ������ ��� �ǳʶٱ�
            if (points[x, 13] != PointType.Visited)
            {
                continue;
            }

            // ��� ������ �� Ÿ�� ����Ʈ ����
            List<PointType> availableRoomsIn13Floor = new List<PointType>();

            foreach (var roomType in pointTypeCounts.Keys)
            {
                if (pointTypeCounts[roomType] > 0 && roomType != PointType.Rest)
                {
                    for (int i = 0; i < pointTypeCounts[roomType]; i++)
                        availableRoomsIn13Floor.Add(roomType);
                }
            }

            // ���õ� �� Ÿ�� ��ġ
            if (availableRoomsIn13Floor.Count > 0)
            {
                PointType selectedRoom = availableRoomsIn13Floor[Random.Range(0, availableRoomsIn13Floor.Count)];
                //Debug.Log("selectedRoom: " + selectedRoom);
                points[x, 13] = selectedRoom;
                pointTypeCounts[selectedRoom]--;
            }
            
        }

        monsterCountTmp = pointTypeCounts[PointType.Monster];
        eventCountTmp = pointTypeCounts[PointType.Event];
        eliteCountTmp = pointTypeCounts[PointType.Elite];
        merchantCountTmp = pointTypeCounts[PointType.Merchant];

        // 2������ 6������ ��ġ
        for (int y = 1; y < 6; y++) 
        {
            for (int x = 0; x < width; x++)
            {
                if (points[x, y] == PointType.Empty)
                    continue;
                // �̹� ���� ������ ��� �ǳʶٱ�
                if (points[x, y] != PointType.Visited)
                    continue;

                // ���� ���� Ÿ�� ã��
                List<PointType> prevRoomTypeList = FindPrevRoomType(y, x);

                // ��� ������ �� Ÿ�� ����Ʈ ����
                List<PointType> availableRooms = new List<PointType>();
               
                foreach (var roomType in pointTypeCounts.Keys)
                {
                    // ����Ʈ��� ��ںҹ��� 6�� ���Ͽ� ������ �� ����
                    if (roomType == PointType.Elite || roomType == PointType.Rest)
                        continue;

                    // ���� ��� ������ Ÿ���� ����
                    if (prevRoomTypeList.Contains(roomType))
                        continue;
              
                    if (pointTypeCounts[roomType] > 0)
                    {
                        for (int i=0; i < pointTypeCounts[roomType]; i++)
                            availableRooms.Add(roomType);
                    }
                }

                // ���õ� �� Ÿ�� ��ġ
                if (availableRooms.Count > 0)
                {
                    PointType selectedRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                    points[x, y] = selectedRoom;
                    pointTypeCounts[selectedRoom]--;
                }
                
            }
        }

        // ���������� ���� ���� ������Ʈ
        monsterCountTmp = pointTypeCounts[PointType.Monster];
        eventCountTmp = pointTypeCounts[PointType.Event];
        eliteCountTmp = pointTypeCounts[PointType.Elite];
        restCountTmp = pointTypeCounts[PointType.Rest];
        merchantCountTmp = pointTypeCounts[PointType.Merchant];

        for (int y = 6; y < 13; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (points[x, y] == PointType.Empty)
                    continue;
                // �̹� ���� ������ ��� �ǳʶٱ�
                if (points[x, y] != PointType.Visited)
                    continue;

                // ���� ���� Ÿ�� ã��
                List<PointType> prevRoomTypeList = FindPrevRoomType(y, x);

                // ��� ������ �� Ÿ�� ����Ʈ ����
                List<PointType> availableRooms = new List<PointType>();

                foreach (var roomType in pointTypeCounts.Keys)
                {                 
                    // ���� ��� ������ Ÿ���� ����
                    if (prevRoomTypeList.Contains(roomType))
                        continue;

                    if (pointTypeCounts[roomType] > 0)
                    {
                        for (int i = 0; i < pointTypeCounts[roomType]; i++)
                            availableRooms.Add(roomType);
                    }
                }

                // ���õ� �� Ÿ�� ��ġ
                if (availableRooms.Count > 0)
                {
                    PointType selectedRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                    points[x, y] = selectedRoom;
                    pointTypeCounts[selectedRoom]--;
                }
                
            }
        }

        // ���������� ���� ���� ������Ʈ
        monsterCountTmp = pointTypeCounts[PointType.Monster];
        eventCountTmp = pointTypeCounts[PointType.Event];
        eliteCountTmp = pointTypeCounts[PointType.Elite];
        restCountTmp = pointTypeCounts[PointType.Rest];
        merchantCountTmp = pointTypeCounts[PointType.Merchant];

        Debug.Log("���� monsterCount: " + monsterCountTmp);
        Debug.Log("���� eventCount: " + eventCountTmp);
        Debug.Log("���� eliteCount: " + eliteCountTmp);
        Debug.Log("���� restCount: " + restCountTmp);
        Debug.Log("���� merchantCount: " + merchantCountTmp);

        if (eventCountTmp == 0 && eliteCountTmp == 0 && restCountTmp == 0 && merchantCountTmp == 0)
        {
            Debug.Log("��� ��Ģ ����");
            allRulesSatisfied = true;
        }
        else
        {
            Debug.Log("��� ��Ģ �������� ����");
            allRulesSatisfied = false;
        }
    }

    // ���� ������ �°� ������ ��ġ
    public void PlacePointsIconForMap()
    {
        // �� ��ġ
        // ������ ���� ��ġ
        AssignFixedRoom();

        originPoints = (PointType[,])points.Clone(); // ���� �迭 ����

        // �� �������� ���� ���
        calculateRoomType();

        allRulesSatisfied = false;
        int cnt = 0;
        // ���� ���� ��Ģ�� ���� ��ġ
        while (!allRulesSatisfied && cnt < 100)
        {
            points = (PointType[,])originPoints.Clone();
            AssignRestRoom();
            cnt++;
            if (cnt >= 100)
            {
                Debug.Log("���� 100ȸ ���� �� �ߴ�, ��� ��Ģ�� ������Ű�� ����");
                break;
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (points[x, y] != PointType.Empty) 
                    Instantiate(pointIconPrefabs[(int)points[x, y]], new Vector3(x, y, 0), Quaternion.identity, iconHoler);
            }
        }

        // ������ ��ġ �� ������ ����ϴ� Point���� ��Ȱ��ȭ
        pointHolderObject.SetActive(false);
    }

    // ���� �������� ��ġ�� ������Ʈ�ϴ� �޼���
    public void UpdateLineRendererPositions(int x, int y, Vector3 updatedPosition)
    {
        Vector3 currentPosition = new Vector3(x, y, 0);

        foreach (Transform child in lineHolder)
        {
            // �ش� ��ο� ���� ���� ������ ã��
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();

            if (lineRenderer != null)
            {
                // �־��� ��ǥ�� ���� �������� ����Ʈ �� �ϳ����� Ȯ��
                for (int i = 0; i < lineRenderer.positionCount; i++) 
                {
                    if (lineRenderer.GetPosition(i) == currentPosition)
                    {
                        // ���� �������� ����Ʈ ��ġ ������Ʈ
                        lineRenderer.SetPosition(i, updatedPosition);
                    }
                }
            }
        }
    }

    public void UpdatePointPositions()
    {
        foreach (Transform child in iconHoler)
        {
            // �������� ��ġ�� GetUpdatedPosition ����Ͽ� �������� ����
            // �������� ���� ��ġ�� ������
            Vector3 currentPosition = child.position;

            // �������� ��ġ�� �����ϰ� ����
            Vector3 updatedPosition = GetUpdatedPosition(currentPosition);

            // �������� ��ġ�� ������Ʈ
            child.position = updatedPosition;

            // ��ġ�� �����ߴ� �������� ������ ���� ������ ã�Ƽ� ������Ʈ
            UpdateLineRendererPositions((int)currentPosition.x, (int)currentPosition.y, updatedPosition);
        }
    }

    // ���� ��ġ�� ������� ������Ʈ�� ��ġ�� ����ϴ� �޼���
    private Vector3 GetUpdatedPosition(Vector3 currentPosition)
    {
        // 0.5 �ݰ� ������ ���� ��ġ�� ���
        float randomRadius = 0.3f;
        Vector3 randomDirection = Random.insideUnitCircle * randomRadius; // X, Y ��鿡 ���� ���� ���� ����
        randomDirection.z = 0; // Z ���� �������� ����

        // ������Ʈ�� ��ġ ���
        Vector3 updatedPosition = currentPosition + randomDirection;

        return updatedPosition;
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
}
