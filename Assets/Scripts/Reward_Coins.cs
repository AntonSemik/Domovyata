using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CoinReward", fileName = "NewCoinReward")]
public class Reward_Coins : Reward
{
    public int _amount;

    public override void GiveReward()
    {
        Coins._instance.AddCoins(_amount);
    }
}
