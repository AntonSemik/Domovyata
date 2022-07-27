using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroomAnimation : MonoBehaviour
{
    [SerializeField] AnimationCurve _curve;
    [SerializeField] Transform _broomOrigin;
    [SerializeField] float _animationspeed;

    float _timer;
    private void Update()
    {
        _timer += Time.deltaTime * _animationspeed;
        if (_timer > 1) _timer--;
        _broomOrigin.rotation = Quaternion.Euler(0,0,_curve.Evaluate(_timer));
    }
}
