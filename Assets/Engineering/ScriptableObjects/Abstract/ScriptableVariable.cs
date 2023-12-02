using UnityEngine;

/// <summary>
/// Extensible base class for ScriptableVariables.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ScriptableVariable<T> : ScriptableObject
{
    [SerializeField] private T value;
    public T Value {
        get { return value; }
        set { this.value = value; }
    }
}
/// <summary>
/// Extensible base class for ScriptableVariables.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class ScriptableVariable<T1, T2> : ScriptableObject
{
    [SerializeField] private T1 value1;
    [SerializeField] private T2 value2;
    public T1 Value1 {
        get { return value1; }
        set { this.value1 = value; }
    }

    public T2 Value2 {

        get { return value2; }
        set { this.value2 = value; }
    }

}