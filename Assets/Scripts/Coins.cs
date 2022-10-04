using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public static Coins _instance;
    int _currentCoinAmount = 0;

    public delegate void CoinEvent(int _amount);
    public static event CoinEvent OnCoinsChange;

    private void Start()
    {
        _instance = this;
        _currentCoinAmount = LoadFromDB();
        if (OnCoinsChange != null) OnCoinsChange(_currentCoinAmount);
    }

    public void AddCoins(int _amount)
    {
        _currentCoinAmount+=_amount;

        if (OnCoinsChange != null) OnCoinsChange(_currentCoinAmount);

        SaveToDB();
    }

    void SaveToDB()
    {
        Database._instance.SaveValueToDB("playerinfo", "coins", 1,_currentCoinAmount);
        
    }

    int LoadFromDB()
    {
        int _dbValue = System.Convert.ToInt32(Database._instance.ReadOneValueFromDB("playerinfo","coins",1));
        return _dbValue;
    }

}
