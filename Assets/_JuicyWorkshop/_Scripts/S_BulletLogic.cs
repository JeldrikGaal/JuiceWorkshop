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
    
    #region Unity Events
    void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();

        _rigidbody2D.velocity = new Vector2(0, _bulletSpeed);
        _visuals.transform.localScale = new Vector3(Random.Range(_bulletSizeRange.x, _bulletSizeRange.y), Random.Range(_bulletSizeRange.x, _bulletSizeRange.y), 1);
        Destroy(gameObject, _bulletLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            if (Vector3.Distance(transform.position, S_PlayerController.Instance.transform.position) < _maxDamageRange)
            {
                other.GetComponent<S_ObstacleLogic>().TakeDamage(_bulletDamage);
                S_CameraShake.Instance.Shake(0.1f, 2);
            }
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    #endregion
}
