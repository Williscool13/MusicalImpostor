using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFade : MonoBehaviour
{
    [SerializeField] Light _light;
    [SerializeField] float fadeTime = 1f;

    public void FadeLight() {
        StartCoroutine(Fade());
    }

    IEnumerator Fade() {
        float baseIntensity = _light.intensity;

        while (_light.intensity > 0) {
            _light.intensity -= baseIntensity * Time.deltaTime / fadeTime;
            yield return null;
        }

        _light.enabled = false;


    }

}
