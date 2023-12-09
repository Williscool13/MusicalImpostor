using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectiveInteract : MonoBehaviour
{
    [SerializeField] float interactDistance = 2f;
    [SerializeField] float emphasizeDistance = 2f;
    [SerializeField] LayerMask interactLayerMask;

    [SerializeField] ScriptableVariable_GameObject selectedFruit;

    [SerializeField] ScriptableGameEvent<GameObject> impostorSelected;
    [SerializeField] Animator handAnimator;

    Camera mainCam;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCam = Camera.main;

        interact = false;

        selectedFruit.Value = null;
    }

    void Update()
    {
        Ray centerCamRay = mainCam.ViewportPointToRay(new Vector2(0.5f, 0.5f));

        bool farRayHit = Physics.Raycast(centerCamRay, out RaycastHit farHit, emphasizeDistance, interactLayerMask);

        if (farRayHit) {
            EmphasizeCurrentTarget(farHit);     
            if (interact) {
                InteractCurrentTarget(farHit);
            }
        } else {
            DeemphasizePreviousTarget();
        }

        interact = false;
    }

    private void InteractCurrentTarget(RaycastHit hit) {
        if (hit.collider.gameObject.TryGetComponent(out IFruitComponent fruitComp)) {
            if (fruitComp.FruitParent.TryGetComponent(out IInteractable interactable)) {
                interactable.Interact();
                handAnimator.SetTrigger("Point");
                impostorSelected.Raise(fruitComp.FruitParent);
            }
        }   
    }

    GameObject prevEmphasized;
    FruitController prevFruitController;
    private void EmphasizeCurrentTarget(RaycastHit hit) {
        if (prevEmphasized != null  && (prevEmphasized.GetInstanceID() == hit.collider.gameObject.GetInstanceID())) 
        {
            return; 
        }

        if (prevFruitController != null) { prevFruitController.SetEmphasis(false); }

        
        prevEmphasized = hit.collider.gameObject;
        IFruitComponent fruitComp = hit.collider.gameObject.GetComponent<IFruitComponent>();  
        prevFruitController = fruitComp.FruitParent.GetComponent<FruitController>();
        prevFruitController.SetEmphasis(true);

        selectedFruit.Value = fruitComp.FruitParent;

    }
    private void DeemphasizePreviousTarget() {
        if (prevEmphasized != null) {
            if (prevFruitController != null) { prevFruitController.SetEmphasis(false); }
            prevEmphasized = null;
            prevFruitController = null;
            selectedFruit.Value = null;
        }
    }

    private void OnDrawGizmos() {
        Camera curCam = Camera.main;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(curCam.transform.position, curCam.transform.position + curCam.ViewportPointToRay(new Vector3(0.5f, 0.5f)).direction * interactDistance);
    }


    bool interact;
    bool interactDisabled;
    [ContextMenu("EnableInteract")]
    public void DisableInteract() {
        interactDisabled = true;
    }

    public void OnInteract(InputValue inputValue) {
        if (interactDisabled) { return; }
        if (Cursor.lockState == CursorLockMode.Locked) {
            interact = inputValue.isPressed;
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
