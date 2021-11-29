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
        
        Debug.Log("v: " + v.Length);
        Debug.Log("n: " + n.Length);
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
        
        //Use list where index of outer List == vertex v index (v0, v1, v2, etc.),
        //the inner List will carry all of the indexes of the triangles that touch the vertex v
        List<List<int>> normsLoc = new List<List<int>>();
        for (int i = 0; i < N*M; i++)
        {
            normsLoc.Add(new List<int>());
        }

        //Retrieve the total count of triangles again
        int numTriangles = (N - 1) * (M - 1) * 2;
        //create triangle vector array for storing normal vectors of each triangle
        Vector3[] tri = new Vector3[numTriangles];
        //The cur variable will track which triangle we are working on
        int cur = 0;
        //This of this as rowXcolumn
        for (int i = 0; i < N - 1; i++)
        {
            for (int j = 0; j < M - 1; j++)
            {
                //TopLeft refers to the top left of the "square" that is a resultant of two triangles
                int topLeft = (i + 1) * M + j;
                //TopRight refers to the top right of the "square" that is a resultant of two triangles
                int topRight = (i + 1) * M + (j + 1);
                //BottomLeft refers to the bottom left of the "square" that is a resultant of two triangles
                int bottomLeft = i * M + j;
                //BottomRight refers to the bottom right of the "square" that is a resultant of two triangles
                int bottomRight = i * M + (j + 1);
                //Add the left triangle starting at the lowest left corner of the mesh first (1/2)
                tri[cur] = FaceNormal(v, topLeft, topRight, bottomLeft);
                //Add the triangle to each vertex to keep track for averaging
                normsLoc[topLeft].Add(cur);
                normsLoc[topRight].Add(cur);
                normsLoc[bottomLeft].Add(cur);
                //increment to the next triangle
                cur = cur + 1;
                //Add the right triangle starting at the lowest left corner of the mesh first (2/2)
                tri[cur] = FaceNormal(v, bottomLeft, topRight, bottomRight);
                //Add the triangle to each vertex to keep track for averaging
                normsLoc[bottomLeft].Add(cur);
                normsLoc[topRight].Add(cur);
                normsLoc[bottomRight].Add(cur);
                //increment to the next triangle
                cur = cur + 1;
            }
        }

        
        //Computation for solving the averaging of the triangles at each vertex/normal
        for (int i = 0; i < n.Length; i++)
        {
            Vector3 sumTris = new Vector3();
            for (int j = 0; j < normsLoc[i].Count; j++)
            {
                int index = normsLoc[i][j];
                if (j == 0)
                {
                    sumTris = tri[index];
                }
                else
                {
                    sumTris = sumTris + tri[index];
                }
            }
            n[i] = sumTris.normalized;
        }
        //Update the normal vectors
        UpdateNormals(v, n);

        /*
        Debug.Log("Print out of the n[]");
        for (int i = 0; i < n.Length; i++)
        {
            Debug.Log(n[i]);
        }
        */

        /*
        //Professor Old Implementation for triangle normals
        Vector3[] triNormal = new Vector3[8];
        triNormal[0] = FaceNormal(v, 3, 4, 0);
        triNormal[1] = FaceNormal(v, 0, 4, 1);

        triNormal[2] = FaceNormal(v, 4, 5, 1);
        triNormal[3] = FaceNormal(v, 1, 5, 2);

        triNormal[4] = FaceNormal(v, 6, 7, 3);
        triNormal[5] = FaceNormal(v, 3, 7, 4);

        triNormal[6] = FaceNormal(v, 7, 8, 4);
        triNormal[7] = FaceNormal(v, 4, 8, 5);

        //Professors Old Implementation for the normal vector averages
        n[0] = (triNormal[0] + triNormal[1]).normalized;
        n[1] = (triNormal[1] + triNormal[2] + triNormal[3]).normalized;
        n[2] = triNormal[3].normalized;
        n[3] = (triNormal[0] + triNormal[4] + triNormal[5]).normalized;
        n[4] = (triNormal[0] + triNormal[1] + triNormal[2] + triNormal[5] + triNormal[6] + triNormal[7]).normalized;
        n[5] = (triNormal[2] + triNormal[3]).normalized;
        n[6] = triNormal[4].normalized;
        n[7] = (triNormal[4] + triNormal[5] + triNormal[6]).normalized;
        n[8] = (triNormal[6] + triNormal[7]).normalized;
        */
    }
}
