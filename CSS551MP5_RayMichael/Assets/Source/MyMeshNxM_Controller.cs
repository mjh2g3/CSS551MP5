using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MyMeshNxM : MonoBehaviour {

    //Variable changed to protected to provide access to child subclass CylinderMesh
    protected GameObject[] mControllers;

    public virtual void InitControllers(Vector3[] v)
    {
        mControllers = new GameObject[v.Length];
        for (int i =0; i<v.Length; i++ )
        {
            mControllers[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mControllers[i].transform.name = "ManSphere";
            mControllers[i].transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

            mControllers[i].transform.localPosition = v[i];
            mControllers[i].transform.parent = this.transform;
        }
    }

    public GameObject[] GetControllers()
    {
        return mControllers;
    }
}