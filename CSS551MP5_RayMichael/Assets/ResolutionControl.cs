using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolutionControl : MonoBehaviour
{
    public SliderWithEchoInt N, M;
    public MyMeshNxM mMesh;

    private float prevSliderValuesN = 0;
    private float prevSliderValuesM = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(N != null);
        Debug.Assert(M != null);
        N.SetSliderListener(NValueChanged);
        M.SetSliderListener(MValueChanged);
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

        N.InitSliderRange(2, 20, 2);
        M.InitSliderRange(2, 20, 2);
    }

    void NValueChanged(int v)
    {
        int intV = (int)v;
        List<int> res = ReadMeshRes();
        int n = res[0];
        prevSliderValuesN = (float)n;
        n = intV;
        //n = (int)v;
        res[0] = n;
        UISetMeshResolution(ref res);
    }

    void MValueChanged(int v)
    {
        int intV = (int)v;
        List<int> res = ReadMeshRes();
        int m = res[1];
        prevSliderValuesM = (float)m;
        m = intV;
        //m = (int)v;
        res[1] = m;
        UISetMeshResolution(ref res);
    }

    private List<int> ReadMeshRes()
    {
        List<int> res = mMesh.GetResolution();
        return res;
    }

    private void UISetMeshResolution(ref List<int> r)
    {
        List<int> res = r;
        mMesh.SetResolution(res);
    }

    public void MeshSetUI()
    {
        List<int> res = ReadMeshRes();
        N.SetSliderValue(res[0]);  // do not need to call back for this comes from the object
        M.SetSliderValue(res[1]);
    }
}
