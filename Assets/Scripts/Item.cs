using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Create Item", fileName = "NewItem")]
public class Item : ScriptableObject
{
    public int _id;
    public Sprite _iconSprite;
    public int _quantity;
}
