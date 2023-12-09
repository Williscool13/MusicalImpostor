using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameImpostorManager : MonoBehaviour
{
    [SerializeField] ScriptableVariable_BlenderVictim blenderVictim;
    [SerializeField] ScriptableVariable_Bool isVictorious;
    [SerializeField] Vector2 targetViewPortPosition;

    /// <summary>
    /// When player has selected an impostor, this will respond to the ScriptableGameEvent_ImpostorSelected event.
    /// It will save the reference of the unit's FruitController to the blenderVictim ScriptableVariable.
    /// </summary>
    /// <param name="target"></param>
    public void OnImpostorSelected(GameObject target) {
        FruitController impostorController = target.GetComponent<FruitController>();
        blenderVictim.Value = impostorController.FruitType;
        isVictorious.Value = impostorController.Impostor;

        targetViewPortPosition = Camera.main.WorldToViewportPoint(target.transform.position + new Vector3(0, 0.6f, 0));
        Debug.Log("Transition offset is " + targetViewPortPosition);
    }

    public void OnGameOver() {
        SceneTransitioner.Instance.TransitionScene("ScoreScene", targetViewPortPosition, new Vector2(0.5f, 0.5f));
    }
}
