using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR.Haptics;

public class CountdownController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI countdownText;
    [SerializeField] AudioSource countdownSource;
    [SerializeField] AudioSource startSource;
    [SerializeField] float sizeChangeTime = 0.1f;
    [SerializeField] float sizeHoldTime = 0.8f;

    private void Start() {
        countdownText.text = "";
        countdownText.transform.localScale = new Vector3(0, 0, 0);
    }

    [ContextMenu("StartCountdown")]
    public void StartCountdown() {
        StartCoroutine(Countdown());
    }
    IEnumerator Countdown() {
        int currentTime = 3;
        float currSize = 0;
        while (currentTime > 0) {
            countdownText.text = currentTime.ToString();
            while (currSize < 1) {
                currSize += 1 / sizeChangeTime * Time.deltaTime;
                countdownText.transform.localScale = new Vector3(currSize, currSize, currSize);
                yield return null;
            }

            countdownText.transform.localScale = new Vector3(1, 1, 1);
            currSize = 1;
            
            countdownSource.Play();
            yield return new WaitForSeconds(sizeHoldTime);

            while (currSize > 0) {
                currSize -= 1 / sizeChangeTime * Time.deltaTime;
                countdownText.transform.localScale = new Vector3(currSize, currSize, currSize);
                yield return null;
            }
            countdownText.transform.localScale = new Vector3(0, 0, 0);
            currSize = 0;

            currentTime--;
        }

        countdownText.text = "GO!";

        while (currSize < 1) {
            currSize += 1 / sizeChangeTime * Time.deltaTime;
            countdownText.transform.localScale = new Vector3(currSize, currSize, currSize);

            yield return null;
        }
        countdownText.transform.localScale = new Vector3(1, 1, 1);
        startSource.Play();
        yield return new WaitForSeconds(sizeHoldTime);
        currSize = 1;
        while (currSize > 0) {
            currSize -= 1 / sizeChangeTime * Time.deltaTime;

            countdownText.transform.localScale = new Vector3(currSize, currSize, currSize);

            yield return null;

        }
        countdownText.text = "";
        countdownText.transform.localScale = new Vector3(0, 0, 0);
    }
}
