using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ResourceReward", fileName = "NewResourceReward")]
public class RewardResource : Reward
{
    public int _itemId;
    public int _amount;

    public override void GiveReward()
    {
        Inventory._instance.AddItem(_itemId,_amount);
    }
}
