using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriangulationTest : MonoBehaviour
{
    public GameObject[] points;
    private List<DelaunayTriangulation.Triangle> triangles; // 삼각형 데이터를 저장할 리스트
    private List<Vector3> meshData; // 클래스 수준에서 meshData를 정의

    void Start()
    {
        meshData = new List<Vector3>();
    }

    void Update()
    {
        Mesh mesh = new Mesh();
        meshData.Clear(); // 매 업데이트마다 meshData를 초기화

        List<DelaunayTriangulation.Vertex> triangulationData = new List<DelaunayTriangulation.Vertex>();
        List<int> indecies = new List<int>();

        for (int i = 0; i < points.Length; ++i)
        {
            Vector3 position = points[i].transform.position;
            meshData.Add(position);
            triangulationData.Add(new DelaunayTriangulation.Vertex(new Vector2(position.x, position.y), i));
        }

        DelaunayTriangulation.Triangulation triangulation = new DelaunayTriangulation.Triangulation(triangulationData);
        triangles = new List<DelaunayTriangulation.Triangle>(triangulation.triangles);

        foreach (DelaunayTriangulation.Triangle triangle in triangulation.triangles)
        {
            indecies.Add(triangle.vertex0.index);
            indecies.Add(triangle.vertex1.index);
            indecies.Add(triangle.vertex2.index);
        }

        mesh.SetVertices(meshData);
        mesh.SetIndices(indecies.ToArray(), MeshTopology.Triangles, 0);
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();

        MeshFilter meshFilter = GetComponent<MeshFilter>();

        meshFilter.mesh = mesh;
    }

    // Gizmos를 이용해 삼각형의 엣지를 그리는 메소드
    void OnDrawGizmos()
    {
        if (triangles == null) return;

        Gizmos.color = Color.red; // 엣지의 색상 설정
        foreach (var triangle in triangles)
        {
            Gizmos.DrawLine(meshData[triangle.vertex0.index], meshData[triangle.vertex1.index]);
            Gizmos.DrawLine(meshData[triangle.vertex1.index], meshData[triangle.vertex2.index]);
            Gizmos.DrawLine(meshData[triangle.vertex2.index], meshData[triangle.vertex0.index]);
        }
    }
}
