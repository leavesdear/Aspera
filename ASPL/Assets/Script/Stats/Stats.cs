using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Stats
{
    [SerializeField] private int baseValue;
    [SerializeField] List<int> modifier;

    public int GetValue()
    {
        int finalValue = baseValue;

        foreach (int modifier in modifier)
        {
            finalValue += modifier;
        }

        return finalValue;
    }

    public void SetDefaultValue(int _value)
    {
        baseValue = _value;
    }

    public void AddModifier(int _modifier)
    {
        modifier.Add(_modifier);
    }

    public void RemoveModifier(int _modifier)
    {
        modifier.Remove(_modifier);
    }
}
