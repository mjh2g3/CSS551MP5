using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexturePlacement : MonoBehaviour {

    public Vector2 Offset = Vector2.zero;
    public Vector2 Scale = Vector2.one;
    public float Rotation = 0.0f;
    Vector2[] mInitUV = null; // initial values

    public void SaveInitUV(Vector2[] uv)
    {
        mInitUV = new Vector2[uv.Length];
        for (int i = 0; i < uv.Length; i++)
            mInitUV[i] = uv[i];
    }

	// Update is called once per frame
	void Update()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        Vector2[] uv = theMesh.uv;


        Matrix3x3 t = Matrix3x3Helpers.CreateTranslation(Offset);
        Matrix3x3 s = Matrix3x3Helpers.CreateScale(Scale);
        Matrix3x3 r = Matrix3x3Helpers.CreateRotation(Rotation);
        
        for (int i = 0; i < uv.Length; i++)
        {

            uv[i] = t * r * s * mInitUV[i];
            Debug.Log("This is uv[i] at i = " + i + ":" + uv[i]);
        }

        theMesh.uv = uv;
    }
}