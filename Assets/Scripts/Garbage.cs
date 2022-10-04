using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Garbage : MonoBehaviour
{
    public int _instanceID;

    public delegate void GarbageAction(Garbage _garbageInstance);
    public static event GarbageAction OnClicked;
    public static event GarbageAction OnCollected;

    [SerializeField] float _timeToCollect = 1.0f; float _collectTimer;
    bool _isBeingCollected = false;

    private void Start()
    {
        _instanceID = GetInstanceID();

        Domovenok.OnGarbageReached += StartCollecting;
    }

    private void Update()
    {
        if (_isBeingCollected)
        {
            _collectTimer += Time.deltaTime;

            if(_collectTimer >= _timeToCollect)
            {
                Collected();
            }
        }
    }

    private void OnMouseDown()
    {
        if (OnClicked != null) OnClicked(this);
    }

    void StartCollecting(int _id)
    {
        if (_id == _instanceID)
        {
            _collectTimer = 0;
            _isBeingCollected = true;
        }
    }

    void Collected()
    {
        _isBeingCollected = false;
        if (OnCollected != null) OnCollected(this);

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Domovenok.OnGarbageReached -= StartCollecting;
    }
}
