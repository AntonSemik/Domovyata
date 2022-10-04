using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : ScriptableObject
{
    public Sprite _icon;

    public virtual void GiveReward() { }
}
