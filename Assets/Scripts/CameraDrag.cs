using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] float _dragSpeed = 0.2f;
    [SerializeField] Transform _cameraTransform;

    [SerializeField] Transform _edgeLeft;
    [SerializeField] Transform _edgeRight;


    bool _isActive = true;


    public void OnBeginDrag(PointerEventData eventData)
    {
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_isActive)
        {
            Vector2 _cameraMovement = new Vector2(-eventData.delta.x, 0);

            //Limiting camera movement by checking if level edge markers are on screen
            if (IsOnscreen(_edgeLeft))
            {
                _cameraMovement.x = Mathf.Max(_cameraMovement.x, 0f);
            }
            if (IsOnscreen(_edgeRight))
            {
                _cameraMovement.x = Mathf.Min(_cameraMovement.x, 0f);
            }


            _cameraTransform.Translate(_cameraMovement * _dragSpeed, Space.World);
        }
    }
    bool IsOnscreen(Transform _transform)
    {
        float _relativePosition = Camera.main.WorldToViewportPoint(_transform.position).x;
        return (_relativePosition > 0 && _relativePosition < 1);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    void Activate()
    {
        _isActive = true;
    }

    void Deactivate()
    {
        _isActive = false;
    }
}