using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    //The selected vertex sphere controller
    public GameObject mSelected;
    public MyMeshNxM mMesh;

    private Color kSelectedColor = Color.red; // highlight color for selected obj
    private Color mOrgObjColor = Color.white; // remember obj's original color

    private GameObject selectedAxis;
    private Color selectedAxisColor = Color.yellow; // highlight color for selected axis manipulator
    private Color selectedAxisOrgColor; // original color for selected axis manipulator

    private bool DirectManipulationOn = false;
    private GameObject axisGroup, xAxis, yAxis, zAxis;

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

    public void ResetSelected() {
        if (mSelected != null)
            mSelected.GetComponent<Renderer>().material.color = mOrgObjColor;
    }

    public void SelectAxis(GameObject axis)
    {
        if (selectedAxis != null) {
            selectedAxis.GetComponent<Renderer>().material.color = selectedAxisOrgColor;
        }

        selectedAxis = axis;
        selectedAxisOrgColor = selectedAxis.GetComponent<Renderer>().material.color;
        selectedAxis.GetComponent<Renderer>().material.color = selectedAxisColor;
    }

    public void DeSelectAxis() {
        selectedAxis.GetComponent<Renderer>().material.color = selectedAxisOrgColor;
    }

    public string GetSelectedAxis() {
        if (selectedAxis.name.Contains("X"))
            return "X";
        else if (selectedAxis.name.Contains("Y"))
            return "Y";
        else
            return "Z";
    }

    public void VertManipulation(bool status)
    {
        if (status != DirectManipulationOn)
        {
            DirectManipulationOn = status;
            mMesh.SwitchOnManipulation(status);
        } if (status == false) {
            DestroyManipulatorAxes();
        }
    }

    private void CreateManipulatorAxes()
    {
        axisGroup = new GameObject("Manipulator Group");
        
        xAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        xAxis.GetComponent<Renderer>().material.color = Color.red;
        xAxis.transform.localScale = new Vector3(0.25f, 1, 0.25f);
        xAxis.transform.name = "X Manipulator";
        xAxis.transform.parent = axisGroup.transform;
        

        yAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        yAxis.GetComponent<Renderer>().material.color = Color.green;
        yAxis.transform.localScale = new Vector3(0.25f, 1, 0.25f);
        yAxis.transform.name = "Y Manipulator";
        yAxis.transform.parent = axisGroup.transform;

        zAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        zAxis.GetComponent<Renderer>().material.color = Color.blue;
        zAxis.transform.localScale = new Vector3(0.25f, 1, 0.25f);
        zAxis.transform.name = "Z Manipulator";
        zAxis.transform.parent = axisGroup.transform;
    }

    public bool ManipulatorAxesOn() {
        if (axisGroup != null) {
            return true;
        } else return false;
    }

    private void UpdateXAxis()
    {
        //Define the start and end point of the axis beam
        Vector3 startPoint = mSelected.transform.localPosition;
        Vector3 endPoint = mSelected.transform.localPosition + mSelected.transform.right * xAxis.transform.localScale.y * 2;


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
        Vector3 endPoint = mSelected.transform.localPosition + mSelected.transform.up * yAxis.transform.localScale.y * 2;

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
        Vector3 endPoint = mSelected.transform.localPosition + mSelected.transform.forward * zAxis.transform.localScale.y * 2;

        //Find the vector v between end point of axis direction beam
        Vector3 v = endPoint - startPoint;

        //Set the beam upright to align to the correct axis direction and compute the position of the axis
        zAxis.transform.up = mSelected.transform.forward;
        zAxis.transform.localPosition = mSelected.transform.localPosition + 0.5f * v;
    }

    public void DestroyManipulatorAxes()
    {
        Destroy(axisGroup);
    }


}
