using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshNxM : MonoBehaviour
{
    LineSegment[] mNormals;

    void InitNormals(Vector3[] v, Vector3[] n)
    {
        mNormals = new LineSegment[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            GameObject o = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            mNormals[i] = o.AddComponent<LineSegment>();
            mNormals[i].SetWidth(0.05f);
            mNormals[i].transform.SetParent(this.transform);
        }
        UpdateNormals(v, n);
    }

    void UpdateNormals(Vector3[] v, Vector3[] n)
    {
        for (int i = 0; i < v.Length; i++)
        {
            mNormals[i].SetEndPoints(v[i], v[i] + 1.0f * n[i]);
        }
    }

    Vector3 FaceNormal(Vector3[] v, int i0, int i1, int i2)
    {
        Vector3 a = v[i1] - v[i0];
        Vector3 b = v[i2] - v[i0];
        return Vector3.Cross(a, b).normalized;
    }

    void ComputeNormals(Vector3[] v, Vector3[] n)
    {
        /*
        // process two new triangles that can be traversed from that point
        if (currentTriangle < numTriangles && m < M - 1)
        {
            tris[currentTriangle * 3] = n * M + m;
            tris[currentTriangle * 3 + 1] = (n + 1) * M + m;
            tris[currentTriangle * 3 + 2] = (n + 1) * M + (m + 1);
            currentTriangle++; // increment currentTriangle

            tris[currentTriangle * 3] = n * M + m;
            tris[currentTriangle * 3 + 1] = (n + 1) * M + (m + 1);
            tris[currentTriangle * 3 + 2] = n * M + (m + 1);
            currentTriangle++; // increment currentTriangle
        }
        */
        int numTriangles = (N - 1) * (M - 1) * 2;
        Vector3[] tri = new Vector3[numTriangles];
        Debug.Log(tri.Length);
        for (int i = 0; i < tri.Length; i++)
        {
            tri[i] = FaceNormal(v[);
        }







        Vector3[] triNormal = new Vector3[8];
        triNormal[0] = FaceNormal(v, 3, 4, 0);
        triNormal[1] = FaceNormal(v, 0, 4, 1);
        triNormal[2] = FaceNormal(v, 4, 5, 1);
        triNormal[3] = FaceNormal(v, 1, 5, 2);
        triNormal[4] = FaceNormal(v, 6, 7, 3);
        triNormal[5] = FaceNormal(v, 3, 7, 4);
        triNormal[6] = FaceNormal(v, 7, 8, 4);
        triNormal[7] = FaceNormal(v, 4, 8, 5);

        n[0] = (triNormal[0] + triNormal[1]).normalized;
        n[1] = (triNormal[1] + triNormal[2] + triNormal[3]).normalized;
        n[2] = triNormal[3].normalized;
        n[3] = (triNormal[0] + triNormal[4] + triNormal[5]).normalized;
        n[4] = (triNormal[0] + triNormal[1] + triNormal[2] + triNormal[5] + triNormal[6] + triNormal[7]).normalized;
        n[5] = (triNormal[2] + triNormal[3]).normalized;
        n[6] = triNormal[4].normalized;
        n[7] = (triNormal[4] + triNormal[5] + triNormal[6]).normalized;
        n[8] = (triNormal[6] + triNormal[7]).normalized;
        UpdateNormals(v, n);
    }
}
