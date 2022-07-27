using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collections : MonoBehaviour
{
    public static Collections _instance;

    [System.Serializable]
    public class Collection
    {
        public int _collectionId;

        public string _collectionName;

        public Item[] _collectableItems = new Item[5];

        public Reward _reward;
    }

    public Collection[] _collectionArray;


    public delegate void OnItemAdded();
    public static event OnItemAdded OnUpdateUI;
    public delegate void OnDropAnimate(Vector3 _position, Item _item);
    public static event OnDropAnimate OnItemDropped;

    private void Awake()
    {
        _instance = this;

        LoadCollections();

        Lootgenerator.OnCollectableDrop += AddRandomItem;

        CollectionElement.OnGetRewardEvent += GetRewardForCollection;
    }

    public void AddItem(int _itemId, int _amount)
    {
        foreach (Collection _collection in _collectionArray)
        {
            foreach (Item _item in _collection._collectableItems)
            {
                if(_item._id == _itemId)
                {
                    _item._quantity += _amount;
                    if (OnUpdateUI != null) OnUpdateUI();

                    SaveItemQuantity(_item._id, _item._quantity);
                }
            }
        }

    }

    void AddRandomItem(Vector3 _vector3)
    {
        //Choose random collection
        int _array = Random.Range(0, _collectionArray.Length);

        Item _item = _collectionArray[_array]._collectableItems[Random.Range(0, _collectionArray[_array]._collectableItems.Length)];

        _item._quantity++;

        if (OnUpdateUI != null) OnUpdateUI();
        if (OnItemDropped != null) OnItemDropped(_vector3, _item);

        SaveItemQuantity(_item._id,_item._quantity);
    }

    void GetRewardForCollection(int _collectionId)
    {
        foreach(Item _item in _collectionArray[_collectionId]._collectableItems)
        {
            _item._quantity -= 1;
            SaveItemQuantity(_item._id, _item._quantity);
        }

        _collectionArray[_collectionId]._reward.GiveReward();

        if (OnUpdateUI != null) OnUpdateUI();
    }

    void LoadCollections()
    {
        foreach(Collection _tempCollection in _collectionArray)
        {
            int i = 0;

            foreach (Item _tempItem in _tempCollection._collectableItems)
            {

                _tempCollection._collectableItems[i]._quantity = System.Convert.ToInt32(Database._instance.ReadOneValueFromDB("collectables", "quantity", _tempCollection._collectableItems[i]._id));
                i++;
            }
        }
    }

    void SaveItemQuantity(int _itemId, int _quantity)
    {
        Database._instance.SaveValueToDB("collectables","quantity",_itemId, _quantity);
    }

    private void OnDestroy()
    {
        Lootgenerator.OnCollectableDrop -= AddRandomItem;
    }
}
