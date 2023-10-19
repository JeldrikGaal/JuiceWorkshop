using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_CameraShake : MonoBehaviour
{
    public static S_CameraShake Instance;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one CameraShake in the scene!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Shake(float duration, float intensity)
    {
        Vector3 punch = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
        transform.DOPunchRotation(punch, duration, 1);
    }
}
