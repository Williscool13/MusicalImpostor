using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class CircleWipeController : MonoBehaviour
{

    [SerializeField] private Material material;
    [SerializeField] private float wipeSpeed = 1.0f;

    float aspectPollTimer = 0.0f;
    float savedAspectRatio;

    [SerializeField] Vector2 offset;
    [SerializeField] float blank = 0.0f;
    [SerializeField] float filled = 1.5f;

    private void Update() {
        aspectPollTimer += Time.deltaTime;
        if (aspectPollTimer > 0.5f) {
            PollScreenAspectRatio();
            aspectPollTimer = 0.0f;
        }
    }

    public float CircleWipeExitScene(Vector2 viewPortPosition) {
        StartCoroutine(Wipe(viewPortPosition, filled, blank));
        return 1 / wipeSpeed * filled;
    }
    public float CircleWipeEnterScene(Vector2 viewPortPosition) {
        StartCoroutine(Wipe(viewPortPosition, blank, filled));
        return 1 / wipeSpeed * filled;
    }

    IEnumerator Wipe(Vector2 viewPortPosition, float start, float end) {
        float t = start;
        material.SetVector("_Offset", viewPortPosition);

        while (Mathf.Abs(t-end) > 0.001f) {
            //t = Mathf.Lerp(t, end, wipeSpeed * Time.deltaTime);
            t = Mathf.MoveTowards(t, end, wipeSpeed * Time.deltaTime);
            //t = Mathf.SmoothStep(t, end, wipeSpeed * Time.deltaTime);
            material.SetFloat("_Radius", t);
            yield return null;
        }
        material.SetFloat("_Radius", end);

    }

    void PollScreenAspectRatio() {
        float currentRatio = Camera.main.aspect;
        if (currentRatio != savedAspectRatio) {
            savedAspectRatio = currentRatio;
            material.SetFloat("_AspectRatio", savedAspectRatio);
        }
    }

}
