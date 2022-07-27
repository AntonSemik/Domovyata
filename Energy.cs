using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public static Energy _instance;

    [SerializeField] int _currentEnergy;
    [SerializeField] int _maxEnergy;

    [SerializeField] float _restoreTime;
    float _restoreTime_timer;
    long _timestamp;

    public delegate void OnEnergyGeneration(int _current, int _max);
    public static event OnEnergyGeneration OnChangeEnergy;

    private void Start()
    {
        _instance = this;
        LoadEnergyInfoFromDB();
    }

    private void Update()
    {
        if (_currentEnergy < _maxEnergy)
        {
            _restoreTime_timer += Time.deltaTime;

            if (_restoreTime_timer >= _restoreTime)
            {
                _currentEnergy++;
                if (OnChangeEnergy != null) OnChangeEnergy(_currentEnergy, _maxEnergy);

                _restoreTime_timer = 0;

                _timestamp = System.DateTime.Now.Ticks;
                SaveToDB();
            }
        }
    }

    public void RefillEnergy()
    {
        _currentEnergy = _maxEnergy;
        if (OnChangeEnergy != null) OnChangeEnergy(_currentEnergy, _maxEnergy);

        _restoreTime_timer = 0;

        _timestamp = System.DateTime.Now.Ticks;
        SaveToDB();
    }

    public bool CheckSufficientEnergy()
    {
        if (_currentEnergy > 0)
        {
            _currentEnergy--;

            if (OnChangeEnergy != null) OnChangeEnergy(_currentEnergy, _maxEnergy);

            SaveToDB();
            return true;
        }

        return false;
    }

    void LoadEnergyInfoFromDB()
    {
        int _dbValue = System.Convert.ToInt32(Database._instance.ReadOneValueFromDB("playerinfo", "energy", 1));
        _currentEnergy = _dbValue;

        float _timeFromLastRestore = (System.DateTime.Now.Ticks - System.Convert.ToInt64(Database._instance.ReadOneValueFromDB("playerinfo", "energyrestoretime", 1)))/10000000;

        while (_currentEnergy < _maxEnergy && _timeFromLastRestore >= _restoreTime)
        {
            _timeFromLastRestore -= _restoreTime;
            _currentEnergy++;
        }

        if (OnChangeEnergy != null) OnChangeEnergy(_currentEnergy, _maxEnergy);

        _timestamp = System.DateTime.Now.Ticks;
        SaveToDB();
    }
    void SaveToDB()
    {
        Database._instance.SaveValueToDB("playerinfo", "energy", 1, _currentEnergy);
        Database._instance.SaveValueToDB("playerinfo", "energyrestoretime",1, _timestamp);
    }


}
