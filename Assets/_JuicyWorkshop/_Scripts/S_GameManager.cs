using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GameManager : MonoBehaviour
{

    public static S_GameManager Instance;
    private float _score;

    [SerializeField] private TMP_Text _scoreText;
    
    #region Unity Events

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one GameManager in the scene!");
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        S_ObstacleLogic.PlayerHitObstacle += PlayerCollision;
        S_ObstacleLogic.ObstacleDestroyedByPlayer += GetScoreFromObstacle;
    }

    private void OnDisable()
    {
        S_ObstacleLogic.PlayerHitObstacle -= PlayerCollision;
        S_ObstacleLogic.ObstacleDestroyedByPlayer -= GetScoreFromObstacle;
    }

    void Start()
    {
        
    }
    void Update()
    {
        //AddScore(Time.deltaTime);
    }
    #endregion
    
    #region Gameflow

    private void Loose()
    {
        Debug.Log("You loose!");
        Restart();
    }

    private void Win()
    {
        
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void AddScore(float amount)
    {
        _score += amount;
        _score = (float)Math.Round(_score, 2);
        UpdateScoreText();
    }
    
    private void UpdateScoreText()
    {
        _scoreText.text = Mathf.Round(_score).ToString();
    }

    private void GetScoreFromObstacle(GameObject obstacle)
    {
        AddScore(obstacle.GetComponent<S_ObstacleLogic>().GetScore());
    }
    #endregion
    
    #region PlayerInteractions
    private void PlayerCollision( GameObject other)
    {
        Loose();
    }
    #endregion
}
