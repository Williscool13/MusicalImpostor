using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitioner : MonoBehaviour
{
    [SerializeField] CircleWipeController circleWipeController;
    [SerializeField] ScriptableGameEvent_Null sceneTransitionComplete;

    public static SceneTransitioner Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(this);
        } else {
            Destroy(this);
        }

        //StartCoroutine(FirstTransition());
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.PageUp)) {
            TransitionScene("MainScene");
            Debug.Log("pressed transition");
        }
    }
    [ContextMenu("Simulate Transition")]
    public void SimulateTransition() {
        sceneTransitionComplete.Raise(null);
    }

    public void TransitionScene(string sceneName) {
        if (transitioning) { Debug.Log("Already Transitioning"); return; }
        transitioning = true;
        StartCoroutine(FadeOut(sceneName, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f)));
    }
    public void TransitionScene(string sceneName, Vector2 exitOffset, Vector2 enterOffset) {
        if (transitioning) return;
        transitioning = true;
        StartCoroutine(FadeOut(sceneName, exitOffset, enterOffset));
    }
    bool transitioning = false;

    IEnumerator FadeOut(string sceneName, Vector2 exitOffset, Vector2 enterOffset) {
        float waitTime = circleWipeController.CircleWipeExitScene(exitOffset);
        yield return new WaitForSeconds(waitTime);

        SceneManager.LoadScene(sceneName);
        yield return new WaitForSeconds(1.0f);
        /*while (!asyncLoad.isDone) {
            yield return null;
        }*/

        waitTime = circleWipeController.CircleWipeEnterScene(enterOffset);
        yield return new WaitForSeconds(waitTime);

        sceneTransitionComplete.Raise(null);
        transitioning = false;
    }

    IEnumerator FirstTransition() {
        float waitTime = circleWipeController.CircleWipeEnterScene(new Vector2(0.5f, 0.5f));
        yield return new WaitForSeconds(waitTime);

        sceneTransitionComplete.Raise(null);
        transitioning = false;
    }
}
