using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class S_GameManager : MonoBehaviour
{

    public static S_GameManager Instance;
    private float _score;

    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] public List<bool> _juiceToggle;
    
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
        S_JuiceManager.ToggleAllJuice += ToggleAllJuice;
    }

    private void OnDisable()
    {
        S_ObstacleLogic.PlayerHitObstacle -= PlayerCollision;
        S_ObstacleLogic.ObstacleDestroyedByPlayer -= GetScoreFromObstacle;
        S_JuiceManager.ToggleAllJuice -= ToggleAllJuice;
    }

    void Start()
    {
        
    }
    void Update()
    {
        
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
        if (_juiceToggle[0])
        {
            ScoreTextAnim();
        }
    }

    private void ScoreTextAnim()
    {
        _scoreText.color = Color.green;
        _scoreText.transform.DOPunchScale(new Vector3(1, 1.5f, 1), 0.2f, 0, 0).OnComplete(() =>
        {
            _scoreText.color = Color.white;
        });
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
    
    private void ToggleAllJuice(bool on)
    {
        _juiceToggle = S_JuiceManager.GetBoolList(on, _juiceToggle.Count);
    }
    private void ToggleJuice(List<bool> toggles)
    {
        _juiceToggle = toggles;
    }
}
