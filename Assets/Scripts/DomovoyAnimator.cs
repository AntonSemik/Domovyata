using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class DomovoyAnimator : MonoBehaviour
{
    public static DomovoyAnimator _instance;

    [SerializeField] AnimationState[] _animations;
    [SerializeField] SkeletonAnimation _skeletonAnimation;

    private void Start()
    {
        _instance = this;

        ChangeAnimationState("idle",true, 1f);
        Debug.Log(_skeletonAnimation.AnimationName);
    }

    public void ChangeAnimationState(string _animationName, bool _isLooped, float _timescale)
    {
        if (_skeletonAnimation.AnimationName != _animationName)
        {
            _skeletonAnimation.state.SetAnimation(0, _animationName, _isLooped).TimeScale = _timescale;
        }
    }
}
