using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCameraRotation : MonoBehaviour
{
    Camera mainCam; 
    void Start() { mainCam = Camera.main; }
    void Update() { transform.rotation = mainCam.transform.rotation; }
}
