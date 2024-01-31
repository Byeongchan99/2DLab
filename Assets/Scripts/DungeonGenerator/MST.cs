using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

public class MST : MonoBehaviour
{
    public StartTriangulation startTriangulation; // StartTriangulation 스크립트 참조
    public Transform MSTHolder;
    [SerializeField] private GameObject linePrefab; // 라인 렌더러 프리팹

    // 크루스칼 알고리즘 실행 메서드
    public void StartKruskal()
    {
        List<Edge> mst = Kruskal(startTriangulation.edges, startTriangulation.points);

        // MST에 속하는 각 간선에 대해 LineRenderer를 사용하여 라인을 그립니다.
        foreach (Edge edge in mst)
        {
            Vector3 start = startTriangulation.points[edge.u].transform.position;
            Vector3 end = startTriangulation.points[edge.v].transform.position;
            CreateLine(start, end);
        }
    }

    // 던전 초기화 메서드
    public void ResetDungeon()
    {      
        // 라인 렌더러 객체들 파괴
        foreach (Transform child in MSTHolder)
        {
            Destroy(child.gameObject);
        }
    }

    // 라인 렌더러로 만들어진 최소 스패닝 트리 표시
    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineGO = Instantiate(linePrefab, MSTHolder.transform);
        LineRenderer lineRenderer = lineGO.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    // 크루스칼 알고리즘 메서드
    private List<Edge> Kruskal(List<Edge> edges, List<GameObject> points)
    {
        edges.Sort((a, b) => a.weight.CompareTo(b.weight)); // 가중치에 따라 간선 정렬
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
