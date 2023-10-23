using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class S_JuiceManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _toggleButtonText;
    public static event Action<bool> ToggleAllJuice;
    public static event Action<Component, List<bool>> ToggleSpecificJuice;
    public static S_JuiceManager Instance;
    public bool _allOn = true;

    // This makes this a 'singleon' pattern, meaning that there can only be one instance of this class in the scene.
    // This is useful for classes that need to be accessed from anywhere in the scene, like a GameManager ( JuiceManager ).
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

    public void ToggleObject(Component script, List<bool> juiceToggle)
    {
        ToggleSpecificJuice?.Invoke(script, juiceToggle);
    }
    
    public void TogglePlayerControllerJuice(int index)
    {
        S_PlayerController.Instance._juiceToggle[index] = !S_PlayerController.Instance._juiceToggle[index];
        ToggleSpecificJuice?.Invoke(S_PlayerController.Instance, S_PlayerController.Instance._juiceToggle);
    }
    
    public void ToggleBulletJuice(int index)
    {
        //S_BulletLogic.Instance._juiceToggle[index] = !S_BulletLogic.Instance._juiceToggle[index];
        //ToggleSpecificJuice?.Invoke(S_BulletLogic.Instance, S_BulletLogic.Instance._juiceToggle);
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
