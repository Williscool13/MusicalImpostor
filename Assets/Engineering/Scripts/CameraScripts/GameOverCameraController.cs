using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera vcam;
    [SerializeField] private float speed = 0.05f;
    CinemachineOrbitalTransposer orbitalTransposer;
    void Start()
    {
        orbitalTransposer = vcam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    [ContextMenu("GameOver")]
    public void GameOverCameraPan() {
        StartCoroutine(CameraPan());
    }

    IEnumerator CameraPan() {
        orbitalTransposer.m_XAxis.Value = -30f;
    
        WaitForSeconds wait = new WaitForSeconds(Time.deltaTime);
        while (orbitalTransposer.m_XAxis.Value < 30) {
            orbitalTransposer.m_XAxis.Value += speed * Time.deltaTime;
            yield return wait;
        }

        orbitalTransposer.m_XAxis.Value = 30f;
    }
}
