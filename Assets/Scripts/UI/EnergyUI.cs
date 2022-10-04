using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] TMP_Text _energyText;

    private void Awake()
    {
        Energy.OnChangeEnergy += UpdateEnergyUI;
    }

    void UpdateEnergyUI(int _current, int _max)
    {
        _energyText.text = _current + "/" + _max;

    }

    private void OnDestroy()
    {
        Energy.OnChangeEnergy -= UpdateEnergyUI;
    }
}
