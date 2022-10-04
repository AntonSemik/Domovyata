using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinUI : MonoBehaviour
{
    [SerializeField] TMP_Text _coinText;

    [SerializeField] Transform _coinImageToMove;
    [SerializeField] Transform _defaultPosition;

    Vector3 _dropPosition;

    private void Start()
    {
        Coins.OnCoinsChange += UpdateCoinUI;
        Lootgenerator.OnCoinDrop += CoinDropped;
    }

    void CoinDropped(Vector3 _position)
    {
        _position.z = 0;
        _dropPosition = _position;

        StopAllCoroutines();
        StartCoroutine(AnimateCoin());
    }

    IEnumerator AnimateCoin()
    {
        for (Vector3 position = _dropPosition; position != _defaultPosition.position; position += (_defaultPosition.position - position) * 0.05f)
        {
            _coinImageToMove.position = position;
            yield return null;
        }
    }

    void UpdateCoinUI(int _coinAmount)
    {
        _coinText.text = _coinAmount.ToString();
    }

    private void OnDestroy()
    {
        Coins.OnCoinsChange -= UpdateCoinUI;
        Lootgenerator.OnCoinDrop -= CoinDropped;
    }

}
