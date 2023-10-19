using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_GroundManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _groundSegments;
    [SerializeField] private GameObject _baseGroundTile;
    [SerializeField] private int _segmentStartAmount;
    
    public static S_GroundManager Instance;
    public Vector2 SegmentSize = new Vector2(7, 1.5f);
    
    private List<GameObject> _spawnedSegments = new List<GameObject>();
    
    #region Unity Events
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one GroundManager in the scene!");
            Destroy(this);
        }
    }

    void Start()
    {
        for (int i = 0; i < _segmentStartAmount; i++)
        {
            SpawnTile();
        }

        SegmentSize = new Vector2(_baseGroundTile.GetComponentInChildren<Rectangle>().Width, _baseGroundTile.GetComponentInChildren<Rectangle>().Height);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SpawnTile();
        }

        if (_spawnedSegments[2].transform.position.y <= S_PlayerController.Instance.transform.position.y)
        {
            SpawnTile();
        }
    }
    #endregion
    
    private void SpawnTile()
    {
        if (_spawnedSegments.Count > 0)
        {
            _spawnedSegments.Add(Instantiate(_groundSegments[Random.Range(0,_groundSegments.Count)]));
            _spawnedSegments[^1].transform.position = new Vector3(_spawnedSegments[^2].transform.position.x,
                _spawnedSegments[^2].transform.position.y + SegmentSize.x, _spawnedSegments[^2].transform.position.z);
        }
        else
        {
            _spawnedSegments.Add(Instantiate(_groundSegments[0]));
            _spawnedSegments[^1].GetComponent<S_GroundSegmentLogic>().SetObstacleProbability(0);
        }

        if (_spawnedSegments.Count > 5)
        {
            RemoveTile();
        }
    }
    
    private void RemoveTile()
    {
        Destroy(_spawnedSegments[0]);
        _spawnedSegments.RemoveAt(0);
    }
}