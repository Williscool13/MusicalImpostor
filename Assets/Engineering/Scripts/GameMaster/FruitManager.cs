using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

public class FruitManager : MonoBehaviour, IFruit
{
    [SerializeField] private GameObject _fruitPrefab;
    [SerializeField] private FruitPositions tubaFruitPositions;
    [SerializeField] private FruitPositions fluteFruitPositions;
    [SerializeField] private FruitPositions bassFruitPositions;
    [SerializeField] private FruitPositions violinFruitPositions;

    [SerializeField] AudioClip tuba;
    [SerializeField] AudioClip flute;
    [SerializeField] AudioClip bass;
    [SerializeField] AudioClip violin;

    [SerializeField] AudioClip tuning;

    public FruitType FruitType => FruitType.Banana;

    List<FruitController> fruits = new();
    // Start is called before the first frame update
    void Start()
    {
        SpawnFruits();
        SelectImpostor();
    }

    private void SelectImpostor() {
        // select one impostor and make it play a different track
        Dictionary<Instrument, AudioClip> instrumentDictionary = new Dictionary<Instrument, AudioClip>() {
            {Instrument.Tuba, tuba},
            {Instrument.Flute, flute},
            {Instrument.Bass, bass},
            {Instrument.Violin, violin}
        };

        int impostorIndex = Random.Range(0, fruits.Count);
        Debug.Log("[Fruit Manager Impostor Selection] Selecting impostor " + impostorIndex + " out of " + fruits.Count);
        FruitController targetFruit = fruits[impostorIndex];
        Debug.Log("[Fruit Manager Impostor Selection] Removing " + targetFruit.FruitInstrument + " from dictionary");
        instrumentDictionary.Remove(targetFruit.FruitInstrument);

        Debug.Log("[Fruit Manager Impostor Selection] Remaining elements are: " + string.Join(",", instrumentDictionary.Keys.ToArray().Select(ac => ac.ToString())));

        KeyValuePair<Instrument, AudioClip> impostorInstrument = instrumentDictionary.ElementAt(Random.Range(0, instrumentDictionary.Count));
        targetFruit.TurnImpostor(impostorInstrument.Value);
        Debug.Log("[Fruit Manager Impostor Selection] Replacing " + targetFruit.FruitInstrument + " with " + impostorInstrument.Key);
    }
    private void SpawnFruits() {
        foreach (FruitPosition fp in tubaFruitPositions._positions) {
            // convert euler rotation to quaternion

            GameObject gameObject = Instantiate(_fruitPrefab, fp.position, Quaternion.Euler(fp.eulerRotation));
            FruitController fruitController = gameObject.GetComponent<FruitController>();
            fruitController.Initialize(tuning, tuba, Instrument.Tuba, FruitType.Watermelon);

            fruits.Add(fruitController);
        }
        foreach (FruitPosition fp in fluteFruitPositions._positions) {
            GameObject gameObject = Instantiate(_fruitPrefab, fp.position, Quaternion.Euler(fp.eulerRotation));
            FruitController fruitController = gameObject.GetComponent<FruitController>();
            fruitController.Initialize(tuning, flute, Instrument.Flute, FruitType.Watermelon);

            fruits.Add(fruitController);
        }
        foreach (FruitPosition fp in bassFruitPositions._positions) {
            GameObject gameObject = Instantiate(_fruitPrefab, fp.position, Quaternion.Euler(fp.eulerRotation));
            FruitController fruitController = gameObject.GetComponent<FruitController>();
            fruitController.Initialize(tuning, bass, Instrument.Bass, FruitType.Watermelon);

            fruits.Add(fruitController);
        }
        foreach (FruitPosition fp in violinFruitPositions._positions) {
            GameObject gameObject = Instantiate(_fruitPrefab, fp.position, Quaternion.Euler(fp.eulerRotation));
            FruitController fruitController = gameObject.GetComponent<FruitController>();
            fruitController.Initialize(tuning, violin, Instrument.Violin, FruitType.Watermelon);

            fruits.Add(fruitController);
        }
    }


    public void FruitStopTuning() {
        foreach (FruitController fruitController in fruits) {
            fruitController.FadeOutMusic();
        }
    }


    public void FruitStartMainMusic() {
        foreach (FruitController fruitController in fruits) {
            fruitController.FadeInMainMusic();
            
        }
    }

}
