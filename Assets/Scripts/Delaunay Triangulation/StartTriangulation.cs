using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartTriangulation : MonoBehaviour
{
    private List<DelaunayTriangulation.Triangle> triangles; // ��γ� �ﰢ������ ���� ������� �ﰢ���� 

    public List<GameObject> points; // ���� ����Ʈ
    public Transform vertexHolder; // ������ Point���� ��Ƴ��� �� ������Ʈ

    [SerializeField] private GameObject linePrefab; // ���η����� ������
    public Transform edgeHoler; // ������ ���η��������� ��Ƴ��� �� ������Ʈ
    public List<Edge> edges = new List<Edge>(); // Edge ����Ʈ
    public List<GameObject> lines = new List<GameObject>(); // ������ ���η����� ������Ʈ���� ������ ����Ʈ
    private HashSet<string> createdLines = new HashSet<string>(); // ������ ������ �ߺ� ���θ� �˻��ϱ� ���� HashSet

    public MST mstAlgorithms; // �ּ� ���д� Ʈ�� �˰���

    // ��γ� �ﰢ���� �޼���
    public void StartDelaunayTriangulation()
    {
        List<DelaunayTriangulation.Vertex> triangulationData = new List<DelaunayTriangulation.Vertex>();

        for (int i = 0; i < points.Count; ++i)
        {
            Vector3 position = points[i].transform.position;
            triangulationData.Add(new DelaunayTriangulation.Vertex(new Vector2(position.x, position.y), i));
        }

        DelaunayTriangulation.Triangulation triangulation = new DelaunayTriangulation.Triangulation(triangulationData);
        triangles = new List<DelaunayTriangulation.Triangle>(triangulation.triangles);

        // ������ ���ε� ����
        foreach (var line in lines)
        {
            Destroy(line);
        }
        lines.Clear();

        // ���ο� ���� ����
        // �ﰢ������ ��ȸ�ϸ� ���η������� ���� �ߺ����� edge ���� 
        foreach (var triangle in triangles)
        {
            TryCreateLine(triangle.vertex0.index, triangle.vertex1.index);
            TryCreateLine(triangle.vertex1.index, triangle.vertex2.index);
            TryCreateLine(triangle.vertex2.index, triangle.vertex0.index);
        }

        // ũ�罺Į �˰��� ����
        mstAlgorithms.StartKruskal();
    }

    // ���� �ʱ�ȭ �޼���
    public void ResetDungeon()
    {
        // Point ��ü�� �ı�
        foreach (Transform child in vertexHolder)
        {
            Destroy(child.gameObject);
        }
        points.Clear(); // points ����Ʈ �ʱ�ȭ

        // ���� ������ ��ü�� �ı�
        foreach (Transform child in edgeHoler)
        {
            Destroy(child.gameObject);
        }
        lines.Clear(); // lines ����Ʈ �ʱ�ȭ
        edges.Clear(); // edges ����Ʈ �ʱ�ȭ
        createdLines.Clear(); // createdLines HashSet �ʱ�ȭ

        mstAlgorithms.ResetDungeon(); // MST �ʱ�ȭ
    }

    // ���� �߽ɵ��� Vertex�� ����Ͽ� ��Ƴ��� �޼���
    public void AddPoint(Vector2 newPoint)
    {
        // �ߺ� �˻�
        foreach (var point in points)
        {
            if (Vector2.Distance(new Vector2(point.transform.position.x, point.transform.position.y), newPoint) < 0.01f)
                return; // �ߺ��� ��� �߰����� ����
        }

        // �� GameObject ����
        GameObject newGameObject = new GameObject("Point");
        newGameObject.transform.position = new Vector3(newPoint.x, newPoint.y, 0);

        // �θ� GameObject ����
        if (vertexHolder != null)
        {
            newGameObject.transform.SetParent(vertexHolder.transform);
        }

        // points ����Ʈ�� �߰�
        points.Add(newGameObject);
    }

    // �ߺ����� ���η������� line ����
    private void TryCreateLine(int index1, int index2)
    {
        // �� �ε����� �����Ͽ� ������ Ű ����
        string lineKey = index1 < index2 ? $"{index1}-{index2}" : $"{index2}-{index1}";

        // �̹� ������ �������� Ȯ��
        if (!createdLines.Contains(lineKey))
        {
            // Edge ��ü ���� �� �߰�
            float weight = Vector3.Distance(points[index1].transform.position, points[index2].transform.position);
            edges.Add(new Edge(index1, index2, weight));

            CreateLine(points[index1].transform.position, points[index2].transform.position);
            createdLines.Add(lineKey); // Ű�� HashSet�� �߰�
        }
    }

    // ���η����� ���� �޼���
    private void CreateLine(Vector3 start, Vector3 end)
    {
        GameObject lineGO = Instantiate(linePrefab, edgeHoler.transform); // Instantiate with lineParent as parent
        LineRenderer lineRenderer = lineGO.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        lines.Add(lineGO);
    }

    /*
    void OnDrawGizmos()
    {
        if (triangles == null) return;

        Gizmos.color = Color.red;
        foreach (var triangle in triangles)
        {
            Gizmos.DrawLine(points[triangle.vertex0.index].transform.position, points[triangle.vertex1.index].transform.position);
            Gizmos.DrawLine(points[triangle.vertex1.index].transform.position, points[triangle.vertex2.index].transform.position);
            Gizmos.DrawLine(points[triangle.vertex2.index].transform.position, points[triangle.vertex0.index].transform.position);
        }
    }
    */
}
