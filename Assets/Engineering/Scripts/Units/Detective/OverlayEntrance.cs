using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayEntrance : MonoBehaviour
{
    [SerializeField] private float lerpMult = 1f;
    [SerializeField] private float downDistance = 0.5f;
    Vector3 basepos;
    private void Awake() {
        basepos = transform.localPosition;
        DropDown();
    }

    [ContextMenu("DropDown")]
    public void DropDown() {
        transform.localPosition -= new Vector3(0, downDistance, 0);
    }


    [ContextMenu("EnterOverlay")]
    public void EnterOverlay() {
        StartCoroutine(OverlayEntranceCoroutine());
    }

    IEnumerator OverlayEntranceCoroutine() {
        while (Vector3.Distance(transform.localPosition, basepos) > 0.01f) {
            //transform.localPosition = Vector3.Lerp(transform.localPosition, basepos, Time.deltaTime / lerpMult);
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, basepos, Time.deltaTime * lerpMult);
            yield return null;
        }
        Debug.Log("Reached");
    }
}
