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
    [SerializeField] int width = 7, height = 15; // 맵의 가로 길이, 맵의 세로 길이
    [SerializeField] int startPointCount; // 2에서 4 사이의 랜덤한 출발 지점 개수
    [SerializeField] HashSet<int> startPoints; // 출발 지점
    [SerializeField] Path[] paths = new Path[6]; // 경로 6개

    public GameObject lineRendererPrefab; // 라인 렌더러 프리팹
    [SerializeField] private Color[] pathColors; // 인스펙터에서 설정할 색상 배열

    public GameObject pointPrefab; // 지점 프리팹
    [SerializeField] private Transform pointHoler; // 지점 프리팹을 담는 부모 게임 오브젝트
    public int[,] points = new int[7, 15]; // 맵의 모든 좌표에 대한 정보를 담는 배열

    private HashSet<int> GenerateStartPoints(int numberOfPoints)
    {
        HashSet<int> startPoints = new HashSet<int>();
        while (startPoints.Count < numberOfPoints)
        {
            int randomIndex = Random.Range(0, 7); // 7개의 가능한 출발 지점 중 랜덤 선택
            startPoints.Add(randomIndex);
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
        Debug.Log("checkCross: " + start + ", " + end);
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
                        Debug.Log("checkCross: true");
                        return true; // 동일한 Edge 발견
                    }
                }
            }
        }

        Debug.Log("checkCross: false");
        return false; // 동일한 Edge 없음
    }

    // 다음 지점 선택
    private Vector2Int ChooseNextPosition(int currentX, int currentY)
    {
        Debug.Log("ChooseNextPosition 현재 위치: " + currentX + ", " + currentY);
        List<Vector2Int> possiblePositions = new List<Vector2Int>();

        // 왼쪽 대각선 위가 맵 범위 내인지 확인
        if (currentX - 1 > -1 && currentY + 1 < height) 
        {
            Debug.Log("왼쪽 대각선 확인");
            if (!checkCross(new Vector2Int(currentX - 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX - 1, currentY + 1));
            }
        }

        // 바로 위가 맵 범위 내인지 확인
        if (currentY + 1 < height)
        {
            Debug.Log("바로 위 확인");
            possiblePositions.Add(new Vector2Int(currentX, currentY + 1));
        }

        // 오른쪽 대각선 위가 맵 범위 내인지 확인
        if (currentX + 1 < width && currentY + 1 < height) 
        {
            Debug.Log("오른쪽 대각선 확인");  
            if (!checkCross(new Vector2Int(currentX + 1, currentY), new Vector2Int(currentX, currentY + 1)))
            {
                possiblePositions.Add(new Vector2Int(currentX + 1, currentY + 1));
            }
        }

        // 랜덤하게 하나의 위치 선택
        if (possiblePositions.Count > 0)
        {
            int randomIndex = Random.Range(0, possiblePositions.Count);
            Debug.Log("ChooseNextPosition: " + possiblePositions[randomIndex]);
            return possiblePositions[randomIndex];
        }

        // 예외 처리: 유효한 위치가 없는 경우
        return new Vector2Int(-1, -1);
    }

    // 경로 생성
    private void GeneratePath(int pathIndex)
    {
        Debug.Log("GeneratePath: " + pathIndex);

        // paths 배열의 해당 인덱스에 Path 객체가 없으면 새로 생성
        if (paths[pathIndex] == null)
        {
            paths[pathIndex] = new Path();
        }

        int currentX = choiceRandomStartPoint(); // 출발 지점 선택
        Debug.Log("currentX: " + currentX);

        Path path = paths[pathIndex]; // 경로 객체 참조

        int currentY = 0;
        while (currentY < height - 1)
        {
            Vector2Int nextPosition = ChooseNextPosition(currentX, currentY);
            Debug.Log("nextPosition: " + nextPosition);
            if (nextPosition.x == -1 && nextPosition.y == -1) // 예외 처리
                break;

            path.AddEdge(new MapEdge(new Vector2Int(currentX, currentY), nextPosition));
            points[currentX, currentY] = 1; // points 배열에 경로가 지나간 좌표를 1로 표시

            currentX = nextPosition.x;
            currentY = nextPosition.y;
        }
    }

    // 라인 렌더러로 경로 그리기
    private void DrawPathsWithLineRenderer()
    {
        int colorIndex = 0;

        foreach (Path path in paths)
        {
            GameObject lineObj = Instantiate(lineRendererPrefab, this.transform);
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

    // 맵의 모든 좌표에 pointPrefab 배치
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

    // 경로가 지나지 않는 지점들 파괴
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
