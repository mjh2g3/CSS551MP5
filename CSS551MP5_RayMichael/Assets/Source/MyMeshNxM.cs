
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshNxM : MonoBehaviour {

    private float meshLength = 10.0f;
    private float meshWidth = 10.0f;

    private int N = 2;
    private int M = 2;

	// Use this for initialization
	void Start () {
        MeshInitialization();
        /*
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        int numTriangles = (N-1) * (M-1) * 2;
        int currentTriangle = 0;

        Vector3[] vects = new Vector3[N * M];         // NxM Mesh needs NxM vertices
        int[] tris = new int[numTriangles * 3];  // Number of triangles = (N-1) * (M-1) * 2, and each triangle has 3 vertices
       
        Vector3[] norms = new Vector3[N * M];         // MUST be the same as number of vertices

        //Define dN and dM
        
        float dN = meshLength / (N-1);
        float dM = meshWidth / (M-1);
        
        //Define a start point (lower left corner of mesh)
        Vector3 startPoint = new Vector3(-5.0f, 0.0f, -5.0f);
        
        for (int n = 0; n < N ; n++) {
            for (int m = 0; m < M ; m++) {
                vects[n*M + m] = startPoint + new Vector3(m*dM, 0, n*dN);
                
                // process two new triangles that can be traversed from that point
                if (currentTriangle < numTriangles && m < M-1) {
                    tris[currentTriangle * 3] = n*M + m;
                    tris[currentTriangle * 3 + 1] = (n+1)*M + m;
                    tris[currentTriangle * 3 + 2] = (n+1)*M + (m+1);
                    currentTriangle++; // increment currentTriangle

                    tris[currentTriangle * 3] = n*M + m;
                    tris[currentTriangle * 3 + 1] = (n+1)*M + (m+1);
                    tris[currentTriangle * 3 + 2] = n*M + (m+1);
                    currentTriangle++; // increment currentTriangle
                }
            }
        }

        for (int idx = 0; idx < norms.Length; idx++) {
            norms[idx] = new Vector3(0, 1, 0);
        }

        theMesh.vertices = vects; //  new Vector3[3];
        theMesh.triangles = tris; //  new int[3];
        theMesh.normals = norms;

        InitControllers(vects);
        InitNormals(vects, norms);
        */
    }

    // Update is called once per frame
    void Update () {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector3[] v = theMesh.vertices;
        Vector3[] n = theMesh.normals;
        for (int i = 0; i<mControllers.Length; i++)
        {
            v[i] = mControllers[i].transform.localPosition;
        }

        ComputeNormals(v, n);

        theMesh.vertices = v;
        theMesh.normals = n;
	}

    private void MeshInitialization()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;   // get the mesh component
        theMesh.Clear();    // delete whatever is there!!

        int numTriangles = (N - 1) * (M - 1) * 2;
        int currentTriangle = 0;

        Vector3[] vects = new Vector3[N * M];         // NxM Mesh needs NxM vertices
        int[] tris = new int[numTriangles * 3];  // Number of triangles = (N-1) * (M-1) * 2, and each triangle has 3 vertices

        Vector3[] norms = new Vector3[N * M];         // MUST be the same as number of vertices

        //Define dN and dM

        float dN = meshLength / (N - 1);
        float dM = meshWidth / (M - 1);

        //Define a start point (lower left corner of mesh)
        Vector3 startPoint = new Vector3(-5.0f, 0.0f, -5.0f);

        for (int n = 0; n < N; n++)
        {
            for (int m = 0; m < M; m++)
            {
                vects[n * M + m] = startPoint + new Vector3(m * dM, 0, n * dN);

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

        theMesh.vertices = vects; //  new Vector3[3];
        theMesh.triangles = tris; //  new int[3];
        theMesh.normals = norms;

        InitControllers(vects);
        InitNormals(vects, norms);
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
        N = res[0];
        M = res[1];
        MeshInitialization();
    }
}