using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScoreSceneController : MonoBehaviour
{
    [SerializeField] AudienceCameraController dollyController;
    [SerializeField] ScoreInterfaceController scoreInterfaceController;
    [SerializeField] Volume volume;
    [SerializeField] float depthTime = 4.0f;
    float baseWeight;
    void Start() {
        dollyController.StartDolly();
        baseWeight = volume.weight;
    }


    public void OnBlenderFinish() {
        // coroutine - slow enable depth of field 
        StartCoroutine(EnableDepthOfField());
    }

    IEnumerator EnableDepthOfField() {
        float currWeight = baseWeight;
        float maxWeight = 1;
        float diff = maxWeight - baseWeight;
        Debug.Log("Enabling Depth of Field");
        while (currWeight < maxWeight) {
            currWeight += diff * Time.deltaTime / depthTime;
            volume.weight = currWeight;
            yield return null;
        }

        volume.weight = 1;
        //scoreDisplayer.Play();
        scoreInterfaceController.ShowNews();
        //Debug.Log("Playing Score Displayer");
    }

    public void OnGameRestart() {
        StopAllCoroutines();
        StartCoroutine(DisableBlur());
    }

    IEnumerator DisableBlur() {
        
        while (Mathf.Abs(volume.weight - baseWeight) > 0.001f) {
            volume.weight = Mathf.Lerp(volume.weight, baseWeight, Time.deltaTime);
            yield return null;
        }

        // lerp music too?

    }
}
