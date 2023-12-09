using UnityEngine;
using UnityEngine.InputSystem;

public class DetectiveMovement : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float maxSpeed = 12f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] private float lerpUp = 1.0f;
    [SerializeField] private float lerpDown = 1.0f;

    [SerializeField] PlayerInput playerInput;
    private float currSpeed = 0f;
    Camera mainCamera;
    private void Start() {
        // hide mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = Camera.main;

        prevMoveDir = Vector3.zero;
    }

    float verticalVelocity = 0f;
    Vector3 prevMoveDir;


    bool wasGrounded = false;
    void Update()
    {
        Vector3 currMoveDir = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 movedir;
        if (currMoveDir.magnitude > 0) {
            currSpeed = Mathf.Lerp(currSpeed, maxSpeed, lerpUp * Time.deltaTime);
            movedir = new(currMoveDir.x, 0, currMoveDir.y);
            prevMoveDir = movedir;
        } else {
            currSpeed = Mathf.Lerp(currSpeed, 0, lerpDown * Time.deltaTime);
            movedir = prevMoveDir;
        }
        Vector3 forwardAlignedMoveDir = (mainCamera.transform.rotation * movedir).normalized;


        if (controller.isGrounded) {
            verticalVelocity = gravity;
            // if jump button is pressed, gravity = jump force
        } else {
            if (wasGrounded) {
                verticalVelocity = 0;
            }
        }


        controller.Move(currSpeed * Time.deltaTime * (new Vector3(forwardAlignedMoveDir.x, verticalVelocity, forwardAlignedMoveDir.z)));

        verticalVelocity += gravity * Time.deltaTime;
        wasGrounded = controller.isGrounded;
    }

    public void Rotate(Vector3 eulerRotation) {
        controller.transform.Rotate(eulerRotation * Time.deltaTime);
    }

}
