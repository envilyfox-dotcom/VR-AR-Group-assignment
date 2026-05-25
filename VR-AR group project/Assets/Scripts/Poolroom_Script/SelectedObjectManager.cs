using UnityEngine;

public class SelectedObjectManager : MonoBehaviour
{
    public static RotatableSocketObject Current { get; private set; }

    public static void Select(RotatableSocketObject obj) => Current = obj;
    public static void Deselect(RotatableSocketObject obj)
    {
        if (Current == obj) Current = null;
    }
    void OnDisable()
    {
        Current = null;
    }
}