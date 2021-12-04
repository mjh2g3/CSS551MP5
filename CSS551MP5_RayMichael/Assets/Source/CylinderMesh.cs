using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CylinderMesh : MyMeshNxM
{
    private double rotationDegrees = 10.0;
    private float radius = 5.0f;
    
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

    private double ConvertDegreesToRadians(double degrees)
    {
        double rad = (Math.PI / 180.0) * degrees;
        return rad;
    }

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
        
        //Vector3 startPoint = new Vector3(radius, -5.0f, 0.0f);
        int currentTriangle = 0;

        //Step 6: Compute the vertices, triangle vertices, and normal vectors at each vertex
        for (int n = 0; n < N; n++)
        {
            for (int m = 0; m < M; m++)
            {
                
                //Obtain an angle in radians
                double radAngle = ConvertDegreesToRadians(rotationDegrees / (M-1));
                //Increment angle by 2 * theta at each row of vectors
                radAngle = radAngle * m;
                //Compute the x and z values
                float x = (float)(radius * Math.Cos(radAngle));
                float z = (float)(radius * Math.Sin(radAngle));
                verts[n * M + m] = new Vector3(x, n * dN, z);

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
     //   theMesh.vertices = verts; //  new Vector3[3];
     //   theMesh.triangles = tris; //  new int[3];
     //   theMesh.normals = norms;

        //Step 8: Initialize the sphere controllers and normal vector line segments
        //InitControllers(vects);
        //InitNormals(vects, norms);

        //TESTING-------------------------------------------------------------------------------------------------------------------
        //Use list where index of outer List == vertex v index (v0, v1, v2, etc.),
        //the inner List will carry all of the indexes of the triangles that touch the vertex v
        List<List<int>> normsLoc = new List<List<int>>();
        for (int i = 0; i < N * M; i++)
        {
            normsLoc.Add(new List<int>());
        }

        //Retrieve the total count of triangles again
    //    int numTriangles = (N - 1) * (M - 1) * 2;
        //create triangle vector array for storing normal vectors of each triangle
        Vector3[] tri = new Vector3[numTriangles];
        //The cur variable will track which triangle we are working on
        int cur = 0;
        //Think of this as row-major 1D array to represent 2D array
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
                tri[cur] = FaceNormal(verts, topLeft, topRight, bottomLeft);
                //Add the triangle to each vertex to keep track for averaging
                normsLoc[topLeft].Add(cur);
                normsLoc[topRight].Add(cur);
                normsLoc[bottomLeft].Add(cur);
                //increment to the next triangle
                cur = cur + 1;
                //Add the right triangle starting at the lowest left corner of the mesh first (2/2)
                tri[cur] = FaceNormal(verts, bottomLeft, topRight, bottomRight);
                //Add the triangle to each vertex to keep track for averaging
                normsLoc[bottomLeft].Add(cur);
                normsLoc[topRight].Add(cur);
                normsLoc[bottomRight].Add(cur);
                //increment to the next triangle
                cur = cur + 1;
            }
        }

        //Computation for solving the averaging of the triangles at each vertex/normal
        for (int i = 0; i < norms.Length; i++)
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
            if ((rotationDegrees == 360) && (i % M == 0))
            {

                for (int j = 0; j < normsLoc[i + M - 1].Count; j++)
                {
                    int index = normsLoc[i + M - 1][j];
                    if (j == 0)
                    {
                        sumTris = tri[index];
                    }
                    else
                    {
                        sumTris = sumTris + tri[index];
                    }
                }
            }
            norms[i] = sumTris.normalized;
        }


        //Step 7: Assign the vertices, triangles, and normal vectors to the mesh
        theMesh.vertices = verts; //  new Vector3[3];
        theMesh.triangles = tris; //  new int[3];
        theMesh.normals = norms;

    }

    //Updated the color of the manipulation spheres; only the edge spheres are to be white and manipulatable
    public override void InitControllers(Vector3[] v)
    {
        mControllers = new GameObject[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            mControllers[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mControllers[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            mControllers[i].transform.localPosition = v[i];
            mControllers[i].transform.parent = this.transform;
            if (i % M != 0)
            {
                Renderer rend = mControllers[i].GetComponent<Renderer>();
                rend.material.color = Color.black;
            }
            else if (i % M == 0)
            {
                mControllers[i].transform.name = "ManSphere";
            }
        }

        //Code for renaming black edge spheres for 360; it works however affects the direct manipulation code in TheWorld script. would need to revise
        /*
        if (rotationDegrees == 360)
        {
            for (int i = 0; i < v.Length; i++)
            {
                if (i % M == 0)
                {
                    mControllers[i + M - 1].transform.name = "ManSphere";
                }
            }
        }
        */









    }

    //no need to override if applying a mesh object with CylinderMesh script just fyi
    public override void SetRotation(double rotation)
    {
        if (mControllers != null)
        {
            for (int i = 0; i < mNormals.Length; i++)
            {
                Destroy(mNormals[i].gameObject);
                Destroy(mControllers[i]);

            }
        }
        mControllers = null;
        mNormals = null;

        rotationDegrees = rotation;
        MeshInitialization();
        if (ManipulationOn)
        {
            //Step 8: Initialize the sphere controllers and normal vector line segments
            InitControllers(verts);
            InitNormals(verts, norms);
        }
    }

    public override void ComputeNormals(Vector3[] v, Vector3[] n)
    {
        //Use list where index of outer List == vertex v index (v0, v1, v2, etc.),
        //the inner List will carry all of the indexes of the triangles that touch the vertex v
        List<List<int>> normsLoc = new List<List<int>>();
        for (int i = 0; i < N * M; i++)
        {
            normsLoc.Add(new List<int>());
        }

        //Retrieve the total count of triangles again
        int numTriangles = (N - 1) * (M - 1) * 2;
        //create triangle vector array for storing normal vectors of each triangle
        Vector3[] tri = new Vector3[numTriangles];
        //The cur variable will track which triangle we are working on
        int cur = 0;
        //Think of this as row-major 1D array to represent 2D array
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
            if ((rotationDegrees == 360) && (i % M == 0))
            {
                
                for (int j = 0; j < normsLoc[i + M - 1].Count; j++)
                {
                    int index = normsLoc[i + M - 1][j];
                    if (j == 0)
                    {
                        sumTris = tri[index];
                    }
                    else
                    {
                        sumTris = sumTris + tri[index];
                    }
                }
            }
            n[i] = sumTris.normalized;
        }


        //Update the normal vectors
        UpdateNormals(v, n);
    }


    //no need to override if applying a mesh object with CylinderMesh script just fyi
    public override double GetRotation()
    {
        return rotationDegrees;
    }
}
