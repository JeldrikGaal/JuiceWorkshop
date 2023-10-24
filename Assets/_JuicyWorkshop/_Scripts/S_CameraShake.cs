using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_CameraShake : MonoBehaviour
{
    public static S_CameraShake Instance;

    [Tooltip("0: BulletImpact, 1: BulletShot")]
    [SerializeField] public List<bool> _juiceToggle;

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

    private void OnEnable()
    {
        S_BulletLogic.BulletImpact += BulletImpactShake;
        S_BulletLogic.BulletShot += BulletShotShake;
        S_JuiceManager.ToggleAllJuice += ToggleAllJuice;
    }

    private void OnDisable()
    {
        S_BulletLogic.BulletImpact -= BulletImpactShake;
        S_BulletLogic.BulletShot -= BulletShotShake;
        S_JuiceManager.ToggleAllJuice -= ToggleAllJuice;
    }
    
    public void Shake(float duration, float intensity)
    {
        Vector3 punch = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity), 0);
        transform.DOPunchRotation(punch, duration, 1);
    }

    private void BulletImpactShake()
    {
        if(!_juiceToggle[0]) return;
       Shake(0.05f, 1.5f);
    }
    
    private void BulletShotShake()
    {
        if(!_juiceToggle[1]) return;
        Shake(0.025f, 0.5f);
    }
    
    private void ToggleAllJuice(bool on)
    {
        _juiceToggle = S_JuiceManager.GetBoolList(on, _juiceToggle.Count);
    }
    private void ToggleJuice(Component Script, List<bool> toggles)
    {
        if (this != Script) return;
        _juiceToggle = toggles;
    }    
}
