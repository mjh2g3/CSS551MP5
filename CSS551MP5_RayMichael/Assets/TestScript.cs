using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [Min(2)]
    public int N = 2;
    [Min(2)]
    public int M = 2;

    public Transform quad;

    // Start is called before the first frame update
    void Start()
    {
        Vector3[] vects = new Vector3[N * M];         // NxM Mesh needs NxM vertices
        int[] tris = new int[(N-1) * (M-1) * 2 * 3];  // Number of triangles = (N-1) * (M-1) * 2, and each triangle's vertices can be order 3 different ways
        Vector3[] norms = new Vector3[N * M];         // MUST be the same as number of vertices

        float dN = quad.localScale.y/(N-1);
        float dM = quad.localScale.x/(M-1);

        Vector3 startPoint = new Vector3(-quad.localScale.x/2, 0, -quad.localScale.y/2);
        Debug.Log(quad.localScale);

        for (int n = 0; n < N ; n++) {
            for (int m = 0; m < M ; m++) {
                Debug.Log(n*M + m);
                vects[n*M + m] = startPoint + new Vector3(m*dM, 0, n*dN);
            }
        }

        foreach (Vector3 v in vects) {
            GameObject gObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gObj.transform.localPosition = v;
            gObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
