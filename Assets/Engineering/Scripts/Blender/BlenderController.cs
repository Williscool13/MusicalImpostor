using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BlenderController : MonoBehaviour
{
    [SerializeField] ScriptableVariable_BlenderVictim blenderVictim;
    [SerializeField] ScriptableVariable_Bool isVictorious;
    [SerializeField] AudioSource blenderSource;
    [SerializeField] AudioSource pulseSource;
    // 0 = banana (detective), 1 = watermelon
    [SerializeField] GameObject[] victims;
    [SerializeField] ScoreSceneController scoreSceneController;

    [SerializeField] float sceneDuration = 5.0f;
    Dictionary<FruitType, GameObject> gameObjectMap;
    float blenderTimer = 0;
    bool blenderOn = false;

    Rigidbody victimRb;
    void Start() {
        Debug.Log("Blender Victim: " + blenderVictim.Value);
        Debug.Log("Is Victorious: " + isVictorious.Value);

        gameObjectMap = new Dictionary<FruitType, GameObject>() {
            { FruitType.Banana, victims[1]},
            { FruitType.Watermelon, victims[0]},

        };
        // spawn blender victim
        victimRb = Instantiate(gameObjectMap[blenderVictim.Value], transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        victimRb.useGravity = false;

        StartBlender();
    }

    bool swapped = false;
    private void Update() {
        if (blenderOn && !swapped) {
            blenderTimer += Time.deltaTime;
            if (blenderTimer > sceneDuration) {
                swapped = true;
                Debug.Log("Transition");
                //blenderSource.volume /= 20f;
                //pulseSource.volume /= 20f;
                scoreSceneController.OnBlenderFinish();
                //SceneTransitioner.Instance.TransitionScene("ScoreScene");

            }
        }

    }

    [ContextMenu("OnSceneLoad")]
    public void OnSceneLoad() {
        if (!blenderOn) {
            StartBlender();
        }
    }

    void StartBlender() {
        blenderSource.Play();
        blenderOn = true;

        victimRb.useGravity = true;

    }
}
