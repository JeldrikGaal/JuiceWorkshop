using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// This Script manages the singular ground tiles and spawns obstacles on them.
public class S_GroundSegmentLogic : MonoBehaviour
{
    [SerializeField] private List<GameObject> _groundPrefabs;
    [SerializeField] private float _tileAmount = 3f;
    [SerializeField] public float _obstacleProbability = 0.8f;
    [SerializeField] private List<GameObject> _obstaclePrefabs;
    
    private List<GameObject> _groundTiles = new List<GameObject>();
    public List<GameObject> SpawnedObstacles {get; private set; } = new List<GameObject>();

    #region Unity Events
    void Start()
    {
        SpawnGroundTiles(_tileAmount);
    }
    
    void Update()
    {
        
    }

    private void OnEnable()
    {
        S_ObstacleLogic.PlayerHitObstacle += RemoveObstacleFromTile;
        S_ObstacleLogic.ObstacleDestroyedByPlayer += RemoveObstacleFromTile;
    }

    private void OnDisable()
    {
        S_ObstacleLogic.PlayerHitObstacle -= RemoveObstacleFromTile;
        S_ObstacleLogic.ObstacleDestroyedByPlayer -= RemoveObstacleFromTile;
    }

    #endregion
    
    #region Tile & Obstacle Management
    private void SpawnGroundTiles(float amount)
    {
        int count = 0;
        for (int i = 0; i < _tileAmount; i++ )
        {
            Vector3 pos;
            
            if (i % 2 == 0)
            { 
                pos = new Vector3(0,count * 1.5f,0);
            }
            else
            {
                count += 1;
                pos = new Vector3(0,-count * 1.5f,0);
            }

            _groundTiles.Add(Instantiate(_groundPrefabs[Random.Range(0, _groundPrefabs.Count)], transform));
            _groundTiles[^1].transform.localPosition = pos;
            
            AddObstacleToTile(_groundTiles[^1].transform);
        }
    }

    private void AddObstacleToTile(Transform parent)
    {
        if (Random.Range(0f, 1f) < _obstacleProbability)
        {
            SpawnedObstacles.Add(Instantiate(_obstaclePrefabs[Random.Range(0, _obstaclePrefabs.Count)]));
            SpawnedObstacles[^1].transform.parent = parent;
            SpawnedObstacles[^1].transform.localPosition = Vector3.zero;
        }
    }

    private void RemoveObstacleFromTile(GameObject obstacle)
    {
        SpawnedObstacles.Remove(obstacle);
        Destroy(obstacle);
    }

    public void ClearObstacles()
    {
        foreach (GameObject obstacle in SpawnedObstacles)
        {
            Destroy(obstacle);
        }
        SpawnedObstacles = new List<GameObject>();
    }

    public void SetObstacleProbability(float newProbability)
    {
        _obstacleProbability = newProbability;
    }
    #endregion
}
