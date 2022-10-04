using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollectionsUI : MonoBehaviour
{
    [SerializeField] GameObject _panel;
    [SerializeField] GameObject _collectionPrefab, _collectionsHolder;

    [SerializeField] Image _imageToMoveOnDrop;
    [SerializeField] Transform _defaultPosition;

    GameObject _tempGO;
    public CollectionElement[] _elements;
    private void Start()
    {
        CreateCollectionPanels();

        _elements = new CollectionElement[Collections._instance._collectionArray.Length];

        Collections.OnItemDropped += AnimateDrop;
    }

    int i = 0;
    void CreateCollectionPanels()
    {
        i = 0;
        StartCoroutine(CreateCollections());
    }
    IEnumerator CreateCollections()
    {
        foreach (Collections.Collection _collection in Collections._instance._collectionArray)
        {
            _tempGO = Instantiate(_collectionPrefab, transform.position, Quaternion.identity, _collectionsHolder.transform);
            yield return null;

            _elements[i] = _tempGO.GetComponent<CollectionElement>();
            _tempGO.TryGetComponent<CollectionElement>(out _elements[i]);
            _elements[i]._id = i;

            i++;
        }
    }

    Vector3 _dropPosition;
    void AnimateDrop(Vector3 _position, Item _item)
    {
        _position.z = 0;
        _dropPosition = _position;

        _imageToMoveOnDrop.sprite = _item._iconSprite;
        _imageToMoveOnDrop.gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(AnimateCollectable());
    }

    IEnumerator AnimateCollectable()
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
        _panel.SetActive(true);
        _collectionsHolder.SetActive(true);
    }
    public void Close()
    {
        _panel.SetActive(false);
    }

    private void OnDestroy()
    {
        Collections.OnItemDropped -= AnimateDrop;
    }
}
