using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ResourceItemBase))]
[RequireComponent(typeof(InventoryUI))]
public class Inventory : MonoBehaviour
{
    public static Inventory _instance;
    [SerializeField] ResourceItemBase _itemBase;
    [SerializeField] InventoryUI _inventoryUI;

    int _inventorysize = 16;
    int[] _itemIds, _itemQuantities;


    private void Awake()
    {
        _instance = this;
        _itemBase = GetComponent<ResourceItemBase>();
        _inventoryUI = GetComponent<InventoryUI>(); _inventoryUI.CreateInventorySlots(_inventorysize);
        _itemIds = new int[_inventorysize];
        _itemQuantities = new int[_inventorysize];

        LoadInventory();
    }

    private void Start()
    {
        InventoryUI.OnInventoryOpen += UpdateWholeInventoryUI;

        Lootgenerator.OnResouceDrop += AddRandomItem;
    }

    public void AddRandomItem(Vector3 _dropPosition)
    {
        int _randomId = Random.Range(1, _itemBase._availaibleItems.Length);
        AddItem(_randomId , 1);

        _inventoryUI.AnimateDrop(_dropPosition, _itemBase._availaibleItems[_randomId]);
    }

    public void AddItem(int _newItemId, int _amount)
    {
        bool _itemAdded = false;
        for(int i = 0; i < _inventorysize; i++)
        {
            if (_itemIds[i] == _newItemId)
            {
                _itemQuantities[i] += _amount;
                _itemAdded = true;

                SaveInventory();
                //SaveSlot(i+1);

                break;
            }

        }
        if (!_itemAdded)
        {
            for (int i = 0; i < _inventorysize; i++)
            {
                if (_itemIds[i] == 0)
                {
                    _itemIds[i] = _newItemId;
                    _itemQuantities[i] += _amount;
                    _itemAdded = true;

                    SaveInventory();

                    //SaveSlot(i + 1);

                    break;
                }

            }
        }

        if (!_itemAdded)
        {
            Debug.LogError("No appropriate slot");
        }
    }

    public void UpdateWholeInventoryUI()
    {
        int i = 0;
        foreach (InventorySlot _slot in _inventoryUI._slotsUIelements)
        {
            UpdateUIElement(i);
            i++;
        }
    }

    void UpdateUIElement(int _slotId)
    {
        //CheckSlotForEmpty(_slotId);
        _inventoryUI._slotsUIelements[_slotId].SetProperties(_itemBase._availaibleItems[_itemIds[_slotId]]._icon, _itemQuantities[_slotId]);
    }

    void CheckSlotForEmpty(int _slotId)
    {
        if(_itemQuantities[_slotId] < 1)
        {
            _itemIds[_slotId] = 0;
        }
    }

    void LoadInventory()
    {
        int i = 0;
        foreach(int _id in _itemIds)
        {
            _itemIds[i] = System.Convert.ToInt32(Database._instance.ReadOneValueFromDB("inventory", "itemid", i+1));
            _itemQuantities[i] = System.Convert.ToInt32(Database._instance.ReadOneValueFromDB("inventory", "quantity", i + 1));

            i++;
        }
    }

    void SaveInventory()
    {
        for (int i = 0; i < _inventorysize; i++)
        {
            SaveSlot(i+1);
        }
    }

    void SaveSlot(int _slotid)
    {
            Database._instance.SaveValueToDB<int>("inventory", "itemid", _slotid, _itemIds[_slotid-1]);
            Database._instance.SaveValueToDB<int>("inventory", "quantity", _slotid, _itemQuantities[_slotid-1]);
    }

    private void OnDestroy()
    {
        Lootgenerator.OnResouceDrop -= AddRandomItem;
        InventoryUI.OnInventoryOpen -= UpdateWholeInventoryUI;
    }
}
