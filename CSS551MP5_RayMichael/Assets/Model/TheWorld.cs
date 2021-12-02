using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour
{
    //The selected vertex sphere controller
    public GameObject mSelected;
    
    //The two meshes the user can toggle between (planar mMesh and cylinder cMesh)
    public MyMeshNxM mMesh = null;
    public CylinderMesh cMesh = null;

    
    // Start is called before the first frame update
    void Start()
    {
        //Step 1: Both planar and cylinder mesh are initialized
        Debug.Assert(mMesh != null);
        Debug.Assert(cMesh != null);

        //Step 2: Set the cylinder mesh to inactive so that it does not show up
        Debug.Log("Set the cMesh inActive");
        cMesh.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if((xAxis != null) && (yAxis != null) && (zAxis != null))
        {
            UpdateXAxis();
            UpdateYAxis();
            UpdateZAxis();
        }
    }

    public void SetSelected(GameObject g)
    {
        if (mSelected != null)
        {
            mSelected.GetComponent<Renderer>().material.color = mOrgObjColor;
            DestroyManipulatorAxes();
        }

        mSelected = g;
        if (mSelected != null)
        {
            mOrgObjColor = g.GetComponent<Renderer>().material.color; // save a copy
            mSelected.GetComponent<Renderer>().material.color = kSelectedColor;
            CreateManipulatorAxes();
        }
    }

    public void ResetSelected() {
        if (mSelected != null)
        {
            mSelected.GetComponent<Renderer>().material.color = mOrgObjColor;
            mSelected = null;
        }
    }
}
