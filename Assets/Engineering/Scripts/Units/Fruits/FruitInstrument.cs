using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitInstrument : MonoBehaviour, IFruitComponent
{
    public GameObject FruitParent { get; set; }
}


public interface IFruitComponent
{
    public GameObject FruitParent { get; set; }
}