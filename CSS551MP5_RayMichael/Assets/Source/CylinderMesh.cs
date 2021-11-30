using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderMesh : MyMeshNxM
{
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */

    //MeshInitialization Override to adapt planar to cylinder layout
    public override void MeshInitialization()
    {
        //Step 1: Obtain the mesh component and delete whatever is there
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        //Step 2: Identify the number of N, M, and Triangles
        int numTriangles = (N - 1) * (M - 1) * 2;

        //Step 3: Create arrays to store vertices in mesh, triangle vertices, and normal vectors
        verts = new Vector3[N * M];         // NxM Mesh needs NxM vertices
        tris = new int[numTriangles * 3];  // Number of triangles = (N-1) * (M-1) * 2, and each triangle has 3 vertices
        norms = new Vector3[N * M];         // MUST be the same as number of vertices

        //Step 4: Define dN and dM which are the distances between each vertex in the N and M direction
        float dN = meshLength / (N - 1);
        float dM = meshWidth / (M - 1);

        //Step 5: Define a start point (lower left corner of mesh) and variable to track which triangle is being created
        Vector3 startPoint = new Vector3(-5.0f, 0.0f, -5.0f);
        int currentTriangle = 0;

        //Step 6: Compute the vertices, triangle vertices, and normal vectors at each vertex
        for (int n = 0; n < N; n++)
        {
            for (int m = 0; m < M; m++)
            {
                verts[n * M + m] = startPoint + new Vector3(m * dM, 0, n * dN);

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
            }
        }
        for (int idx = 0; idx < norms.Length; idx++)
        {
            norms[idx] = new Vector3(0, 1, 0);
        }

        //Step 7: Assign the vertices, triangles, and normal vectors to the mesh
        theMesh.vertices = verts; //  new Vector3[3];
        theMesh.triangles = tris; //  new int[3];
        theMesh.normals = norms;

        //Step 8: Initialize the sphere controllers and normal vector line segments
        //InitControllers(vects);
        //InitNormals(vects, norms);
    }
}
