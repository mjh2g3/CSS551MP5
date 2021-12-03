
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshNxM : MonoBehaviour {

    protected float meshLength = 10.0f;
    protected float meshWidth = 10.0f;

    protected int N = 2;
    protected int M = 2;

    protected Vector3[] verts; // vertices on the mesh
    protected int[] tris; // triangles defined by vertices
    protected Vector3[] norms; // normals at each vertex
    protected Vector2[] uv; // uv values for texture mapping

    protected bool ManipulationOn = false;

    // Use this for initialization
    void Start () {
        MeshInitialization();
    }

    // Update is called once per frame
    void Update () {
        if ((mControllers != null) && (ManipulationOn))
        {
            UpdateMeshNormals();
        }
    }

    private void UpdateMeshNormals()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        for (int i = 0; i < mControllers.Length; i++)
        {
            v[i] = mControllers[i].transform.localPosition;
        }

        ComputeNormals(v, n);
        theMesh.vertices = v;
        theMesh.normals = n;
    }

    public virtual void MeshInitialization()
    {
        //Step 1: Obtain the mesh component and delete whatever is there
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!
        
        //Step 2: Identify the number of N, M, and Triangles
        int numTriangles = (N - 1) * (M - 1) * 2;
        
        //Step 3: Create arrays for vertices, triangle vertices, normals, and uv vertices
        verts = new Vector3[N * M];         // NxM Mesh needs NxM vertices
        tris = new int[numTriangles * 3];  // Number of triangles = (N-1) * (M-1) * 2, and each triangle has 3 vertices
        norms = new Vector3[N * M];         // MUST be the same as number of vertices
        uv = new Vector2[N * M];

        //Step 4: Define dN and dM which are the distances between each vertex in the N and M direction
        float dN = meshLength / (N - 1);
        float dM = meshWidth / (M - 1);

        //Step 5a: Define a start point (lower left corner of mesh) and variable to track which triangle is being created
        Vector3 startVertex = new Vector3(-5.0f, 0.0f, -5.0f);
        int currentTriangle = 0;

        //Step 5b: Define start point (lower left corner of mesh) for the UV vertices
        Vector2 startUV = new Vector2(-5, -5);

        //Step 6: Compute the vertices, triangle vertices, and normal vectors at each vertex
        for (int n = 0; n < N; n++)
        {
            for (int m = 0; m < M; m++)
            {
                // define current vertex position
                verts[n * M + m] = startVertex + new Vector3(m * dM, 0, n * dN);

                uv[n * M + m] = startUV + new Vector2(m * dM / meshWidth, n * dN/meshLength);

                // process two new triangles that can be traversed from current vertex
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
        theMesh.vertices = verts;
        theMesh.triangles = tris;
        theMesh.normals = norms;
        theMesh.uv = uv;

        GetComponent<TexturePlacement>().SaveInitUV(uv);
    }


    public List<int> GetResolution()
    {
        List<int> res = new List<int>();
        res.Add(N);
        res.Add(M);
        return res;
    }

    public void SetResolution(List<int> res)
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

        N = res[0];
        M = res[1];
        MeshInitialization();
        if (ManipulationOn)
        {
            //Step 8: Initialize the sphere controllers and normal vector line segments
            InitControllers(verts);
            InitNormals(verts, norms);
        }
    }

    public Vector3[] GetVects()
    {
        return verts;
    }

    public int[] GetTris()
    {
        return tris;
    }

    public Vector3[] GetNorms()
    {
        return norms;
    }

    public void SwitchOnManipulation(bool status)
    {
        ManipulationOn = status;
        if (status)
        {
            InitControllers(verts);
            InitNormals(verts, norms);
        }
        else
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
        }
    }

    //no need to override if applying a mesh object with CylinderMesh script just fyi
    public virtual void SetRotation(double rotation)
    {
        
    }

    //no need to override if applying a mesh object with CylinderMesh script just fyi
    public virtual double GetRotation()
    {
        return 0.0;
    }

}