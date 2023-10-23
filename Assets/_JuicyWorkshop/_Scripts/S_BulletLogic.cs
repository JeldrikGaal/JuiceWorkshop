using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_BulletLogic : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _bulletDamage;
    [SerializeField] private float _bulletLifetime;
    [SerializeField] private Vector2 _bulletSizeRange;
    [SerializeField] private GameObject _visuals;
    [SerializeField] private float _maxDamageRange;

    private Rigidbody2D _rigidbody2D;
    private TrailRenderer _trailRenderer;
    
    [Tooltip("0: Camera shake")]
    [SerializeField] public List<bool> _juiceToggle;
    
    #region Unity Events
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();

        _rigidbody2D.velocity = new Vector2(0, _bulletSpeed);
        _visuals.transform.localScale = new Vector3(Random.Range(_bulletSizeRange.x, _bulletSizeRange.y), Random.Range(_bulletSizeRange.x, _bulletSizeRange.y), 1);
        Destroy(gameObject, _bulletLifetime);
        
        ToggleAllJuice(S_JuiceManager.Instance._allOn);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (Vector3.Distance(transform.position, S_PlayerController.Instance.transform.position) < _maxDamageRange)
            {
                other.GetComponent<S_ObstacleLogic>().TakeDamage(_bulletDamage);

                // Camera shake
                if (_juiceToggle[0])
                {
                    S_CameraShake.Instance.Shake(0.05f, 1.5f);
                }
                
            }
            Die();
        }
    }
    
    
    private void OnEnable()
    {
        S_JuiceManager.ToggleAllJuice += ToggleAllJuice;
    }
    
    private void OnDisable()
    {
        S_JuiceManager.ToggleAllJuice -= ToggleAllJuice;
    }
    #endregion
    
    private void Die()
    {
        Destroy(gameObject);
    }

    private void ToggleAllJuice(bool on)
    {
        _juiceToggle = S_JuiceManager.GetBoolList(on, _juiceToggle.Count);
        _trailRenderer.enabled = on;
    }
    private void ToggleJuice(List<bool> toggles)
    {
        _juiceToggle = toggles;
    }
    
}
