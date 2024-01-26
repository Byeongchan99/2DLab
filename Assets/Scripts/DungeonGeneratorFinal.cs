using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DungeonGeneratorFinal : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize;   // ���� ��ü ũ��

    [SerializeField] private int maxNode; // ������ ������ �� �����Ǵ� �ִ� ���(���� ����)�� ��
    [SerializeField] private float minDivideSize; // ������ ���� �� ����ϴ� ���� ������ �ּڰ��� �ִ�
    [SerializeField] private float maxDivideSize; // 0 ~ 1 ������ �� ���
    [SerializeField] private int minRoomSize; // �� ���� �ּ� ũ��

    [SerializeField] private GameObject line; // ������ ���Ҽ� ǥ��
    [SerializeField] private Transform lineHolder; // line ��ü���� ��� �θ� ���� ������Ʈ
    [SerializeField] private GameObject rectangle; // ������ ��輱 ǥ��

    [SerializeField] private string seed; // ���� �����ϱ� ���� �õ�
    [SerializeField] private bool useRandomSeed; // �õ� ��� ����

    [Range(0, 8)]
    [SerializeField] private int referenceWallCount; // ���� �� Ÿ�� ����
    [Range(0, 100)]
    [SerializeField] private int randomFillPercent; // ���� ��ο� �� ����

    private int[,] map, oldMap; // �� ���� ���� �迭
    // Ÿ�� Ÿ��
    private const int ROAD = 0; // ���
    private const int WALL = 1; // ��
    private const int PERMARNANT_ROAD = 2; // �������� ���
    private const int PERMARNANT_WALL = 3; // �������� ��

    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tile roadTile; // ��� Ÿ��
    [SerializeField] private Tile permarnantRoadTile; // �������� ��� Ÿ��
    [SerializeField] private Tile wallTile; // �� Ÿ��
    [SerializeField] private Tile permarnantWallTile; // �������� �� Ÿ��

    public StartTriangulation startTriangulation;

    public void GenerateDungeon()
    {
        ResetDungeon(); // ���� �ʱ�ȭ ȣ��
        GenerateMap(); // �� ���� 

        // BSP �˰��� ����
        OnDrawRectangle(0, 0, mapSize.x, mapSize.y); // �� ũ�⿡ �°� ���� �׸�
        TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); // ��Ʈ ��� ����
        DivideTree(rootNode, 0); // Ʈ�� ����
        GenerateRoom(rootNode, 0); // �� ����
        //ConnectRoad(rootNode, 0); // �� ����

        // BSP�� ������ �� Ȯ��
        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                OnDrawTile(x, y, map[x, y]);
            }
        }

        startTriangulation.StartDelaunayTriangulation(); // ��γ� �ﰢ���� ����
    }

    // ���� �ʱ�ȭ �޼���
    public void ResetDungeon()
    {
        // Ÿ�ϸ��� ��� Ÿ�� ����
        tilemap.ClearAllTiles();

        // ���� ������ ��ü�� �ı�
        foreach (Transform child in lineHolder)
        {
            Destroy(child.gameObject);
        }

        startTriangulation.ResetDungeon(); // ��γ� �ﰢ���� �ʱ�ȭ
    }

    // �� ����
    private void GenerateMap()
    {
        map = new int[mapSize.x, mapSize.y];
        oldMap = new int[mapSize.x, mapSize.y];

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (x == 0 || x == mapSize.x - 1 || y == 0 || y == mapSize.y - 1)
                    map[x, y] = PERMARNANT_WALL; // �����ڸ��� �������� ������ ����
                else
                    map[x, y] = WALL; // ������ �ʱ�ȭ
            }
        }
    }

    // BSP �˰���
    // Ʈ�� ����
    private void DivideTree(TreeNode treeNode, int n)
    {
        if (n < maxNode) //0���� �����ؼ� ����� �ִ񰪿� �̸� �� ���� �ݺ�
        {
            RectInt size = treeNode.treeSize; // ���� ����� ���� �� ����
            int length = size.width >= size.height ? size.width : size.height; // �簢���� ���ο� ���� �� ���̰� �� ���� ���� ��带 ������ ���ؼ����� ���
            int split = Mathf.RoundToInt(Random.Range(length * minDivideSize, length * maxDivideSize)); // ���ؼ����� �ּ� ������ �ִ� ���� ������ ���� �������� ����

            if (size.width >= size.height) // ����
            {
                treeNode.leftTree = new TreeNode(size.x, size.y, split, size.height); // ���ؼ��� ������ ���� ���� split�� ���� ���̷�, ���� Ʈ���� height���� ���� ���̷� ���
                treeNode.rightTree = new TreeNode(size.x + split, size.y, size.width - split, size.height); // ���ؼ� ���� ������ leftTree, �������� rightTree
                OnDrawLine(new Vector2(size.x + split, size.y), new Vector2(size.x + split, size.y + size.height)); // ���Ҽ� ������
            }
            else // ����
            {
                treeNode.leftTree = new TreeNode(size.x, size.y, size.width, split); // ���ؼ� ���� �Ʒ����� leftTree, ������ rightTree
                treeNode.rightTree = new TreeNode(size.x, size.y + split, size.width, size.height - split);
                OnDrawLine(new Vector2(size.x, size.y + split), new Vector2(size.x + size.width, size.y + split));
            }

            treeNode.leftTree.parentTree = treeNode; // ������ Ʈ���� �θ� Ʈ���� �Ű������� ���� Ʈ���� �Ҵ�
            treeNode.rightTree.parentTree = treeNode;
            DivideTree(treeNode.leftTree, n + 1); // ��� �Լ��� ��尪 ����
            DivideTree(treeNode.rightTree, n + 1);
        }
    }

    // �� ����
    private RectInt GenerateRoom(TreeNode treeNode, int n)
    {
        if (n == maxNode) // ������ ����϶� �� ����
        {
            RectInt size = treeNode.treeSize;

            // ���� ���ο� ���� ũ�⸦ 1���� ���� ũ������� ����
            int width = Mathf.Max(1, Random.Range(2, size.width / 2 + 1));
            int height = Mathf.Max(1, Random.Range(2, size.height / 2 + 1));

            // ���� ��ġ ����
            int x = treeNode.treeSize.x + Random.Range(1, size.width - width);
            int y = treeNode.treeSize.y + Random.Range(1, size.height - height);

            OnDrawRoom(x, y, width, height); // �� ����
            startTriangulation.AddPoint(new Vector2(x + (width / 2) - (mapSize.x / 2), y + (height / 2) - (mapSize.y / 2))); // �� �߽��� ��γ� �ﰢ������ point�� ���
            return new RectInt(x, y, width, height); // ���� ���� ������ ������ ���� ũ��(roomSize) ������ ���
        }

        treeNode.leftTree.roomSize = GenerateRoom(treeNode.leftTree, n + 1); // ��ͷ� �� ����
        treeNode.rightTree.roomSize = GenerateRoom(treeNode.rightTree, n + 1);
        return treeNode.leftTree.roomSize; // ���� �ڽ� Ʈ���� �� ũ�� ��ȯ
    }

    // �� ����
    private void ConnectRoad(TreeNode treeNode, int n)
    {
        if (n == maxNode) // ��尡 �������� ���� ���� �������� ����
            return;
        // ���� �ڽ� ���� ������ �ڽ� ��� �� ����
        int x1 = GetCenterX(treeNode.leftTree.roomSize); // �ڽ� Ʈ���� �� �߾� ��ġ�� ������
        int x2 = GetCenterX(treeNode.rightTree.roomSize);
        int y1 = GetCenterY(treeNode.leftTree.roomSize);
        int y2 = GetCenterY(treeNode.rightTree.roomSize);
        for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) // x1�� x2 �� ���� ���� ������ ���� ū ������ �������� ��η� ����
            map[x, y1] = PERMARNANT_ROAD;
        for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++) // y1�� y2 �� ���� ���� ������ ���� ū ������ �������� ��η� ����
            map[x2, y] = PERMARNANT_ROAD;
        ConnectRoad(treeNode.leftTree, n + 1);
        ConnectRoad(treeNode.rightTree, n + 1);
    }

    // ���� �������� ����� ���� ���Ҽ� ����
    private void OnDrawLine(Vector2 from, Vector2 to)
    {
        LineRenderer lineRenderer = Instantiate(line, lineHolder).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, from - mapSize / 2); // ��ġ�� ȭ�� �߾ӿ� ����
        lineRenderer.SetPosition(1, to - mapSize / 2);
    }

    // ���� ũ�⿡ ���� �� ����
    private void OnDrawRoom(int x, int y, int width, int height)
    {
        for (int i = x; i < x + width; i++)
            for (int j = y; j < y + height; j++)
                map[i, j] = PERMARNANT_ROAD; // �������� ��� ����
    }

    // ���� �������� ����� ���� ��輱 ����
    private void OnDrawRectangle(int x, int y, int width, int height)
    {
        LineRenderer lineRenderer = Instantiate(rectangle, lineHolder).GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector2(x, y) - mapSize / 2); // ��ġ�� ȭ�� �߾ӿ� ����
        lineRenderer.SetPosition(1, new Vector2(x + width, y) - mapSize / 2);
        lineRenderer.SetPosition(2, new Vector2(x + width, y + height) - mapSize / 2);
        lineRenderer.SetPosition(3, new Vector2(x, y + height) - mapSize / 2);
    }

    // �� �߾��� x ��ǥ
    private int GetCenterX(RectInt size)
    {
        return size.x + size.width / 2;
    }

    // �� �߾��� y ��ǥ
    private int GetCenterY(RectInt size)
    {
        return size.y + size.height / 2;
    }

    // ���귯 ���丶Ÿ ����
    public void CellularAutomata()
    {
        MapRandomFill(); // �� ä���
        SmoothMap(); // ������ ����
    }

    // ���� ������ ���� �� Ȥ�� �� �������� �����ϰ� ä��� �޼���
    private void MapRandomFill()
    {
        if (useRandomSeed) // �õ� ��� ����
            seed = Time.time.ToString(); // �õ� ����

        System.Random pseudoRandom = new System.Random(seed.GetHashCode()); // �ǻ� ���� ����

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (map[x, y] == WALL) // ��� ��θ� �����ϰ� ���� ������ �ٽ� �������� ä��               
                    map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? WALL : ROAD; // ������ ���� �� �Ǵ� ��η� ����
            }
        }
    }

    // ���� Ÿ�ϵ��� ��ȸ�ϸ� ������ �ٵ�� �޼���
    public void SmoothMap()
    {
        CreateOldMap(); // ���� �� ���� ����

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                if (map[x, y] == PERMARNANT_ROAD || map[x, y] == PERMARNANT_WALL) // �������� ��γ� ���̸� ��ȭ X
                    continue;

                int neighbourWallTiles = GetSurroundingWallCount(x, y); // �ֺ� �� Ÿ�� ���� ���

                if (neighbourWallTiles > referenceWallCount)
                    map[x, y] = WALL; // �ֺ� ĭ �� ���� ���� �� Ÿ�� ������ �ʰ��� ��� ���� Ÿ���� ������ ����
                else if (neighbourWallTiles < referenceWallCount)
                    map[x, y] = ROAD; // �ֺ� ĭ �� ���� ���� �� Ÿ�� ���� �̸��� ��� ���� Ÿ���� ��η� ����

                OnDrawTile(x, y, map[x, y]); // Ÿ�� ���� ����
            }
        }
    }

    // ���� �� ���� ����
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

    // �ֺ��� �� Ÿ�� ������ ����ϴ� �޼���
    private int GetSurroundingWallCount(int x, int y)
    {
        int wallCount = 0;

        // ���� ��ǥ�� �������� �ֺ� 8ĭ �˻�
        for (int neighbourX = x - 1; neighbourX <= x + 1; neighbourX++)
        {
            for (int neighbourY = y - 1; neighbourY <= y + 1; neighbourY++)
            {
                // �� ���� �˻�
                if (neighbourX >= 0 && neighbourX < mapSize.x && neighbourY >= 0 && neighbourY < mapSize.y)
                {
                    // ���� �ٲ��� ���� oldMap ���� ���
                    if (neighbourX != x || neighbourY != y)
                    {
                        // �ֺ� Ÿ���� ���̰ų� �������� ���� ��
                        if (oldMap[neighbourX, neighbourY] == 1 || oldMap[neighbourX, neighbourY] == 3)
                            wallCount += 1;
                    }
                }
                else  // �ֺ� Ÿ���� �� ������ ��� ���(���� Ÿ���� �����ڸ� Ÿ��)
                    wallCount++;
            }
        }
        return wallCount;
    }

    // Ÿ�� Ÿ�Կ� ���� Ÿ�� ����
    private void OnDrawTile(int x, int y, int tileType)
    {
        Vector3Int pos = new Vector3Int(x - mapSize.x / 2, y - mapSize.y / 2, 0); // �߾� ����

        if (tileType == ROAD) // ���
            tilemap.SetTile(pos, roadTile);
        else if (tileType == PERMARNANT_ROAD) // �������� ���
            tilemap.SetTile(pos, permarnantRoadTile);
        else if (tileType == WALL) // ��
            tilemap.SetTile(pos, wallTile);
        else // �������� ��
            tilemap.SetTile(pos, permarnantWallTile);
    }
}