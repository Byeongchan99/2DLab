using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneratorFinal : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize;   // 맵의 전체 크기

    [SerializeField] private int maxNode; // 던전을 분할할 때 생성되는 최대 노드(분할 영역)의 수
    [SerializeField] private float minDivideSize; // 던전을 나눌 때 사용하는 분할 비율의 최솟값과 최댓값
    [SerializeField] private float maxDivideSize; // 0 ~ 1 사이의 값 사용
    [SerializeField] private int minRoomSize; // 각 방의 최소 크기

    [SerializeField] private GameObject line; // 던전의 분할선 표시
    [SerializeField] private Transform lineHolder; // line 객체들을 담는 부모 게임 오브젝트
    [SerializeField] private GameObject rectangle; // 던전의 경계선 표시

    [SerializeField] private string seed; // 난수 생성하기 위한 시드
    [SerializeField] private bool useRandomSeed; // 시드 사용 여부

    [Range(0, 8)]
    [SerializeField] private int referenceWallCount; // 기준 벽 타일 개수
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent; // 맵의 통로와 벽 비율

    private int[,] map, oldMap; // 맵 정보 저장 배열
    // 타일 타입
    private const int ROAD = 0; // 통로
    private const int WALL = 1; // 벽
    private const int PERMARNANT_ROAD = 2; // 영구적인 통로
    private const int PERMARNANT_WALL = 3; // 영구적인 벽

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile roadTile; // 통로 타일
    [SerializeField] private Tile permarnantRoadTile; // 영구적인 통로 타일
    [SerializeField] private Tile wallTile; // 벽 타일
    [SerializeField] private Tile permarnantWallTile; // 영구적인 벽 타일

    public StartTriangulation startTriangulation;

    public void GenerateDungeon()
    {
        ResetDungeon(); // 던전 초기화 호출
        GenerateMap(); // 맵 생성 

        // BSP 알고리즘 실행
        OnDrawRectangle(0, 0, mapSize.x, mapSize.y); // 맵 크기에 맞게 벽을 그림
        TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); // 루트 노드 생성
        DivideTree(rootNode, 0); // 트리 분할
        GenerateRoom(rootNode, 0); // 방 생성
        //ConnectRoad(rootNode, 0); // 길 연결

        // BSP로 생성된 맵 확인
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                OnDrawTile(x, y, map[x, y]);
            }
        }

        startTriangulation.StartDelaunayTriangulation(); // 들로네 삼각분할 실행
    }

    // 던전 초기화 메서드
    public void ResetDungeon()
    {
        // 타일맵의 모든 타일 제거
        tilemap.ClearAllTiles();

        // 라인 렌더러 객체들 파괴
        foreach (Transform child in lineHolder)
        {
            Destroy(child.gameObject);
        }

        startTriangulation.ResetDungeon(); // 들로네 삼각분할 초기화
    }

    // 맵 생성
    private void GenerateMap()
    {
        map = new int[mapSize.x, mapSize.y];
        oldMap = new int[mapSize.x, mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (x == 0 || x == mapSize.x - 1 || y == 0 || y == mapSize.y - 1)
                    map[x, y] = PERMARNANT_WALL; // 가장자리는 영구적인 벽으로 설정
                else
                    map[x, y] = WALL; // 벽으로 초기화
            }
        }
    }

    // BSP 알고리즘
    // 트리 분할
    private void DivideTree(TreeNode treeNode, int n)
    {
        if (n < maxNode) //0부터 시작해서 노드의 최댓값에 이를 때 까지 반복
        {
            RectInt size = treeNode.treeSize; // 이전 노드의 범위 값 저장
            int length = size.width >= size.height ? size.width : size.height; // 사각형의 가로와 세로 중 길이가 긴 축을 현재 노드를 나누는 기준선으로 사용
            int split = Mathf.RoundToInt(Random.Range(length * minDivideSize, length * maxDivideSize)); // 기준선에서 최소 범위와 최대 범위 사이의 값을 무작위로 선택

            if (size.width >= size.height) // 가로
            {
                treeNode.leftTree = new TreeNode(size.x, size.y, split, size.height); // 기준선을 반으로 나눈 값인 split을 가로 길이로, 이전 트리의 height값을 세로 길이로 사용
                treeNode.rightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height); // 기준선 기준 왼쪽이 leftTree, 오른쪽이 rightTree
                OnDrawLine(new Vector2(size.x + split, size.y), new Vector2(size.x + split, size.y + size.height)); // 분할선 렌더링
            }
            else // 세로
            {
                treeNode.leftTree = new TreeNode(size.x, size.y, size.width, split); // 기준선 기준 아래쪽이 leftTree, 위쪽이 rightTree
                treeNode.rightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
                OnDrawLine(new Vector2(size.x, size.y + split), new Vector2(size.x + size.width, size.y + split));
            }

            treeNode.leftTree.parentTree = treeNode; // 분할한 트리의 부모 트리를 매개변수로 받은 트리로 할당
            treeNode.rightTree.parentTree = treeNode;
            DivideTree(treeNode.leftTree, n + 1); // 재귀 함수로 노드값 증가
            DivideTree(treeNode.rightTree, n + 1);
        }
    }

    // 방 생성
    private RectInt GenerateRoom(TreeNode treeNode, int n)
    {
        if (n == maxNode) // 최하위 노드일때 방 생성
        {
            RectInt size = treeNode.treeSize;

            // 방의 가로와 세로 크기를 1부터 절반 크기까지로 설정
            int width = Mathf.Max(1, Random.Range(2, size.width / 2 + 1));
            int height = Mathf.Max(1, Random.Range(2, size.height / 2 + 1));

            // 방의 위치 설정
            int x = treeNode.treeSize.x + Random.Range(1, size.width - width);
            int y = treeNode.treeSize.y + Random.Range(1, size.height - height);

            OnDrawRoom(x, y, width, height); // 방 설정
            startTriangulation.AddPoint(new Vector2(x + (width / 2) - (mapSize.x / 2), y + (height / 2) - (mapSize.y / 2))); // 방 중심을 들로네 삼각분할의 point로 사용
            return new RectInt(x, y, width, height); // 리턴 값은 실제로 생성된 방의 크기(roomSize) 값으로 사용
        }

        treeNode.leftTree.roomSize = GenerateRoom(treeNode.leftTree, n + 1); // 재귀로 방 생성
        treeNode.rightTree.roomSize = GenerateRoom(treeNode.rightTree, n + 1);
        return treeNode.leftTree.roomSize; // 왼쪽 자식 트리의 방 크기 반환
    }

    // 길 연결
    private void ConnectRoad(TreeNode treeNode, int n)
    {
        if (n == maxNode) // 노드가 최하위일 때는 길을 연결하지 않음
            return;
        // 왼쪽 자식 노드와 오른쪽 자식 노드 길 연결
        int x1 = GetCenterX(treeNode.leftTree.roomSize); // 자식 트리의 방 중앙 위치를 가져옴
        int x2 = GetCenterX(treeNode.rightTree.roomSize);
        int y1 = GetCenterY(treeNode.leftTree.roomSize);
        int y2 = GetCenterY(treeNode.rightTree.roomSize);
        for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) // x1과 x2 중 값이 작은 곳부터 값이 큰 곳까지 영구적인 통로로 설정
            map[x, y1] = PERMARNANT_ROAD;
        for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++) // y1과 y2 중 값이 작은 곳부터 값이 큰 곳까지 영구적인 통로로 설정
            map[x2, y] = PERMARNANT_ROAD;
        ConnectRoad(treeNode.leftTree, n + 1);
        ConnectRoad(treeNode.rightTree, n + 1);
    }

    // 라인 렌더러를 사용해 던전 분할선 생성
    private void OnDrawLine(Vector2 from, Vector2 to)
    {
        LineRenderer lineRenderer = Instantiate(line, lineHolder).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2); // 위치를 화면 중앙에 맞춤
        lineRenderer.SetPosition(1, to - mapSize / 2);
    }

    // 방의 크기에 맞춰 방 설정
    private void OnDrawRoom(int x, int y, int width, int height)
    {
        for (int i = x; i < x + width; i++)
            for (int j = y; j < y + height; j++)
                map[i, j] = PERMARNANT_ROAD; // 영구적인 통로 생성
    }

    // 라인 렌더러를 사용해 던전 경계선 생성
    private void OnDrawRectangle(int x, int y, int width, int height)
    {
        LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); // 위치를 화면 중앙에 맞춤
        lineRenderer.SetPosition(1, new Vector2(x + width, y) - mapSize / 2);
        lineRenderer.SetPosition(2, new Vector2(x + width, y + height) - mapSize / 2);
        lineRenderer.SetPosition(3, new Vector2(x, y + height) - mapSize / 2);
    }

    // 방 중앙의 x 좌표
    private int GetCenterX(RectInt size)
    {
        return size.x + size.width / 2;
    }

    // 방 중앙의 y 좌표
    private int GetCenterY(RectInt size)
    {
        return size.y + size.height / 2;
    }

    // 셀룰러 오토마타 실행
    public void CellularAutomata()
    {
        MapRandomFill(); // 맵 채우기
        SmoothMap(); // 스무딩 실행
    }

    // 맵을 비율에 따라 벽 혹은 빈 공간으로 랜덤하게 채우는 메서드
    private void MapRandomFill()
    {
        if (useRandomSeed) // 시드 사용 여부
            seed = Time.time.ToString(); // 시드 생성

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); // 의사 난수 생성

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (map[x, y] == WALL) // 방과 통로를 생성하고 남은 공간을 다시 랜덤으로 채움               
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD; // 비율에 따라 벽 또는 통로로 설정
            }
        }
    }

    // 맵의 타일들을 순회하며 경계면을 다듬는 메서드
    public void SmoothMap()
    {
        CreateOldMap(); // 원래 맵 정보 저장

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (map[x, y] == PERMARNANT_ROAD || map[x, y] == PERMARNANT_WALL) // 영구적인 통로나 벽이면 변화 X
                    continue;

                int neighbourWallTiles = GetSurroundingWallCount(x, y); // 주변 벽 타일 개수 계산

                if (neighbourWallTiles > referenceWallCount)
                    map[x, y] = WALL; // 주변 칸 중 벽이 기준 벽 타일 개수를 초과할 경우 현재 타일을 벽으로 설정
                else if (neighbourWallTiles < referenceWallCount)
                    map[x, y] = ROAD; // 주변 칸 중 벽이 기준 벽 타일 개수 미만일 경우 현재 타일을 통로로 설정

                OnDrawTile(x, y, map[x, y]); // 타일 색상 변경
            }
        }
    }

    // 원래 맵 정보 저장
    private void CreateOldMap()
    {
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                oldMap[x, y] = map[x, y];
            }
        }
    }

    // 주변의 벽 타일 개수를 계산하는 메서드
    private int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;

        // 현재 좌표를 기준으로 주변 8칸 검사
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                // 맵 범위 검사
                if (neighbourX >= 0 && neighbourX < mapSize.x && neighbourY >= 0 && neighbourY < mapSize.y)
                {
                    // 값이 바뀌지 않은 oldMap 정보 사용
                    if (neighbourX != x || neighbourY != y)
                    {
                        // 주변 타일이 벽이거나 영구적인 벽일 때
                        if (oldMap[neighbourX, neighbourY] == 1 || oldMap[neighbourX, neighbourY] == 3)
                            wallCount += 1;
                    }
                }
                else  // 주변 타일이 맵 범위를 벗어날 경우(현재 타일이 가장자리 타일)
                    wallCount++;
            }
        }
        return wallCount;
    }

    // 타일 타입에 따라 타일 생성
    private void OnDrawTile(int x, int y, int tileType)
    {
        Vector3Int pos = new Vector3Int(x - mapSize.x / 2, y - mapSize.y / 2, 0); // 중앙 정렬

        if (tileType == ROAD) // 통로
            tilemap.SetTile(pos, roadTile);
        else if (tileType == PERMARNANT_ROAD) // 영구적인 통로
            tilemap.SetTile(pos, permarnantRoadTile);
        else if (tileType == WALL) // 벽
            tilemap.SetTile(pos, wallTile);
        else // 영구적인 벽
            tilemap.SetTile(pos, permarnantWallTile);
    }
}