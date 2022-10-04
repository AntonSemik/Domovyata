using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CollectionElement : MonoBehaviour
{
    public int _id;

    public TMP_Text _collectionName;

    [SerializeField] TMP_Text[] _quantityTexts;
    int[] _quantities;
    public Image[] _icons;
    public Image _rewardImage;

    [SerializeField] GameObject _rewardButton;
    bool IsComplete()
    {
        foreach (int i in _quantities)
        {
            if (i < 1) return false;
        }

        return true;
    }

    public delegate void OnGetReward(int _id);
    public static event OnGetReward OnGetRewardEvent;

    private void Awake()
    {
        _quantities = new int[_quantityTexts.Length];

        Collections.OnUpdateUI += UpdateQuantities;
        SetIconsAndName();
    }

    public void GetReward()
    {
        if (OnGetRewardEvent != null) OnGetRewardEvent(_id);
    }

    private void OnEnable()
    {
        UpdateQuantities();
    }

    void UpdateQuantities()
    {
        for (int i = 0; i < _quantityTexts.Length; i++)
        {
            _quantities[i] = Collections._instance._collectionArray[_id]._collectableItems[i]._quantity;

            _quantityTexts[i].text = _quantities[i].ToString();
        }

        if (IsComplete())
        {
            _rewardButton.SetActive(true);
        } else
        {
            _rewardButton.SetActive(false);
        }
    }

    void SetIconsAndName()
    {
        _collectionName.text = Collections._instance._collectionArray[_id]._collectionName;

        _rewardImage.sprite = Collections._instance._collectionArray[_id]._reward._icon;

        int i = 0;
        foreach(Image _image in _icons)
        {
            _image.sprite = Collections._instance._collectionArray[_id]._collectableItems[i]._iconSprite;

            i++;
        }
    }

    private void OnDestroy()
    {
        Collections.OnUpdateUI -= UpdateQuantities;
    }
}
