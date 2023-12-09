using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class AudienceCameraController : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera audienceCamera;
    [SerializeField] float dollyTime = 1.0f;
    [SerializeField] float endDollyTime = 1.5f;
    CinemachineTrackedDolly dolly;
    bool started = false;

    private void Awake() {
        dolly = audienceCamera.GetCinemachineComponent<CinemachineTrackedDolly>();

    }


    public void StartDolly() {
        if (started) { throw new System.Exception("Dolly already started"); }
        started = true;
        StartCoroutine(DollyMove(1));
    }
    IEnumerator DollyMove(float target) {
        float dollyPos = dolly.m_PathPosition;
        while (Mathf.Abs(dollyPos - target) > 0.001f) {
            dolly.m_PathPosition = dollyPos;
            dollyPos = Mathf.MoveTowards(dollyPos, target, Time.deltaTime / dollyTime);


            yield return null;
        }

        yield return new WaitForSeconds(endDollyTime);
        StartCoroutine(DollyMove(target == 0 ? 1 : 0));
    }
}
