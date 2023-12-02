using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitGroundAligner : MonoBehaviour
{
    [SerializeField] float floorOffset = 1.5f;
    [SerializeField] LayerMask layerMask;
    void Start() { if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100f, layerMask)) { transform.position = hit.point + new Vector3(0, floorOffset, 0); } }
}
