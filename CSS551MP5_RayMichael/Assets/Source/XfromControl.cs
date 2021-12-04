using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XfromControl : MonoBehaviour {
    public Toggle T, R, S;
    public SliderWithEcho X, Y, Z;
    public TexturePlacement texture;
    private Vector3 mPreviousSliderValues = Vector3.zero;

	// Use this for initialization
	void Start () {
        T.onValueChanged.AddListener(SetToTranslation);
        R.onValueChanged.AddListener(SetToRotation);
        S.onValueChanged.AddListener(SetToScaling);
        X.SetSliderListener(XValueChanged);
        Y.SetSliderListener(YValueChanged);
        Z.SetSliderListener(ZValueChanged);

        T.isOn = true;
        R.isOn = false;
        S.isOn = false;
        SetToTranslation(true);
	}
	
    //---------------------------------------------------------------------------------
    // Initialize slider bars to specific function
    void SetToTranslation(bool v)
    {
        Vector3 p = ReadObjectXfrom();
        mPreviousSliderValues = p;
        X.InitSliderRange(-4, 4, p.x);
        Y.InitSliderRange(-4, 4, p.y);
        Z.InitSliderRange(1, 1, 1f);
    }

    void SetToScaling(bool v)
    {
        Vector3 s = ReadObjectXfrom();
        mPreviousSliderValues = s;
        X.InitSliderRange(0.1f, 10, s.x);
        Y.InitSliderRange(0.1f, 10, s.y);
        Z.InitSliderRange(1, 1, s.z);
    }

    void SetToRotation(bool v)
    {
        Vector3 r = ReadObjectXfrom();
        mPreviousSliderValues = r;
        X.InitSliderRange(0, 0, r.x);
        Y.InitSliderRange(0, 0, r.y);
        Z.InitSliderRange(-180, 180, r.z);
    }
    //---------------------------------------------------------------------------------
    // resopond to sldier bar value changes
    void XValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        // if not in rotation, next two lines of work would be wasted
        float dx = v - mPreviousSliderValues.x;
        mPreviousSliderValues.x = v;
        Quaternion q = Quaternion.AngleAxis(dx, Vector3.right);
        p.x = v;
        UISetObjectXform(ref p, ref q);
    }
    
    void YValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        // if not in rotation, next two lines of work would be wasted
        float dy = v - mPreviousSliderValues.y;
        mPreviousSliderValues.y = v;
        Quaternion q = Quaternion.AngleAxis(dy, Vector3.up);
        p.y = v;
        UISetObjectXform(ref p, ref q);
    }

    void ZValueChanged(float v)
    {
        Vector3 p = ReadObjectXfrom();
        // if not in rotation, next two lines of work would be wasterd
        float dz = v - mPreviousSliderValues.z;
        mPreviousSliderValues.z = v;
        Quaternion q = Quaternion.AngleAxis(dz, Vector3.forward);
        p.z = v;
        UISetObjectXform(ref p, ref q);
    }
    //---------------------------------------------------------------------------------

    public void ObjectSetUI()
    {
        Vector3 p = ReadObjectXfrom();
        X.SetSliderValue(p.x);  // do not need to call back for this comes from the object
        Y.SetSliderValue(p.y);
        Z.SetSliderValue(p.z);
    }

    private Vector3 ReadObjectXfrom()
    {
        Vector3 p;
        
        if (T.isOn)
        {
            p = texture.Offset;
        }
        else if (S.isOn)
        {
            p = texture.Scale;
        }
        else
        {
            p = new Vector3(0, 0, texture.Rotation);
        }
        return p;
    }

    private void UISetObjectXform(ref Vector3 p, ref Quaternion q)
    {
        if (T.isOn)
        {
            texture.Offset = p;
        }
        else if (S.isOn)
        {
            texture.Scale = p;
        } else
        {
            texture.Rotation = p.z;
        }
    }
}