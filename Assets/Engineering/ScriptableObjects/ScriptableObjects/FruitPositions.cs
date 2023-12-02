using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FruitPositions", menuName = "ScriptableObjects/FruitPositions", order = 1)]
public class FruitPositions : ScriptableObject
{
    public List<FruitPosition> _positions;
}


[Serializable]
public struct FruitPosition
{
    public Vector3 position;
    public Vector3 eulerRotation;
    public FruitPosition(Vector3 position, Vector3 eulerRotation) {
        this.position = position;
        this.eulerRotation = eulerRotation;
    }
}
