using System.Collections;
using System.Collections.Generic;
using System;
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

    private bool FoundSelected(GameObject g)
    {
        
        return mSelected == g;
    }

    public void UpdateSelected(Vector3 pos)
    {
        if (dropDownIndx == 0)
        {
            //perform regular manipulation
            mSelected.transform.localPosition += pos;
        }
        else if (dropDownIndx == 1)
        {
            //mSelected.transform.localPosition += pos;
            Vector3 newPos = new Vector3();
            newPos = mSelected.transform.localPosition;
            newPos += pos;
            float radius = newPos.x;
            //perform cylinder sweep translation
            //Need to obtain the array of controllers
            GameObject[] v = cMesh.GetControllers();
            //Next find the index of the currently selected vertex
            int found = Array.FindIndex(v, FoundSelected);
            
            //Next find the resolution to know how many vertexes are on the same row as mSelected
            List<int> res = cMesh.GetResolution();
            //Also need to rotation for movement on x/z axis
            double rot = cMesh.GetRotation();

            //if manipulation is occuring on the x axis
            if (pos.x != 0)
            {
                for (int m = 0; m < res[1]; m++)
                {
                    //Obtain an angle in radians
                    double rad = (Math.PI / 180.0) * (rot / (res[1] - 1));

                    //Increment angle by 2 * theta at each row of vectors
                    rad = rad * m;
                    //Compute the x and z values
                    float x = (float)(radius * Math.Cos(rad));
                    float z = (float)(radius * Math.Sin(rad));
                    Vector3 sweep = new Vector3(x, newPos.y, z);
                    v[found + m].transform.localPosition = sweep;
                }
            }
            //if manipulation is occuring on the x axis
            if (pos.y != 0)
            {
                for (int m = 0; m < res[1]; m++)
                {
                    //Obtain an angle in radians
                    double rad = (Math.PI / 180.0) * (rot / res[1]);

                    //Increment angle by 2 * theta at each row of vectors
                    rad = rad * m;
                    //Compute the x and z values
                    float x = (float)(radius * Math.Cos(rad));
                    float z = (float)(radius * Math.Sin(rad));
                    Vector3 sweep = new Vector3(x, newPos.y, z);
                    v[found + m].transform.localPosition = sweep;
                }
            }
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
