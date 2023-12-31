using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

// This class is the ObstacleLogic. It manages the obstacle health and score.
public class S_ObstacleLogic : MonoBehaviour
{
    public static event Action<GameObject> PlayerHitObstacle;
    public static event Action<GameObject> ObstacleDestroyedByPlayer;

    [SerializeField] private float _obstacleHealth;
    private float _startingHealth;
    [SerializeField] private float _obstacleScore;

    [FormerlySerializedAs("_juiceToggles")]
    [Tooltip("0: shrinking on damage")]
    [SerializeField] public List<bool> _juiceToggle;
    
    #region Unity Events

    private void Awake()
    {
        _juiceToggle[0] = S_JuiceManager.Instance._obstacleJuice;
    }

    void Start()
    {
        _startingHealth = _obstacleHealth;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHitObstacle?.Invoke(gameObject);
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
    public void TakeDamage(float damage)
    {
        _obstacleHealth -= damage;
       
        if (S_JuiceManager.Instance._obstacleJuice)
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

    #region JuiceManagement
    private void ToggleAllJuice(bool on)
    {
        _juiceToggle = S_JuiceManager.GetBoolList(on, _juiceToggle.Count);
    }
    
    private void ToggleJuice(Component Script, List<bool> toggles)
    {
        if (this != Script) return;
        _juiceToggle[0] = S_JuiceManager.Instance._obstacleJuice;
    }    
   #endregion
}
