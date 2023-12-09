using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AddCameraToStack : MonoBehaviour
{
    [SerializeField] Camera thisCam;
    void Start()
    {
        Debug.Assert(thisCam.GetUniversalAdditionalCameraData().renderType == CameraRenderType.Overlay, "You should only add overlay types to the stack");
     
        Camera mainCam = Camera.main;
        UniversalAdditionalCameraData camData = mainCam.GetUniversalAdditionalCameraData();

        camData.cameraStack.Add(thisCam);
    }
}
