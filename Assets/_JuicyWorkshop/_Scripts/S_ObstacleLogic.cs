using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class S_ObstacleLogic : MonoBehaviour
{
    public static event Action<GameObject> PlayerHitObstacle;
    public static event Action<GameObject> ObstacleDestroyedByPlayer;

    [SerializeField] private float _obstacleHealth;
    private float _startingHealth;
    [SerializeField] private float _obstacleScore;

    [Tooltip("0: shrinking on damage")]
    [SerializeField] public List<bool> _juiceToggles;
    
    #region Unity Events
    void Start()
    {
        _startingHealth = _obstacleHealth;
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHitObstacle?.Invoke(gameObject);
        }
    }
    #endregion
    public void TakeDamage(float damage)
    {
        _obstacleHealth -= damage;
       
        if (_juiceToggles[0])
        {
            float percentage = _obstacleHealth / _startingHealth;
            transform.DOScale(new Vector3(percentage, percentage, percentage), 0.1f);
        }
        
        if (_obstacleHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        ObstacleDestroyedByPlayer?.Invoke(gameObject);
    }

    public float GetScore()
    {
        return _obstacleScore;
    }
}
