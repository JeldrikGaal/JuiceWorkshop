using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

// This class is only used to toggle juice on and off. 

public class S_JuiceManager : MonoBehaviour
{
    [SerializeField] private TMP_Text _toggleButtonText;
    [SerializeField] private GameObject _allButtons;
    public static event Action<bool> ToggleAllJuice;
    public static event Action<Component, List<bool>> ToggleSpecificJuice;
    public static S_JuiceManager Instance;
    public bool _allOn = false;
    public bool _obstacleJuice = false;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _allButtons.SetActive(!_allButtons.activeSelf);
        }
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
        S_CameraShake.Instance._juiceToggle[index] = !S_CameraShake.Instance._juiceToggle[index];
        ToggleSpecificJuice?.Invoke(S_CameraShake.Instance, S_CameraShake.Instance._juiceToggle);
    }

    public void ToggleObstacleJuice()
    {
        _obstacleJuice = !_obstacleJuice;
    }

    public void ToggleGameManagerJuice()
    {
        S_GameManager.Instance._juiceToggle[0] = !S_GameManager.Instance._juiceToggle[0];
        ToggleSpecificJuice?.Invoke(S_GameManager.Instance, S_GameManager.Instance._juiceToggle);
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
