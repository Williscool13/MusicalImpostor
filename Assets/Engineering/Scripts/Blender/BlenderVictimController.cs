using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlenderVictimController : MonoBehaviour
{
    [SerializeField] Collider thisCollider;
    [SerializeField] Rigidbody thisRigidbody;

    bool split = false;
    public void OnBlenderHit() {
        if (split) { return; }
        split = true;
        thisRigidbody.isKinematic = true;
        thisCollider.enabled = false;
        foreach (Rigidbody rb in transform.GetComponentsInChildren<Rigidbody>()) {
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        foreach (Collider col in transform.GetComponentsInChildren<Collider>()) {
            col.enabled = true;
        }

    }
}
