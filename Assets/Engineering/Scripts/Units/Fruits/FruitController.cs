using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

public class FruitController : MonoBehaviour, IInteractable, IMusician, IImpostor, IFruit
{
    [SerializeField] AudioSource source;
    AudioClip mainClip;
    [SerializeField] float normalVolume = 0.33f;
    [SerializeField] float emphasizedVolume = 1.0f;

    bool emphasized = false;
    public Instrument FruitInstrument { get; private set; }
    public bool Impostor { get; private set; } = false;

    public FruitType FruitType { get; private set; }

    public void Initialize(AudioClip tuningTrack, AudioClip actualTrack, Instrument fruitInstrument, FruitType fruitType) {
        source.volume = 0;
        mainClip = actualTrack;
        this.FruitInstrument = fruitInstrument;
        this.FruitType = fruitType;

        switch (this.FruitType) {
            case FruitType.Watermelon:
                // instantiate prefab of watermelon model
                break;
        }

        // choose and generate one of four random watermelon models
        int val = UnityEngine.Random.Range(0, fruitModels.Length);
        GameObject fruitModel = Instantiate(fruitModels[val], fruitModelParent.transform);
        fruitModel.GetComponent<IFruitComponent>().FruitParent = gameObject;
        fruitAnimator = fruitModel.GetComponent<Animator>();


        GameObject instrumentModel = null;

        switch (this.FruitInstrument) {
            case Instrument.Bass:
                instrumentModel = Instantiate(instrumentModels[1], instrumentModelParent.transform);
                break;
            case Instrument.Flute:
                instrumentModel = Instantiate(instrumentModels[3], instrumentModelParent.transform);
                // instantiate prefab of flute model
                break;
            case Instrument.Tuba:
                instrumentModel = Instantiate(instrumentModels[0], instrumentModelParent.transform);
                break;
            case Instrument.Violin:
                normalVolume *= 0.5f;
                emphasizedVolume *= 0.5f;
                instrumentModel = Instantiate(instrumentModels[2], instrumentModelParent.transform);
                // instantiate prefab of violin model
                break;
        }


        //instrumentModel.transform.localPosition = instrumentModel.transform.position;
        instrumentModel.GetComponent<IFruitComponent>().FruitParent = gameObject;
        instrumentAnimator = instrumentModel.GetComponent<Animator>();






    }

    [SerializeField] GameObject[] fruitModels;
    [SerializeField] GameObject[] instrumentModels;
    [SerializeField] GameObject fruitModelParent;
    [SerializeField] GameObject instrumentModelParent;

    Animator fruitAnimator;
    Animator instrumentAnimator;

    enum  VolumeState
    {
        normal,
        emphasized
    }

    [SerializeField] float volumeMoverate = 3.0f;
    VolumeState volumeState = VolumeState.normal;

    private void Update() {
        if (volumeFade != null) { return; }
        source.volume = Mathf.MoveTowards(source.volume, volumeState == VolumeState.normal ? normalVolume : emphasizedVolume, volumeMoverate * Time.deltaTime);
    }

    public void TurnImpostor(AudioClip track) {
        this.Impostor = true;
        normalVolume = Mathf.Max(normalVolume, emphasizedVolume * 0.75f);
        source.rolloffMode = AudioRolloffMode.Linear;
        mainClip = track;
    }

    public void Interact() {
        if (Impostor) {
            //transform.position = transform.position + new Vector3(0, 0.5f, 0);
        }
        else {
            Debug.Log("You selected the wrong fruit");
        }
    }

    internal void SetEmphasis(bool status) {
        if (status) {
            //_fruitModelMaterial.SetInt("_OutlineActive", 1);
            //source.volume = emphasizedVolume;
            volumeState = VolumeState.emphasized;
            source.spatialBlend = 0.0f;
            emphasized = true;
        } else {
            //_fruitModelMaterial.SetInt("_OutlineActive", 0);
            //source.volume = normalVolume;
            volumeState = VolumeState.normal;
            source.spatialBlend = 1.0f;
            emphasized = false;
        }
    }

    #region IMusician
    IEnumerator volumeFade;
    public void FadeOutMusic() {
        if (volumeFade != null) { StopCoroutine(volumeFade); }
        //volumeFade = FadeOutMusic(1.0f, 0.0f);
        volumeFade = FadeMusic(source.clip, 1.0f, source.volume, 0f);
        StartCoroutine(volumeFade);

        // animator transition to idle
        fruitAnimator.SetTrigger("StopPlaying");
        instrumentAnimator.SetTrigger("StopPlaying");
    }
    public void FadeInMusic(AudioClip track) {
        if (volumeFade != null) { StopCoroutine(volumeFade); }
        //volumeFade = FadeInMusic(track, 1.0f, emphasized ? 1.0f : 0.33f);
        volumeFade = FadeMusic(track, 1.0f, source.volume, emphasized ? emphasizedVolume : normalVolume);
        StartCoroutine(volumeFade);

        // animator transition to playing
        fruitAnimator.SetTrigger("StartPlaying");
        instrumentAnimator.SetTrigger("StartPlaying");
    }

    IEnumerator FadeMusic(AudioClip track, float time, float sourceVol, float targetVol) {
        float diff = Mathf.Abs(sourceVol - targetVol);
        //if (diff <= 0) { yield break; }
        source.volume = sourceVol;

        if (source.clip != track) { source.clip = track; }
        if (source.isPlaying == false) { source.Play(); }

        while (!Mathf.Approximately(source.volume, targetVol)) {
            source.volume = Mathf.MoveTowards(source.volume, targetVol, diff * Time.deltaTime / time);
            yield return null;
            Debug.Log("Approaching " + targetVol);
        }

        source.volume = targetVol;

        if (source.volume <= 0) { source.Stop(); }

        volumeFade = null;
    }

    #endregion 
    public void FadeInMainMusic() {
        FadeInMusic(mainClip);
    }

    
    /// <summary>
    /// Called by scriptable event listener
    /// </summary>
    /// <param name="gob"></param>
    public void OnImpostorSelected(GameObject gob) {
        StopPlaying();
        if (gob.GetInstanceID() == this.GetInstanceID()) {
            // you have selected the impostor
        }
    }

    public void OnTimeOut() {
        //StopPlaying();
    }

    void StopPlaying() {
        source.Stop();
        fruitAnimator.SetTrigger("StopPlaying");
        instrumentAnimator.SetTrigger("StopPlaying");
    }
}

public interface IInteractable
{
    void Interact();
}

public interface IFruit
{
    public FruitType FruitType { get; }
}


public interface IMusician
{
    public Instrument FruitInstrument { get; }

    void Initialize(AudioClip tuningTrack, AudioClip actualTrack, Instrument fruitInstrument, FruitType fruitType);
}

public interface IImpostor
{
    public bool Impostor { get; }
    public void TurnImpostor(AudioClip track);

}

public enum Instrument {
    Tuba, 
    Flute, 
    Bass, 
    Violin
}

public enum FruitType {     
    Banana,
    Watermelon
}