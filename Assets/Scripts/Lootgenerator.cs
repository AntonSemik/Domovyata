using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lootgenerator : MonoBehaviour
{
    public enum LootType
    {
        coin,
        resource,
        collectable
    }

    [SerializeField] float _weightCoins;
    [SerializeField] float _weightCollectable;
    [SerializeField] float _weightResource;
    float _weightSum;

    public delegate void OnLootAction(Vector3 _position);
    public static event OnLootAction OnResouceDrop;
    public static event OnLootAction OnCollectableDrop;
    public static event OnLootAction OnCoinDrop;

    LootType _droppedLootType;
    Vector3 _dropPosition;
    private void Start()
    {
        Garbage.OnCollected += GenerateLoot;

        _weightSum = _weightCoins + _weightCollectable + _weightResource;
    }

    public void GenerateLoot(Garbage _garbage)
    {
        LootType _droppedLootType = GetRandomLootType();

        _dropPosition = Camera.main.WorldToScreenPoint(_garbage.transform.position);
        switch (_droppedLootType) {

            case LootType.coin:
                Coins._instance.AddCoins(1);
                if (OnCoinDrop != null) OnCoinDrop(_dropPosition);
                break;

            case LootType.collectable:
                if (OnCollectableDrop != null) OnCollectableDrop(_dropPosition);
                break;

            case LootType.resource:
                if (OnResouceDrop != null) OnResouceDrop(_dropPosition);
                break;
        }
    }

    LootType GetRandomLootType()
    {
        float _randomNumber = Random.Range(0.0f,_weightSum);

        if (_randomNumber < _weightCoins)
        {
            return LootType.coin;
        } else if(_randomNumber < (_weightCoins + _weightCollectable))
        {
            return LootType.collectable;
        }

        return LootType.resource;
    }
}
