using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectiveLook : MonoBehaviour
{
    [SerializeField] DetectiveMovement detectiveMovement;
    [SerializeField] float horizRotationSpeed = 2.0f;
    Vector2 lookDir = Vector2.zero;

    [SerializeField] GameObject[] facialFeatures;
    
    public void DisableFacialFeatures() {
        foreach (GameObject facialFeature in facialFeatures) {
            facialFeature.SetActive(false);
        }
    }

    public void EnableFacialFeatures() {
        foreach (GameObject facialFeature in facialFeatures) {
            facialFeature.SetActive(true);
        }
    }
    private void Update() {
        detectiveMovement.Rotate(new Vector3(0, lookDir.x * horizRotationSpeed, 0));
        lookDir = Vector2.zero; 
    }
    public void OnLook(InputValue inputValue) {
        lookDir = inputValue.Get<Vector2>();
    }
}
