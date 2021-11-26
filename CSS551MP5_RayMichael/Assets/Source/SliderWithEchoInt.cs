using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithEchoInt : MonoBehaviour
{
    public Slider TheSlider = null;
    public Text TheEcho = null;
    public Text TheLabel = null;

    public delegate void SliderCallbackDelegate(int v);      // defined a new data type
    private SliderCallbackDelegate mCallBack = null;           // private instance of the data type
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(TheSlider != null);
        Debug.Assert(TheEcho != null);
        Debug.Assert(TheLabel != null);
        TheSlider.wholeNumbers = true;

        TheSlider.onValueChanged.AddListener(SliderValueChange);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSliderListener(SliderCallbackDelegate listener)
    {
        mCallBack = listener;
    }

    // GUI element changes the object
    void SliderValueChange(float v)
    {
        int intV = (int)v;
        TheEcho.text = intV.ToString();
        // Debug.Log("SliderValueChange: " + v);
        if (mCallBack != null)
            mCallBack(intV);
    }

    public float GetSliderValue()
    {
        return TheSlider.value;
    }
    public void SetSliderLabel(string l)
    {
        TheLabel.text = l;
    }
    public void SetSliderValue(int v)
    {
        TheSlider.value = v;
        SliderValueChange(v);
    }
    public void InitSliderRange(int min, int max, int v)
    {
        TheSlider.minValue = min;
        TheSlider.maxValue = max;
        SetSliderValue(v);
    }
}
