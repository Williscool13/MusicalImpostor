using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private float speed = 1f;
    [SerializeField] private float mouseSpeedMultiplier = 1.0f;
    [SerializeField] private float delayBeforeAutoMove = 0.5f;
    [SerializeField] private float autoMoveBuildupTime = 1f;

    CinemachineOrbitalTransposer orbitalTransposer;

    private void Start() {
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private float timeSinceLastInput = float.MaxValue;
    float currSpeed = 0f;
    void Update()
    {
        // if mouse input is bigger than 0, dont auto move camera
        if (rightClicking) {
            if (mouseDelta.magnitude > 0) { 
                // rotate the orbital transposer around the x axis based on the mouse's x delta
                orbitalTransposer.m_XAxis.Value += mouseDelta.x * speed * mouseSpeedMultiplier * Time.deltaTime;
            }
            timeSinceLastInput = 0f;
            currSpeed = 0f;
        }

        timeSinceLastInput += Time.deltaTime;
        if (timeSinceLastInput < delayBeforeAutoMove) {
            return; 
        }

        orbitalTransposer.m_XAxis.Value += Time.deltaTime * currSpeed;

        currSpeed = Mathf.Min(currSpeed + Time.deltaTime / autoMoveBuildupTime, speed);
    }

    bool rightClicking = false;
    Vector2 mouseDelta = Vector2.zero;

    public void OnLook(InputValue inputValue) {
        mouseDelta = inputValue.Get<Vector2>();
    }

    public void OnFocus(InputValue inputValue) { 
        rightClicking = inputValue.Get<float>() > 0;
    }
}
