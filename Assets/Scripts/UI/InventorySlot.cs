using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image _iconImage;
    [SerializeField] TMP_Text _quantityText;

    public void SetProperties(Sprite _icon, int _quantity)
    {
        _iconImage.sprite = _icon;
        _quantityText.text = _quantity.ToString();
    }
}
