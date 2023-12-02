using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmphasisLightController : MonoBehaviour
{
    [SerializeField] ScriptableVariable_GameObject selectedFruit;
    [SerializeField] Light thisLight;
    [SerializeField] float lightIntensitySpeed = 0.03f;
    [SerializeField] float lightMoveSpeed = 2.0f;
    Transform previousFruit;

    const float targetIntensity = 15.0f;

    bool gameStarted = false;


    void Update()
    {
        if (!gameStarted) { return; }
        UpdateCurrentTarget();
        AdjustIntensity();
        if (previousFruit != null) {
            AdjustPosition();
        }
    }
    void UpdateCurrentTarget() {
        if (selectedFruit.Value == previousFruit) { return; }

        if (selectedFruit.Value == null) {
            previousFruit = null;
        } else {
            previousFruit = selectedFruit.Value.transform;
        }
        
    }

    void AdjustIntensity() {
        float _tarInt = previousFruit == null ? 0.0f : targetIntensity;
        if (Mathf.Abs(thisLight.intensity - _tarInt) < 0.05f) { return; }
        thisLight.intensity = Mathf.Lerp(thisLight.intensity, _tarInt, Time.deltaTime * lightIntensitySpeed);
    }
    void AdjustPosition() {
        if (Vector3.Distance(transform.position, previousFruit.position) < 0.05f) { return; }
        transform.position = Vector3.Lerp(transform.position, previousFruit.position, Time.deltaTime * lightMoveSpeed);
    }


    public void StartGame() {
        gameStarted = true;
    }
}
