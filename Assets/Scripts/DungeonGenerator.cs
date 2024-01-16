using UnityEngine;
using UnityEngine.Tilemaps;

    public class TreeNode
    {
        public TreeNode leftTree;
        public TreeNode rightTree;
        public TreeNode parentTree;
        public RectInt treeSize; // TreeNode ��ü�� ��Ÿ���� ���� ���� ������ ��ġ�� ũ��
        public RectInt roomSize; // �� ���� ���� ���� ������ ������ ������ ���� ũ��� ��ġ

        public TreeNode(int x, int y, int width, int height)   // ���� �ϴ� �𼭸� ��ǥ, �ʺ�, ����
        {
            treeSize.x = x;
            treeSize.y = y;
            treeSize.width = width;
            treeSize.height = height;
        }
    }

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] private Vector2Int mapSize;   // ���� ��ü ũ��

    [SerializeField] private int maxNode; // ������ ������ �� �����Ǵ� �ִ� ���(���� ����)�� ��
    [SerializeField] private float minDivideSize; // ������ ���� �� ����ϴ� ���� ������ �ּڰ��� �ִ�
    [SerializeField] private float maxDivideSize; // 0 ~ 1 ������ �� ���
    [SerializeField] private int minRoomSize; // �� ���� �ּ� ũ��

    [SerializeField] private GameObject line; // ������ ���Ҽ� ǥ��
    [SerializeField] private Transform lineHolder; // line ��ü���� ��� �θ� ���� ������Ʈ
    [SerializeField] private GameObject rectangle; // ������ ��輱 ǥ��

    [SerializeField] private Tile roomTile; // �濡 ���Ǵ� Ÿ��
    [SerializeField] private Tile roadTile; // �濡 ���Ǵ� Ÿ��  
    [SerializeField] private Tilemap tilemap;

    public void GenerateDungeon()
    {
        ResetDungeon();  // ���� �ʱ�ȭ ȣ��
        OnDrawRectangle(0, 0, mapSize.x, mapSize.y); // �� ũ�⿡ �°� ���� �׸�
        TreeNode rootNode = new TreeNode(0, 0, mapSize.x, mapSize.y); // ��Ʈ ��� ����
        DivideTree(rootNode, 0); // Ʈ�� ����
        GenerateRoom(rootNode, 0); // �� ����
        ConnectRoad(rootNode, 0); // �� ����
    }

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
            int width = Mathf.Max(Random.Range(size.width / 2, size.width - 1)); // ����� treeSize���� ���� ������ ������ ũ��� �� ũ�� ����
            int height = Mathf.Max(Random.Range(size.height / 2, size.height - 1));
            int x = treeNode.treeSize.x + Random.Range(1, size.width - width);
            int y = treeNode.treeSize.y + Random.Range(1, size.height - height);
            OnDrawRoom(x, y, width, height); // �� ������
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
        for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++) // x1�� x2 �� ���� ���� ������ ���� ū ������ Ÿ�� ����
            tilemap.SetTile(new Vector3Int(x - mapSize.x / 2, y1 - mapSize.y / 2, 0), roadTile); // mapSize�� ������ ���� ȭ�� �߾ӿ� ����
        for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            tilemap.SetTile(new Vector3Int(x2 - mapSize.x / 2, y - mapSize.y / 2, 0), roadTile);
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

    // ���� ũ�⿡ ���� Ÿ�� ����
    private void OnDrawRoom(int x, int y, int width, int height)
    {
        for (int i = x; i < x + width; i++)
            for (int j = y; j < y + height; j++)
                tilemap.SetTile(new Vector3Int(i - mapSize.x / 2, j - mapSize.y / 2, 0), roomTile); // ��ġ�� ȭ�� �߾ӿ� ����
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
    }
}