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
            mModel.VertManipulation(true);
        }
        //Step2: If LeftControl Key is Down AND user selected with LMB
        else if (Input.GetKey(KeyCode.LeftControl) && (Input.GetMouseButtonDown(0)))
        {
            //Debug.Log("Holding Down the Left Control AND MouseButton is clicked");
            //if sphere selected, set mSelected
            mousPosX = Input.mousePosition.x;
            mousPosY = Input.mousePosition.y;

            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, Mathf.Infinity);
            if (hit)
            {
                if (hitInfo.transform.gameObject.name == "Sphere")
                {
                    Debug.Log(hitInfo.transform.gameObject.name);
                    SetSelectedController(hitInfo.transform.gameObject);
                }
            }

        }

        //Step3: Obtain the nextPosition for the mouse position at mouse button hold + Alt-Left
        else if (Input.GetKey(KeyCode.LeftControl) && (Input.GetMouseButton(0)))
        {
            //Debug.Log("Holding Down the Left Control AND MouseButton is held down");
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
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            mModel.VertManipulation(false);

        }
    }

    private void SetSelectedController(GameObject s)
    {
        mModel.SetSelected(s);
    }
}