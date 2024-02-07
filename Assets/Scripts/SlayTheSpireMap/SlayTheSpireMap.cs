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
    [SerializeField] int monsterCount, eventCount, eliteCount, restCount, merchantCount; // 이벤트, 엘리트, 휴식, 상인 방 개수
    public GameObject[] pointIconPrefabs; // 아이콘 프리팹
    [SerializeField] private Transform iconHoler;

    [SerializeField] int width = 7, height = 15; // 맵의 가로 길이, 맵의 세로 길이
    [SerializeField] int startPointCount; // 2에서 4 사이의 랜덤한 출발 지점 개수
    [SerializeField] HashSet<int> startPoints; // 출발 지점
    [SerializeField] Path[] paths = new Path[6]; // 경로 6개

    public GameObject lineRendererPrefab; // 라인 렌더러 프리팹
    [SerializeField] private Transform lineHolder; // 라인 렌더러 프리팹을 담는 부모 게임 오브젝트
    [SerializeField] private Color[] pathColors; // 인스펙터에서 설정할 색상 배열

    public GameObject pointPrefab; // 지점 프리팹
    public int totalPointCount; // 맵의 모든 좌표 개수
    [SerializeField] private Transform pointHolder; // 지점 프리팹들을 담는 오브젝트
    [SerializeField] private GameObject pointHolderObject; // 지점 프리팹을 담는 게임 오브젝트
    public PointType[,] points = new PointType[7, 15]; // 맵의 모든 좌표에 대한 정보를 담는 배열
    public PointType[,] originPoints = new PointType[7, 15];

    public bool allRulesSatisfied = false; // 모든 규칙이 만족되었는지 확인

    private HashSet<int> GenerateStartPoints(int numberOfPoints)
    {
        HashSet<int> startPoints = new HashSet<int>();
        int randomIndex;

        switch (numberOfPoints)
        {
            case 2: // 출발 지점이 2개인 경우 0 ~ 2, 3 ~ 6에서 하나씩 선택
                randomIndex = Random.Range(0, 3);
                startPoints.Add(randomIndex);
                randomIndex = Random.Range(3, 7);
                startPoints.Add(randomIndex);
                break;
            case 3: // 출발 지점이 3개인 경우 0 ~ 1, 2 ~ 4, 5 ~ 6에서 하나씩 선택
                randomIndex = Random.Range(0, 2);
                startPoints.Add(randomIndex);
                randomIndex = Random.Range(2, 5);
                startPoints.Add(randomIndex);
                randomIndex = Random.Range(2, 7);
                startPoints.Add(randomIndex);
                break;
            case 4: // 출발 지점이 4개인 경우
                while (startPoints.Count < numberOfPoints)
                {
                    randomIndex = Random.Range(0, 7); // 7개의 가능한 출발 지점 중 랜덤 선택
                    startPoints.Add(randomIndex);
                }
                break;
            default:
                break;
        }
        return startPoints;
    }

    // 출발 지점을 랜덤으로 선택
    private int choiceRandomStartPoint()
    {
        int randomIndex = Random.Range(0, startPointCount); // 0부터 startPointCount - 1 사이의 랜덤 숫자 선택
        int currentIndex = 0;

        foreach (int startPoint in startPoints)
        {
            if (currentIndex == randomIndex)
            {
                return startPoint; // 선택된 랜덤 인덱스에 해당하는 출발 지점 반환
            }
            currentIndex++;
        }

        return -1; // 예외 상황, 일반적으로 발생하지 않음
    }

    // path들을 순회하면서 Edge가 있는지 확인
    private bool checkCross(Vector2Int start, Vector2Int end)
    {
        //Debug.Log("checkCross: " + start + ", " + end);
        MapEdge newEdge = new MapEdge(start, end);

        // 모든 경로에 대해 newEdge를 포함하는지 확인
        foreach (Path path in paths)
        {
            if (path != null && path.Edges != null)
            {
                foreach (MapEdge pathEdge in path.Edges)
                {
                    // 시작점과 끝점이 모두 같은지 확인
                    if (pathEdge.StartPoint == newEdge.StartPoint && pathEdge.EndPoint == newEdge.EndPoint)
                    {
                        //Debug.Log("checkCross: true");
                        return true; // 동일한 Edge 발견
                    }
                }
            }
        }

        //Debug.Log("checkCross: false");
        return false; // 동일한 Edge 없음
    }

    // 다음 지점 선택
    private Vector2Int ChooseNextPosition(int currentX, int currentY)
    {
        //Debug.Log("ChooseNextPosition 현재 위치: " + currentX + ", " + currentY);
        List<Vector2Int> possiblePositions = new List<Vector2Int>();

        // 왼쪽 대각선 위가 맵 범위 내인지 확인
        if (currentX - 1 > -1 && currentY + 1 < height)
        {
            //Debug.Log("왼쪽 대각선 확인");
            if (!checkCross(new Vector2Int(currentX - 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX - 1, currentY + 1));
            }
        }

        // 바로 위가 맵 범위 내인지 확인
        if (currentY + 1 < height)
        {
            //Debug.Log("바로 위 확인");
            possiblePositions.Add(new Vector2Int(currentX, currentY + 1));
        }

        // 오른쪽 대각선 위가 맵 범위 내인지 확인
        if (currentX + 1 < width && currentY + 1 < height)
        {
            //Debug.Log("오른쪽 대각선 확인");  
            if (!checkCross(new Vector2Int(currentX + 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX + 1, currentY + 1));
            }
        }

        // 랜덤하게 하나의 위치 선택
        if (possiblePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            //Debug.Log("ChooseNextPosition: " + possiblePositions[randomIndex]);
            return possiblePositions[randomIndex];
        }

        // 예외 처리: 유효한 위치가 없는 경우
        return new Vector2Int(-1, -1);
    }

    // 경로 생성
    private void GeneratePath(int pathIndex)
    {
        //Debug.Log("GeneratePath: " + pathIndex);

        // paths 배열의 해당 인덱스에 Path 객체가 없으면 새로 생성
        if (paths[pathIndex] == null)
        {
            paths[pathIndex] = new Path();
        }

        int currentX = choiceRandomStartPoint(); // 출발 지점 선택
        //Debug.Log("currentX: " + currentX);

        Path path = paths[pathIndex]; // 경로 객체 참조

        int currentY = 0;
        while (currentY < height)
        {
            Vector2Int nextPosition = ChooseNextPosition(currentX, currentY);
            //Debug.Log("nextPosition: " + nextPosition);
            if (nextPosition.x == -1 && nextPosition.y == -1) // 예외 처리
                break;

            path.AddEdge(new MapEdge(new Vector2Int(currentX, currentY), nextPosition));
            points[currentX, currentY] = PointType.Visited; // points 배열에 경로가 지나간 좌표를 1로 표시

            currentX = nextPosition.x;
            currentY = nextPosition.y;
        }
        points[currentX, currentY] = PointType.Visited;
    }

    // 라인 렌더러로 경로 그리기
    private void DrawPathsWithLineRenderer()
    {
        int colorIndex = 0;

        foreach (Path path in paths)
        {
            GameObject lineObj = Instantiate(lineRendererPrefab, lineHolder);
            LineRenderer lineRenderer = lineObj.GetComponent<LineRenderer>();

            // 색상 설정
            Color pathColor = pathColors[colorIndex % pathColors.Length];
            lineRenderer.startColor = pathColor;
            lineRenderer.endColor = pathColor;

            // 다음 색상 선택을 위한 인덱스 증가
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

    // 맵의 모든 좌표에 pointPrefab 배치
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

    // 경로가 지나지 않는 지점들 파괴
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
                    totalPointCount++; // 맵의 모든 좌표 개수 증가
                }
            }
        }
    }

    // 고정된 지점 배치
    private void AssignFixedRoom()
    {
        // 1층은 모두 몬스터방
        for (int i = 0; i < 7; i++)
        {
            if (points[i, 0] == PointType.Empty)
                continue;
            if (points[i, 0] == PointType.Visited)
            {
                points[i, 0] = PointType.Monster;
                totalPointCount--; // 첫 층 몬스터방은 제외
            }
        }

        // 9층은 모두 보물방
        for (int i = 0; i < 7; i++)
        {
            if (points[i, 8] == PointType.Empty)
                continue;
            if (points[i, 8] == PointType.Visited)
            {
                points[i, 8] = PointType.Treasure;
                totalPointCount--; // 9층 보물방은 제외
            }
        }

        // 15층은 모두 휴식방
        for (int i = 0; i < 7; i++)
        {
            if (points[i, 14] == PointType.Empty)
                continue;
            if (points[i, 14] == PointType.Visited)
            {
                points[i, 14] = PointType.Rest;
                totalPointCount--; // 마지막층 휴식방은 제외
            }
        }
    }

    // 방 종류별로 개수 계산
    private void calculateRoomType()
    {
        // 이벤트 22%, 엘리트 16%, 휴식 12%, 상인 5%
        Debug.Log("고정층 제외 totalPointCount: " + totalPointCount);
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

    // 이전 방의 타입 찾기
    private List<PointType> FindPrevRoomType(int y, int x)
    {
        Vector2Int currentPoint = new Vector2Int(x, y);
        List<PointType> list = new List<PointType>();

        // 모든 경로에 대해 현재 지점을 포함하는지 확인
        foreach (Path path in paths)
        {
            if (path != null && path.Edges != null)
            {
                foreach (MapEdge pathEdge in path.Edges)
                {
                    // 끝점이 현재 지점인지 확인
                    if (pathEdge.EndPoint == currentPoint)
                    {
                        // 시작점의 타입을 리스트에 추가
                        // 몬스터방과 이벤트방은 제외
                        if (points[pathEdge.StartPoint.x, pathEdge.StartPoint.y] != PointType.Monster && points[pathEdge.StartPoint.x, pathEdge.StartPoint.y] != PointType.Event)
                            list.Add(points[pathEdge.StartPoint.x, pathEdge.StartPoint.y]);                                          
                    }
                }
            }
        }
        return list;
    }

    // 규칙에 따라 남은 지점 배치
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

        // 13층 먼저 휴식방 제외하고 배치
        for (int x = 0; x < width; x++) 
        {
            if (points[x, 13] == PointType.Empty)
                continue;

            // 이미 방이 배정된 경우 건너뛰기
            if (points[x, 13] != PointType.Visited)
            {
                continue;
            }

            // 사용 가능한 방 타입 리스트 생성
            List<PointType> availableRoomsIn13Floor = new List<PointType>();

            foreach (var roomType in pointTypeCounts.Keys)
            {
                if (pointTypeCounts[roomType] > 0 && roomType != PointType.Rest)
                {
                    for (int i = 0; i < pointTypeCounts[roomType]; i++)
                        availableRoomsIn13Floor.Add(roomType);
                }
            }

            // 선택된 방 타입 배치
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

        // 2층부터 6층까지 배치
        for (int y = 1; y < 6; y++) 
        {
            for (int x = 0; x < width; x++)
            {
                if (points[x, y] == PointType.Empty)
                    continue;
                // 이미 방이 배정된 경우 건너뛰기
                if (points[x, y] != PointType.Visited)
                    continue;

                // 이전 방의 타입 찾기
                List<PointType> prevRoomTypeList = FindPrevRoomType(y, x);

                // 사용 가능한 방 타입 리스트 생성
                List<PointType> availableRooms = new List<PointType>();
               
                foreach (var roomType in pointTypeCounts.Keys)
                {
                    // 엘리트방과 모닥불방은 6층 이하에 배정할 수 없음
                    if (roomType == PointType.Elite || roomType == PointType.Rest)
                        continue;

                    // 이전 방과 동일한 타입은 제외
                    if (prevRoomTypeList.Contains(roomType))
                        continue;
              
                    if (pointTypeCounts[roomType] > 0)
                    {
                        for (int i=0; i < pointTypeCounts[roomType]; i++)
                            availableRooms.Add(roomType);
                    }
                }

                // 선택된 방 타입 배치
                if (availableRooms.Count > 0)
                {
                    PointType selectedRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                    points[x, y] = selectedRoom;
                    pointTypeCounts[selectedRoom]--;
                }
                
            }
        }

        // 마지막으로 남은 개수 업데이트
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
                // 이미 방이 배정된 경우 건너뛰기
                if (points[x, y] != PointType.Visited)
                    continue;

                // 이전 방의 타입 찾기
                List<PointType> prevRoomTypeList = FindPrevRoomType(y, x);

                // 사용 가능한 방 타입 리스트 생성
                List<PointType> availableRooms = new List<PointType>();

                foreach (var roomType in pointTypeCounts.Keys)
                {                 
                    // 이전 방과 동일한 타입은 제외
                    if (prevRoomTypeList.Contains(roomType))
                        continue;

                    if (pointTypeCounts[roomType] > 0)
                    {
                        for (int i = 0; i < pointTypeCounts[roomType]; i++)
                            availableRooms.Add(roomType);
                    }
                }

                // 선택된 방 타입 배치
                if (availableRooms.Count > 0)
                {
                    PointType selectedRoom = availableRooms[Random.Range(0, availableRooms.Count)];
                    points[x, y] = selectedRoom;
                    pointTypeCounts[selectedRoom]--;
                }
                
            }
        }

        // 마지막으로 남은 개수 업데이트
        monsterCountTmp = pointTypeCounts[PointType.Monster];
        eventCountTmp = pointTypeCounts[PointType.Event];
        eliteCountTmp = pointTypeCounts[PointType.Elite];
        restCountTmp = pointTypeCounts[PointType.Rest];
        merchantCountTmp = pointTypeCounts[PointType.Merchant];

        Debug.Log("남은 monsterCount: " + monsterCountTmp);
        Debug.Log("남은 eventCount: " + eventCountTmp);
        Debug.Log("남은 eliteCount: " + eliteCountTmp);
        Debug.Log("남은 restCount: " + restCountTmp);
        Debug.Log("남은 merchantCount: " + merchantCountTmp);

        if (eventCountTmp == 0 && eliteCountTmp == 0 && restCountTmp == 0 && merchantCountTmp == 0)
        {
            Debug.Log("모든 규칙 만족");
            allRulesSatisfied = true;
        }
        else
        {
            Debug.Log("모든 규칙 만족하지 않음");
            allRulesSatisfied = false;
        }
    }

    // 방의 종류에 맞게 아이콘 배치
    public void PlacePointsIconForMap()
    {
        // 방 배치
        // 고정된 지점 배치
        AssignFixedRoom();

        originPoints = (PointType[,])points.Clone(); // 원본 배열 복사

        // 방 종류별로 개수 계산
        calculateRoomType();

        allRulesSatisfied = false;
        int cnt = 0;
        // 남은 지점 규칙에 따라 배치
        while (!allRulesSatisfied && cnt < 100)
        {
            points = (PointType[,])originPoints.Clone();
            AssignRestRoom();
            cnt++;
            if (cnt >= 100)
            {
                Debug.Log("루프 100회 실행 후 중단, 모든 규칙을 만족시키지 못함");
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

        // 아이콘 배치 후 기존에 사용하던 Point들은 비활성화
        pointHolderObject.SetActive(false);
    }

    // 라인 렌더러의 위치를 업데이트하는 메서드
    public void UpdateLineRendererPositions(int x, int y, Vector3 updatedPosition)
    {
        Vector3 currentPosition = new Vector3(x, y, 0);

        foreach (Transform child in lineHolder)
        {
            // 해당 경로에 대한 라인 렌더러 찾기
            LineRenderer lineRenderer = child.GetComponent<LineRenderer>();

            if (lineRenderer != null)
            {
                // 주어진 좌표가 라인 렌더러의 포인트 중 하나인지 확인
                for (int i = 0; i < lineRenderer.positionCount; i++) 
                {
                    if (lineRenderer.GetPosition(i) == currentPosition)
                    {
                        // 라인 렌더러의 포인트 위치 업데이트
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
            // 아이콘의 위치를 GetUpdatedPosition 사용하여 랜덤으로 변경
            // 아이콘의 현재 위치를 가져옴
            Vector3 currentPosition = child.position;

            // 아이콘의 위치를 랜덤하게 변경
            Vector3 updatedPosition = GetUpdatedPosition(currentPosition);

            // 아이콘의 위치를 업데이트
            child.position = updatedPosition;

            // 위치를 변경했던 아이콘을 지나는 라인 렌더러 찾아서 업데이트
            UpdateLineRendererPositions((int)currentPosition.x, (int)currentPosition.y, updatedPosition);
        }
    }

    // 현재 위치를 기반으로 업데이트된 위치를 계산하는 메서드
    private Vector3 GetUpdatedPosition(Vector3 currentPosition)
    {
        // 0.5 반경 내에서 랜덤 위치를 계산
        float randomRadius = 0.3f;
        Vector3 randomDirection = Random.insideUnitCircle * randomRadius; // X, Y 평면에 대해 랜덤 벡터 생성
        randomDirection.z = 0; // Z 축은 변경하지 않음

        // 업데이트된 위치 계산
        Vector3 updatedPosition = currentPosition + randomDirection;

        return updatedPosition;
    }

    void Start()
    {
        startPointCount = Random.Range(2, 5); // 랜덤으로 출발 지점 개수 선택
        startPoints = GenerateStartPoints(startPointCount);

        // paths 배열 초기화
        for (int i = 0; i < paths.Length; i++)
        {
            paths[i] = new Path();
        }

        // 맵의 모든 좌표에 pointPrefab 배치
        PlacePointsForMap();

        // 경로 6개 생성
        for (int i = 0; i < 6; i++)
        {
            GeneratePath(i);
        }

        // 라인 렌더러로 경로 그리기
        DrawPathsWithLineRenderer();    
    }
}
