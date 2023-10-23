using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    public static event Action BulletImpact;
    public static event Action BulletShot;
    
    #region Unity Events
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _trailRenderer = GetComponentInChildren<TrailRenderer>();

        _rigidbody2D.velocity = new Vector2(0, _bulletSpeed);
        _visuals.transform.localScale = new Vector3(Random.Range(_bulletSizeRange.x, _bulletSizeRange.y), Random.Range(_bulletSizeRange.x, _bulletSizeRange.y), 1);
        Destroy(gameObject, _bulletLifetime);
        
        ToggleAllJuice(S_JuiceManager.Instance._allOn);
        BulletShot?.Invoke();
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
                    BulletImpact?.Invoke();
                }
                
            }
            Die();
        }
    }
    
    
    private void OnEnable()
    {
        S_JuiceManager.ToggleAllJuice += ToggleAllJuice;
        S_JuiceManager.ToggleSpecificJuice += ToggleJuice;
    }
    
    private void OnDisable()
    {
        S_JuiceManager.ToggleAllJuice -= ToggleAllJuice;
        S_JuiceManager.ToggleSpecificJuice -= ToggleJuice;
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
    private void ToggleJuice(Component Script, List<bool> toggles)
    {
        if (this != Script) return;
        _juiceToggle = toggles;
    }    
    
}
