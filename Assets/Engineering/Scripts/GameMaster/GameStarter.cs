using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Playables;

public class GameStarter : MonoBehaviour
{
    [SerializeField] PlayableDirector playableDirector;

    [SerializeField] AudioSource tuningSource;
    public void OnInteract(InputValue inputValue) {
        if (inputValue.isPressed) { StartGame(); }
    }

    void StartGame() {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playableDirector.Play();
    }

    public void FadeOutMainTuning() {
        StartCoroutine(OrchestraTuningFadeOut(2.0f, 0.0f));
    }


    IEnumerator OrchestraTuningFadeOut(float fadeTime, float targetVolume) {
        float volMult = tuningSource.volume - targetVolume;
        Debug.Assert(volMult > 0, "Difference is smaller or less than 0, will never reach end");

        while (tuningSource.volume > targetVolume) {
            tuningSource.volume -= volMult * Time.deltaTime / fadeTime;
            yield return null;
        }
        tuningSource.volume = targetVolume;
        tuningSource.Stop();
    }
}
