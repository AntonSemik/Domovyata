using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CollectableReward", fileName = "NewCollectableReward")]
public class Reward_Collectable : Reward
{
    public int _itemId;
    public int _amount;

    public override void GiveReward()
    {
        Collections._instance.AddItem(_itemId, _amount);
    }
}
