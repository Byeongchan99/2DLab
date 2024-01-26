using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class MST : MonoBehaviour
{
    public StartTriangulation startTriangulation; // StartTriangulation ��ũ��Ʈ ����
    public Transform MSTHolder;
    [SerializeField] private GameObject linePrefab; // ���� ������ ������

    // ũ�罺Į �˰��� ���� �޼���
    public void StartKruskal()
    {
        List<Edge> mst = Kruskal(startTriangulation.edges, startTriangulation.points);

        // MST�� ���ϴ� �� ������ ���� LineRenderer�� ����Ͽ� ������ �׸��ϴ�.
        foreach (Edge edge in mst)
        {
            Vector3 start = startTriangulation.points[edge.u].transform.position;
            Vector3 end = startTriangulation.points[edge.v].transform.position;
            CreateLine(start, end);
        }
    }

    // ���� �ʱ�ȭ �޼���
    public void ResetDungeon()
    {      
        // ���� ������ ��ü�� �ı�
        foreach (Transform child in MSTHolder)
        {
            Destroy(child.gameObject);
        }
    }

    // ���� �������� ������� �ּ� ���д� Ʈ�� ǥ��
    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineGO = Instantiate(linePrefab, MSTHolder.transform);
        LineRenderer lineRenderer = lineGO.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    // ũ�罺Į �˰��� �޼���
    private List<Edge> Kruskal(List<Edge> edges, List<GameObject> points)
    {
        edges.Sort((a, b) => a.weight.CompareTo(b.weight)); // ����ġ�� ���� ���� ����
        UnionFind uf = new UnionFind(points.Count);

        List<Edge> mst = new List<Edge>();
        foreach (var edge in edges)
        {
            if (uf.Find(edge.u) != uf.Find(edge.v))
            {
                uf.Union(edge.u, edge.v);
                mst.Add(edge);
            }
        }
        return mst;
    }
}

public class Edge
{
    public int u, v;
    public float weight;

    public Edge(int u, int v, float weight)
    {
        this.u = u;
        this.v = v;
        this.weight = weight;
    }
}

public class UnionFind
{
    private int[] parent;

    public UnionFind(int n)
    {
        parent = new int[n];
        for (int i = 0; i < n; i++)
            parent[i] = i;
    }

    public int Find(int x)
    {
        if (parent[x] == x) return x;
        return parent[x] = Find(parent[x]);
    }

    public void Union(int x, int y)
    {
        int px = Find(x);
        int py = Find(y);
        if (px != py) parent[px] = py;
    }
}
