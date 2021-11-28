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

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(mMesh != null);    
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

    public void VertManipulation(bool status)
    {
        if (status != DirectManipulationOn)
        {
            DirectManipulationOn = status;
            mMesh.SwitchOnManipulation(status);
        }
    }

}
