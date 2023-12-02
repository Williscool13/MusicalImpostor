using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimeoutManager : MonoBehaviour
{
    [SerializeField] ScriptableVariable_Bool isVictorious;
    [SerializeField] ScriptableVariable_BlenderVictim blenderVictim;
    public void TimeoutSceneTransition() {
        SceneTransitioner.Instance.TransitionScene("ScoreScene");
    }

    public void DisableTimeoutVictory() {
        isVictorious.Value = false;
        blenderVictim.Value = FruitType.Banana;
    }

}
