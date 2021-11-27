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

        }
        //Step2: If LeftControl Key is Down AND user selected with LMB
        if (Input.GetKey(KeyCode.LeftControl) && (Input.GetMouseButtonDown(0)))
        {
            //if sphere selected, set mSelected
            mousPosX = Input.mousePosition.x;
            mousPosY = Input.mousePosition.y;
        }

        //Step3: Obtain the nextPosition for the mouse position at mouse button hold + Alt-Left
        else if (Input.GetKey(KeyCode.LeftAlt) && (Input.GetMouseButton(0)))
        {
            //Step4: Compute the delta position change for the mouse using the initial and nextPosition
            float dx = mousPosX - Input.mousePosition.x;
            float dy = mousPosY - Input.mousePosition.y;

            mousPosX = Input.mousePosition.x;
            mousPosY = Input.mousePosition.y;

            if (Input.GetMouseButton(0)) // Camera Rotation
            {
                //Move the sphere position incrementally
            }
            
        }
    }
}
