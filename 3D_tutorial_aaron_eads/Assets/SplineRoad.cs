using UnityEngine;
using System.Collections.Generic;

public class SplineRoad : MonoBehaviour
{
    [Header("Spline Control Points")]
    [SerializeField] private Transform[] controlPoints;

    [Header("Road Parameters")]
    [SerializeField] private int resolution = 50; // Points along spline
    [SerializeField] private float roadWidth = 4f;
    [SerializeField] private Material roadMaterial;

    private Mesh roadMesh;

    void Start()
    {
        if (controlPoints.Length < 4)
        {
            Debug.LogError("Need at least 4 control points for Catmull-Rom spline");
            return;
        }

        GenerateRoad();
    }

    void GenerateRoad()
    {
        // Generate spline points
        List<Vector3> splinePoints = GenerateCatmullRomSpline();

        // Generate road mesh from spline
        GenerateRoadMesh(splinePoints);
    }

    List<Vector3> GenerateCatmullRomSpline()
    {
        List<Vector3> points = new List<Vector3>();

        // Iterate through control point segments
        for (int i = 0; i < controlPoints.Length - 3; i++)
        {
            Vector3 p0 = controlPoints[i].position;
            Vector3 p1 = controlPoints[i + 1].position;
            Vector3 p2 = controlPoints[i + 2].position;
            Vector3 p3 = controlPoints[i + 3].position;

            // Generate points along this segment
            for (int j = 0; j < resolution; j++)
            {
                float t = j / (float)resolution;
                Vector3 point = CalculateCatmullRom(p0, p1, p2, p3, t);
                points.Add(point);
            }
        }

        return points;
    }

    Vector3 CalculateCatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        // Catmull-Rom spline formula
        float t2 = t * t;
        float t3 = t2 * t;

        Vector3 result = 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );

        return result;
    }

    void GenerateRoadMesh(List<Vector3> splinePoints)
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        float halfWidth = roadWidth / 2f;

        for (int i = 0; i < splinePoints.Count - 1; i++)
        {
            Vector3 current = splinePoints[i];
            Vector3 next = splinePoints[i + 1];

            // Calculate direction and perpendicular
            Vector3 forward = (next - current).normalized;
            Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;

            // Create road segment vertices
            Vector3 v0 = current - right * halfWidth;
            Vector3 v1 = current + right * halfWidth;
            Vector3 v2 = next + right * halfWidth;
            Vector3 v3 = next - right * halfWidth;

            int startIndex = vertices.Count;

            vertices.Add(v0);
            vertices.Add(v1);
            vertices.Add(v2);
            vertices.Add(v3);

            // Create two triangles for quad
            triangles.Add(startIndex);
            triangles.Add(startIndex + 2);
            triangles.Add(startIndex + 1);

            triangles.Add(startIndex);
            triangles.Add(startIndex + 3);
            triangles.Add(startIndex + 2);

            // UVs
            float uvY = i / (float)splinePoints.Count;
            uvs.Add(new Vector2(0, uvY));
            uvs.Add(new Vector2(1, uvY));
            uvs.Add(new Vector2(1, uvY));
            uvs.Add(new Vector2(0, uvY));
        }

        // Create mesh
        roadMesh = new Mesh();
        roadMesh.name = "Spline Road";
        roadMesh.vertices = vertices.ToArray();
        roadMesh.triangles = triangles.ToArray();
        roadMesh.uv = uvs.ToArray();
        roadMesh.RecalculateNormals();
        roadMesh.RecalculateBounds();

        // Assign mesh
        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf == null) mf = gameObject.AddComponent<MeshFilter>();
        mf.mesh = roadMesh;

        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null) mr = gameObject.AddComponent<MeshRenderer>();
        mr.material = roadMaterial;

        // Add collider
        MeshCollider mc = GetComponent<MeshCollider>();
        if (mc == null) mc = gameObject.AddComponent<MeshCollider>();
        mc.sharedMesh = roadMesh;
    }
}

