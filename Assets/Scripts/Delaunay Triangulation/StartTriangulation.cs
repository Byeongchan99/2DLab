using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartTriangulation : MonoBehaviour
{
    private List<DelaunayTriangulation.Triangle> triangles; // 들로네 삼각분할을 통해 만들어진 삼각형들 

    public List<GameObject> points; // 정점 리스트
    public Transform vertexHolder; // 생성된 Point들을 모아놓은 빈 오브젝트

    [SerializeField] private GameObject linePrefab; // 라인렌더러 프리팹
    public Transform edgeHoler; // 생성된 라인렌더러들을 모아놓은 빈 오브젝트
    public List<Edge> edges = new List<Edge>(); // Edge 리스트
    public List<GameObject> lines = new List<GameObject>(); // 생성된 라인렌더러 오브젝트들을 저장할 리스트
    private HashSet<string> createdLines = new HashSet<string>(); // 생성된 라인의 중복 여부를 검사하기 위한 HashSet

    public MST mstAlgorithms; // 최소 스패닝 트리 알고리즘

    // 들로네 삼각분할 메서드
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

        // 기존의 라인들 삭제
        foreach (var line in lines)
        {
            Destroy(line);
        }
        lines.Clear();

        // 새로운 라인 생성
        // 삼각형들을 순회하며 라인렌더러를 통해 중복없이 edge 생성 
        foreach (var triangle in triangles)
        {
            TryCreateLine(triangle.vertex0.index, triangle.vertex1.index);
            TryCreateLine(triangle.vertex1.index, triangle.vertex2.index);
            TryCreateLine(triangle.vertex2.index, triangle.vertex0.index);
        }

        // 크루스칼 알고리즘 실행
        mstAlgorithms.StartKruskal();
    }

    // 던전 초기화 메서드
    public void ResetDungeon()
    {
        // Point 객체들 파괴
        foreach (Transform child in vertexHolder)
        {
            Destroy(child.gameObject);
        }
        points.Clear(); // points 리스트 초기화

        // 라인 렌더러 객체들 파괴
        foreach (Transform child in edgeHoler)
        {
            Destroy(child.gameObject);
        }
        lines.Clear(); // lines 리스트 초기화
        edges.Clear(); // edges 리스트 초기화
        createdLines.Clear(); // createdLines HashSet 초기화

        mstAlgorithms.ResetDungeon(); // MST 초기화
    }

    // 방의 중심들을 Vertex로 사용하여 모아놓는 메서드
    public void AddPoint(Vector2 newPoint)
    {
        // 중복 검사
        foreach (var point in points)
        {
            if (Vector2.Distance(new Vector2(point.transform.position.x, point.transform.position.y), newPoint) < 0.01f)
                return; // 중복된 경우 추가하지 않음
        }

        // 새 GameObject 생성
        GameObject newGameObject = new GameObject("Point");
        newGameObject.transform.position = new Vector3(newPoint.x, newPoint.y, 0);

        // 부모 GameObject 설정
        if (vertexHolder != null)
        {
            newGameObject.transform.SetParent(vertexHolder.transform);
        }

        // points 리스트에 추가
        points.Add(newGameObject);
    }

    // 중복없이 라인렌더러로 line 생성
    private void TryCreateLine(int index1, int index2)
    {
        // 두 인덱스를 정렬하여 고유한 키 생성
        string lineKey = index1 < index2 ? $"{index1}-{index2}" : $"{index2}-{index1}";

        // 이미 생성된 라인인지 확인
        if (!createdLines.Contains(lineKey))
        {
            // Edge 객체 생성 및 추가
            float weight = Vector3.Distance(points[index1].transform.position, points[index2].transform.position);
            edges.Add(new Edge(index1, index2, weight));

            CreateLine(points[index1].transform.position, points[index2].transform.position);
            createdLines.Add(lineKey); // 키를 HashSet에 추가
        }
    }

    // 라인렌더러 생성 메서드
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
