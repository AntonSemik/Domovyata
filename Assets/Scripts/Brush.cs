using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    [SerializeField] GameObject _brush;

    private void Start()
    {
        Domovenok.OnGarbageReached += ActivateBrush;
        Garbage.OnCollected += DeactivateBrush;
    }

    void ActivateBrush(int i)
    {
        _brush.SetActive(true);
    }

    void DeactivateBrush(Garbage _garbageInstance)
    {
        _brush.SetActive(false);
    }

    private void OnDestroy()
    {
        Domovenok.OnGarbageReached -= ActivateBrush;
        Garbage.OnCollected -= DeactivateBrush;
    }
}
