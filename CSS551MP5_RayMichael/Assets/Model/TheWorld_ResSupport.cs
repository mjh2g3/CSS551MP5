using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TheWorld : MonoBehaviour
{
    public int dropDownIndx = 0; //index 0 == planar, index 1 == cylinder

    //Accessor Method to Set the MeshType from Dropdown
    public void SetMeshType(int indx)
    {
        dropDownIndx = indx;
        if (indx == 0)
        {
            //If planar selected, then set cMesh to inactive
            cMesh.gameObject.SetActive(false);
            mMesh.gameObject.SetActive(true);
        }
        else if (indx == 1)
        {
            cMesh.gameObject.SetActive(true);
            mMesh.gameObject.SetActive(false);
        }
    }
}
