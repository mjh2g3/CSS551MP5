using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionControl : MonoBehaviour
{
    public TheWorld mModel = null;
    public Dropdown MeshType = null;

    public int curType = 0;

    public SliderWithEchoInt N, M;

    public MyMeshNxM mMesh;
    public CylinderMesh cMesh;


    public SliderWithEchoInt Rotation;

    private float prevSliderValuesN = 0;
    private float prevSliderValuesM = 0;
    private float prevSliderValuesRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        //Dropdown menu
        Debug.Assert(MeshType != null);
        MeshType.onValueChanged.AddListener(UserSelection);
        
        Debug.Assert(mModel != null);
        Debug.Assert(N != null);
        Debug.Assert(M != null);
        N.SetSliderListener(NValueChanged);
        M.SetSliderListener(MValueChanged);

        //Cylinder mesh rotation code
        Debug.Assert(Rotation != null);
        Rotation.SetSliderListener(RotationValueChanged);

        InitSliders();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void InitSliders()
    {
        List<int> res = ReadMeshRes();
        prevSliderValuesN = res[0];
        prevSliderValuesM = res[1];

        if (curType == 0)
        {
            N.InitSliderRange(2, 20, (int)prevSliderValuesN);
            M.InitSliderRange(2, 20, (int)prevSliderValuesM);
        }
        else if (curType == 1)
        {
            if (prevSliderValuesN < 4.0f)
            {
                N.InitSliderRange(4, 20, 4);
            }
            else
            {
                N.InitSliderRange(4, 20, (int)prevSliderValuesN);
            }
            if (prevSliderValuesM < 4.0f)
            {
                M.InitSliderRange(4, 20, 4);
            }
            else
            {
                M.InitSliderRange(4, 20, (int)prevSliderValuesM);
            }
            //Cylinder Rotation initialization
            double rot = ReadMeshRotation();
            prevSliderValuesRotation = (float)rot;
            Rotation.InitSliderRange(10, 360, (int)prevSliderValuesRotation);

        }
    }

    void NValueChanged(int v)
    {
        if (curType == 0)
        {
            Debug.Log("Init of the res control N PLANE");
            int intV = (int)v;
            List<int> res = ReadMeshRes();
            int n = res[0];
            prevSliderValuesN = (float)n;
            n = intV;
            res[0] = n;
            UISetMeshResolution(ref res);
            mModel.DestroyManipulatorAxes();
        }
        else if (curType == 1)
        {
            Debug.Log("Init of the res control N CYLINDER");
            int intV = (int)v;
            List<int> res = ReadMeshRes();
            int n = res[0];
            prevSliderValuesN = (float)n;
            n = intV;
            res[0] = n;
            UISetMeshResolution(ref res);
            mModel.DestroyManipulatorAxes();
        }
    }

    void MValueChanged(int v)
    {
        Debug.Log("Init of the res control M PLANE");
        int intV = (int)v;
        List<int> res = ReadMeshRes();
        int m = res[1];
        prevSliderValuesM = (float)m;
        m = intV;
        res[1] = m;
        UISetMeshResolution(ref res);
        mModel.DestroyManipulatorAxes();
    }

    //Cylinder rotation changed call method
    void RotationValueChanged(int v)
    {
        Debug.Log("Init of the res control rotation");
        int intV = (int)v;
        double rotation = ReadMeshRotation();
        int r = (int)rotation;
        prevSliderValuesRotation = (float)r;
        r = intV;
        rotation = r;
        UISetMeshRotation(ref rotation);
        mModel.DestroyManipulatorAxes();
    }

    private double ReadMeshRotation()
    {
        double rotation = cMesh.GetRotation();
        return rotation;
    }

    private List<int> ReadMeshRes()
    {
        List<int> res = new List<int>();
        if (curType == 0)
        {
            res = mMesh.GetResolution();
        }
        else if (curType == 1)
        {
            res = cMesh.GetResolution();
        }

        return res;
    }

    private void UISetMeshResolution(ref List<int> r)
    {
        List<int> res = r;
        if (curType == 0)
        {
            mMesh.SetResolution(res);
        }
        else if (curType == 1)
        {
            cMesh.SetResolution(res);
        }
        
    }

    //Cylinder code
    private void UISetMeshRotation(ref double rotation)
    {
        cMesh.SetRotation(rotation);
    }

    public void MeshSetUI()
    {
        List<int> res = ReadMeshRes();
        N.SetSliderValue(res[0]);  // do not need to call back for this comes from the object
        M.SetSliderValue(res[1]);

        
        double rotation = ReadMeshRotation();
        Rotation.SetSliderValue((int)rotation);
    }

    

    void UserSelection(int index)
    {
        if (index == 0)
        {
            Debug.Log("index is 0");
            mModel.SetMeshType(index);
            curType = index;
            InitSliders();
        }
        else
        {
            Debug.Log("index is 1");
            mModel.SetMeshType(index);
            curType = index;
            InitSliders();
        }
    }
}
