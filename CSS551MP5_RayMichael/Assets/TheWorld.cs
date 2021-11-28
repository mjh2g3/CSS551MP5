using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheWorld : MonoBehaviour
{
    //The selected vertex sphere controller
    public GameObject mSelected;
    private Color kSelectedColor = Color.red;
    private Color mOrgObjColor = Color.white; // remember obj's original color

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelected(GameObject g)
    {
        if (mSelected != null)
            mSelected.GetComponent<Renderer>().material.color = mOrgObjColor;

        mSelected = g;
        if (mSelected != null)
        {
            mOrgObjColor = g.GetComponent<Renderer>().material.color; // save a copy
            mSelected.GetComponent<Renderer>().material.color = kSelectedColor;
        }
    }
}
