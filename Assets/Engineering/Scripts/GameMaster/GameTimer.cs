using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class GameTimer : MonoBehaviour
{
    [SerializeField] ScriptableGameEvent<object> timeOut;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] AudioSource beepSource;
    [SerializeField] ScriptableVariable_Float timeleft;

    [SerializeField] bool infiniteTime = false;

    IEnumerator countdownCoroutine;
    IEnumerator scaleShiftCoroutine;
    float scaleShiftSpeed = 0.05f;
    float _t = 21.0f;

    public void StartTimer() {
        if (infiniteTime) { return; }

        countdownCoroutine = Countdown();
        StartCoroutine(countdownCoroutine);
    }



    public void StopTimer() {
        StopCoroutine(countdownCoroutine);
        timeleft.Value = _t;
    }

    IEnumerator ScaleShift(string nextText) {
        float currScale = 1.0f;
        while (currScale > 0) {
            currScale -= Time.deltaTime / scaleShiftSpeed;
            timerText.transform.localScale = new Vector3(currScale, currScale, currScale);
            yield return null;
        }
        timerText.transform.localScale = new Vector3(0, 0, 0);
        timerText.text = nextText;

        while (currScale < 1) {
            currScale += Time.deltaTime / scaleShiftSpeed;
            timerText.transform.localScale = new Vector3(currScale, currScale, currScale);
            yield return null;
        }
        timerText.transform.localScale = new Vector3(1, 1, 1);
    }
    IEnumerator Countdown() {
        float nextThreshold = 20.0f;

        scaleShiftCoroutine = ScaleShift(nextThreshold.ToString("F0"));
        StartCoroutine(scaleShiftCoroutine);
        while (_t > 0) {
            _t -= Time.deltaTime;

            if (_t <= nextThreshold) {
                nextThreshold--;
             
                StopCoroutine(scaleShiftCoroutine);
                scaleShiftCoroutine = ScaleShift(nextThreshold.ToString("F0"));
                StartCoroutine(scaleShiftCoroutine);
                PlayCountdownSound((int)nextThreshold);
            }
            yield return null;
        }
    }
    [SerializeField] TextMeshProUGUI timesUp;
    [SerializeField] float timeUpTextGrowSpeed = 0.1f;
    IEnumerator TimeUpTextGrow() {
        float currScale = 0.0f;
        while (currScale < 1) {
            currScale += Time.deltaTime / timeUpTextGrowSpeed;
            timesUp.transform.localScale = new Vector3(currScale, currScale, currScale);
            yield return null;
        }
        timesUp.transform.localScale = new Vector3(1, 1, 1);
    }

    void PlayCountdownSound(int value) {
        switch (value) {
            case 10:
                beepSource.volume = 0.2f;
                beepSource.Play();
                break;
            case 5:
                beepSource.volume = 0.5f;
                beepSource.Play();
                break;
            case 4:
                beepSource.volume = 0.6f;
                beepSource.Play();
                break;
            case 3:
                beepSource.volume = 0.7f;
                beepSource.Play();
                break;
            case 2:
                beepSource.volume = 0.8f;
                beepSource.Play();
                break;
            case 1:
                beepSource.volume = 1f;
                beepSource.Play();
                break;
            case 0:
                StartCoroutine(TimeUpTextGrow());
                // grow times up text
                timeOut.Raise(null);
                break;
            default:
                // play no sound
                break;
        }
    }
}
