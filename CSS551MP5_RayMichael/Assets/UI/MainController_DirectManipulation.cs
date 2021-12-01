using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    public void DirectManipulation()
    {
        //Step 1: If LeftControl Key is Down, compute the controller spheres and normal vectors on mesh
        if (Input.GetKey(KeyCode.LeftControl))
        {
            //Switch On the vertex manipulation
            
            if (!ManipulatorAxesOn()) {
                mModel.VertManipulation(true);
            }

            //Step2: If LeftControl Key is Down AND user selected with LMB
            if ((Input.GetMouseButtonDown(0)))
            {
                //if sphere selected, set mSelected
                mousPosX = Input.mousePosition.x;
                mousPosY = Input.mousePosition.y;

                RaycastHit hitInfo = new RaycastHit();
                bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity);
                if (hit)
                {
                    if (hitInfo.transform.gameObject.name == "Sphere")
                    {
                        SetSelectedObj(hitInfo.transform.gameObject);
                    }
                    else if (hitInfo.transform.gameObject.name.Contains("Manipulator"))
                    {
                        Debug.Log(hitInfo.transform.gameObject.name);
                        //Need to set manipulator active and turn yellow
                        SelectAxis(hitInfo.transform.gameObject);
                    }
                } else {
                    ResetSelectedObj();
                    DestroyAxes();
                }
            }

            //Step3: Obtain the nextPosition for the mouse position at mouse button hold + LeftControl
            else if (Input.GetMouseButton(0))
            {
                //Step4: Compute the delta position change for the mouse using the initial and nextPosition
                float dx = mousPosX - Input.mousePosition.x;
                float dy = mousPosY - Input.mousePosition.y;

                mousPosX = Input.mousePosition.x;
                mousPosY = Input.mousePosition.y;

                if (Input.GetMouseButton(0) && draggingAxis) // 
                {
                    string axis = GetSelectedAxis();
                    if (axis == "X")
                        mSelected.localPosition += new Vector3(-dx * dragSpeed, 0, 0);
                    else if (axis == "Y")
                        mSelected.localPosition += new Vector3(0, -dy * dragSpeed, 0);
                    else 
                        mSelected.localPosition += new Vector3(0, 0, -dy * dragSpeed);
                }
            }

            else if (Input.GetMouseButtonUp(0))
            {
                if (draggingAxis) 
                {
                    DeSelectAxis();
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && !ManipulatorAxesOn())
        {
            mModel.VertManipulation(false);
        }
    }

    // Set Selected 
    private void SetSelectedObj(GameObject s)
    {
        mModel.SetSelected(s);
        mSelected = s.transform;
    }

    private void ResetSelectedObj() {
        mModel.ResetSelected();
    }

    private void SelectAxis(GameObject axis) {
        draggingAxis = true;
        mModel.SelectAxis(axis);
    }

    private void DeSelectAxis() {
        draggingAxis = false;
        mModel.DeSelectAxis();
    }

    private string GetSelectedAxis() {
        return mModel.GetSelectedAxis();
    }

    private void DestroyAxes() {
        mModel.DestroyManipulatorAxes();
    }

    private bool ManipulatorAxesOn() {
        return mModel.ManipulatorAxesOn();
    }
}
