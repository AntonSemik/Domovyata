using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject _panel;

    [SerializeField] GameObject _slotsParent;
    [SerializeField] GameObject _slotPrefab;

    [SerializeField] Image _imageToMoveOnDrop;
    [SerializeField] Transform _defaultPosition;

    public InventorySlot[] _slotsUIelements;
    GameObject _tempGO; int i;

    public delegate void OnInventoryOpened();
    public static event OnInventoryOpened OnInventoryOpen;

    public void CreateInventorySlots(int _size)
    {
        _slotsUIelements = new InventorySlot[_size];

        i = 0;
        StartCoroutine(CreateSlots());
    }
    IEnumerator CreateSlots()
    {
        foreach (InventorySlot _inventorySlot in _slotsUIelements)
        {
            _tempGO = Instantiate(_slotPrefab, transform.position, Quaternion.identity, _slotsParent.transform);
            yield return null;

            _slotsUIelements[i] = _tempGO.GetComponent<InventorySlot>();

            i++;
        }
    }

    Vector3 _dropPosition;
    public void AnimateDrop(Vector3 _position, ResourceItem _item)
    {
        _position.z = 0;
        _dropPosition = _position;

        _imageToMoveOnDrop.sprite = _item._icon;
        _imageToMoveOnDrop.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(AnimateResource());
    }

    IEnumerator AnimateResource()
    {
        for (Vector3 position = _dropPosition; position != _defaultPosition.position; position += (_defaultPosition.position - position) * 0.05f)
        {
            if ((_defaultPosition.position - position).sqrMagnitude <= 0.1f) position = _defaultPosition.position;

            _imageToMoveOnDrop.transform.position = position;
            yield return null;
        }

        _imageToMoveOnDrop.gameObject.SetActive(false);
        yield return null;
    }


    public void Open()
    {
        if (OnInventoryOpen != null) OnInventoryOpen();

        _panel.SetActive(true);

    }
    public void Close()
    {
        _panel.SetActive(false);
    }

}
