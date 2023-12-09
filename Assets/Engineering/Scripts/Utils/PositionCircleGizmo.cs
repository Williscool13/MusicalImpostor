using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCircleGizmo : MonoBehaviour
{
    [SerializeField] Color gizmoColor = Color.green;
    private void OnDrawGizmos() {
        Gizmos.color = gizmoColor;
        Gizmos.DrawSphere(transform.position, 0.5f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.0f);
    }
}
