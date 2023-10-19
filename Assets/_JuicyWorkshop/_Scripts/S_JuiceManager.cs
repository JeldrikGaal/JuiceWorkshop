using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class S_JuiceManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _toggleButtonText;
    public static event Action<bool> ToggleAllJuice;
    public static S_JuiceManager Instance;
    public bool _allOn = true;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one JuiceManager in the scene!");
            Destroy(this);
        }
    }

    private void Start()
    {
        ToggleAllJuice?.Invoke(_allOn);
    }

    public void ToggleAll()
    {
        _allOn = !_allOn;
        ToggleAllJuice?.Invoke(_allOn);
        _toggleButtonText.text = _allOn.ToString();
    }
    
    private void SetAll(bool on)
    {
        _allOn = on;
        ToggleAllJuice?.Invoke(_allOn);
    }

    public static List<bool> GetBoolList(bool content, int amount)
    {
        List<bool> bools = new List<bool>();
        for (int i = 0; i < amount; i++)
        {
            bools.Add(content);
        }

        return bools;
    }
}
