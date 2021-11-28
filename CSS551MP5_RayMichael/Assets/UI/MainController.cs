using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class MainController : MonoBehaviour
{
    public Transform LookAt;
    //public GameObject mSelected;

    public TheWorld mModel;

    private float mousPosX = 0f;
    private float mousPosY = 0f;


    private const float kPixelToDegree = 0.1f;
    private const float kPixelToDistant = 0.05f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(LookAt != null);
        Debug.Assert(mModel != null);
    }

    // Update is called once per frame
    void Update()
    {
        CamManipulation();
        DirectManipulation();
    }

}
