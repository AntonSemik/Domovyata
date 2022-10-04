using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Domovenok : MonoBehaviour
{
    [SerializeField]float _runSpeed;

    public delegate void DomovenokAction(int id);
    public static event DomovenokAction OnGarbageReached;


    enum Activity
    {
        idle,
        running,
        collecting
    }

    Activity _activityCurrent = Activity.idle;
    float _Xtarget, _Xdifference = 0, _XdifferenceThreshold = 0.15f;
    bool IsOnscreen()
    {
        float _relativePosition = Camera.main.WorldToViewportPoint(transform.position).x;
        return (_relativePosition > 0 && _relativePosition < 1);
    }

    int _targetGarbageID;
    bool _isControlled = true;
    private void Start()
    {
        Garbage.OnClicked += OnGarbageCall;
        Garbage.OnCollected += OnCollectedGarbage;
    }


    private void Update()
    {
        if (_isControlled)
        {
            if (!IsOnscreen() && _activityCurrent != Activity.collecting)
            {
                _activityCurrent = Activity.running;

                _Xtarget = Camera.main.transform.position.x;
                _targetGarbageID = 0;
            }

            if (Input.GetMouseButtonDown(0) && _activityCurrent != Activity.collecting)
            {
                _activityCurrent = Activity.running;

                _Xtarget = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                _targetGarbageID = 0;
            }

            _Xdifference = _Xtarget - transform.position.x;

            switch (_activityCurrent)
            {
                case Activity.idle:
                    DomovoyAnimator._instance.ChangeAnimationState("idle", true, 1f);
                    break;

                case Activity.running:
                    RunToTarget();
                    if (Mathf.Abs(_Xdifference) < _XdifferenceThreshold)
                    {
                        _activityCurrent = Activity.idle;
                    }
                    break;

                case Activity.collecting:
                    if (Mathf.Abs(_Xdifference) < _XdifferenceThreshold)
                    {
                        DomovoyAnimator._instance.ChangeAnimationState("think", false, 1.2f);
                        StartCollectingGarbage();
                    }
                    else
                    {
                        RunToTarget();
                    }
                    break;
            }
        }
    }

    void RunToTarget()
    {
        if (_Xdifference < 0)
        {
            DomovoyAnimator._instance.ChangeAnimationState("run_left", true, 1f);
        }
        else
        {
            DomovoyAnimator._instance.ChangeAnimationState("run_right", true, 1f);
        }
        transform.Translate(new Vector3(_Xdifference,0,0).normalized * _runSpeed * Time.deltaTime);
    }

    void OnGarbageCall(Garbage _garbage)
    {
        _activityCurrent = Activity.collecting;

        _targetGarbageID = _garbage._instanceID;
        _Xtarget = _garbage.transform.position.x;
    }

    void StartCollectingGarbage()
    {
        if (Energy._instance.CheckSufficientEnergy())
        {

            _isControlled = false;
            if (OnGarbageReached != null) OnGarbageReached(_targetGarbageID);
        } else
        {
            _targetGarbageID = 0;
            _activityCurrent = Activity.idle;
        }
    }

    void OnCollectedGarbage(Garbage _garbage)
    {
        _targetGarbageID = 0;
        _activityCurrent = Activity.idle;

        _isControlled = true;
    }


    private void OnDestroy()
    {
        Garbage.OnClicked -= OnGarbageCall;
        Garbage.OnCollected -= OnCollectedGarbage;
    }
}
