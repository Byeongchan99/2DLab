using UnityEngine;
using UnityEngine.Tilemaps;

    public class TreeNode
    {
        public TreeNode leftTree;
        public TreeNode rightTree;
        public TreeNode parentTree;
        public RectInt treeSize; // TreeNode 객체가 나타내는 던전 분할 영역의 위치와 크기
        public RectInt roomSize; // 각 던전 분할 영역 내에서 실제로 생성된 방의 크기와 위치

        public TreeNode(int x, int y, int width, int height)   // 왼쪽 하단 모서리 좌표, 너비, 높이
        {
            treeSize.x = x;
            treeSize.y = y;
            treeSize.width = width;
            treeSize.height = height;
        }
    }

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize;   // 맵의 전체 크기

    [SerializeField] private int maxNode; // 던전을 분할할 때 생성되는 최대 노드(분할 영역)의 수
    [SerializeField] private float minDivideSize; // 던전을 나눌 때 사용하는 분할 비율의 최솟값과 최댓값
    [SerializeField] private float maxDivideSize; // 0 ~ 1 사이의 값 사용
    [SerializeField] private int minRoomSize; // 각 방의 최소 크기

    [SerializeField] private GameObject line; // 던전의 분할선 표시
    [SerializeField] private Transform lineHolder; // line 객체들을 담는 부모 게임 오브젝트
    [SerializeField] private GameObject rectangle; // 던전의 경계선 표시

    [SerializeField] private Tile roomTile; // 방에 사용되는 타일
    [SerializeField] private Tile roadTile; // 길에 사용되는 타일  
    [SerializeField] private Tilemap tilemap;

    public void GenerateDungeon()
    {
        ResetDungeon();  // 던전 초기화 호출
        OnDrawRectangle(0, 0, mapSize.x, mapSize.y); // 맵 크기에 맞게 벽을 그림
        TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); // 루트 노드 생성
        DivideTree(rootNode, 0); // 트리 분할
        GenerateRoom(rootNode, 0); // 방 생성
        ConnectRoad(rootNode, 0); // 길 연결
    }

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
            int width = Mathf.Max(Random.Range(size.width / 2, size.width - 1)); // 노드의 treeSize값의 범위 내에서 무작위 크기로 방 크기 선택
            int height = Mathf.Max(Random.Range(size.height / 2, size.height - 1));
            int x = treeNode.treeSize.x + Random.Range(1, size.width - width);
            int y = treeNode.treeSize.y + Random.Range(1, size.height - height);
            OnDrawRoom(x, y, width, height); // 방 렌더링
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
        for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) // x1과 x2 중 값이 작은 곳부터 값이 큰 곳까지 타일 생성
            tilemap.SetTile(new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0), roadTile); // mapSize의 절반을 빼서 화면 중앙에 맞춤
        for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            tilemap.SetTile(new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2, 0), roadTile);
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

    // 방의 크기에 맞춰 타일 생성
    private void OnDrawRoom(int x, int y, int width, int height)
    {
        for (int i = x; i < x + width; i++)
            for (int j = y; j < y + height; j++)
                tilemap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile); // 위치를 화면 중앙에 맞춤
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
    }
}