using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    //The selected vertex sphere controller
    public GameObject mSelected;
    public MyMeshNxM mMesh;

    private Color kSelectedColor = Color.red;
    private Color mOrgObjColor = Color.white; // remember obj's original color

    private bool DirectManipulationOn = false;
    private GameObject xAxis, yAxis, zAxis;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mMesh != null);
        
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

    public void VertManipulation(bool status)
    {
        if (status != DirectManipulationOn)
        {
            DirectManipulationOn = status;
            mMesh.SwitchOnManipulation(status);
        }
    }

    private void CreateManipulatorAxes()
    {
        xAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        xAxis.GetComponent<Renderer>().material.color = Color.red;
        //xAxis.transform.localScale = new Vector3(0.75f, 0, 0.75f);
        xAxis.transform.name = "Manipulator";

        yAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        yAxis.GetComponent<Renderer>().material.color = Color.green;
        //yAxis.transform.localScale = new Vector3(0.75f, 0, 0.75f);
        yAxis.transform.name = "Manipulator";

        zAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        zAxis.GetComponent<Renderer>().material.color = Color.blue;
        //zAxis.transform.localScale = new Vector3(0.75f, 0, 0.75f);
        zAxis.transform.name = "Manipulator";
    }

    private void UpdateXAxis()
    {
        //Define the start and end point of the axis beam
        Vector3 startPoint = mSelected.transform.localPosition;
        Vector3 endPoint = mSelected.transform.localPosition + mSelected.transform.right * 2.0f;

        //Find the vector v between end point of axis direction beam
        Vector3 v = endPoint - startPoint;

        //Set the beam upright to align to the correct axis direction and compute the position of the axis
        xAxis.transform.up = mSelected.transform.right;
        xAxis.transform.localPosition = mSelected.transform.localPosition + 0.5f * v;
    }

    private void UpdateYAxis()
    {
        //Define the start and end point of the axis beam
        Vector3 startPoint = mSelected.transform.localPosition;
        Vector3 endPoint = mSelected.transform.localPosition + mSelected.transform.up * 2.0f;

        //Find the vector v between end point of axis direction beam
        Vector3 v = endPoint - startPoint;

        //Set the beam upright to align to the correct axis direction and compute the position of the axis
        yAxis.transform.up = mSelected.transform.up;
        yAxis.transform.localPosition = mSelected.transform.localPosition + 0.5f * v;
    }

    private void UpdateZAxis()
    {
        //Define the start and end point of the axis beam
        Vector3 startPoint = mSelected.transform.localPosition;
        Vector3 endPoint = mSelected.transform.localPosition + mSelected.transform.forward * 2.0f;

        //Find the vector v between end point of axis direction beam
        Vector3 v = endPoint - startPoint;

        //Set the beam upright to align to the correct axis direction and compute the position of the axis
        zAxis.transform.up = mSelected.transform.forward;
        zAxis.transform.localPosition = mSelected.transform.localPosition + 0.5f * v;
    }

    private void DestroyManipulatorAxes()
    {
        Destroy(xAxis);
        Destroy(yAxis);
        Destroy(zAxis);
        xAxis = null;
        yAxis = null;
        zAxis = null;
    }
}
