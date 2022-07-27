using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CreateResource", fileName = "NewResource")]
public class ResourceItem : ScriptableObject
{
    public int _id;

    public Sprite _icon;
}
