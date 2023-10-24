using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;
using Random = UnityEngine.Random;

// This Script manages the spawning, moving and removing of ground segments.
public class S_GroundManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> _groundSegments;
    [SerializeField] private GameObject _baseGroundTile;
    [SerializeField] private int _segmentStartAmount;
    
    public static S_GroundManager Instance;
    public Vector2 SegmentSize = new Vector2(7, 1.5f);

    public List<GameObject> SpawnedSegments {get; private set;} = new List<GameObject>(); 
    
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

        if (SpawnedSegments[2].transform.position.y <= S_PlayerController.Instance.transform.position.y)
        {
            SpawnTile();
        }
    }
    #endregion
    
    #region Tile Management
    private void SpawnTile()
    {
        if (SpawnedSegments.Count > 0)
        {
            SpawnedSegments.Add(Instantiate(_groundSegments[Random.Range(0,_groundSegments.Count)]));
            SpawnedSegments[^1].transform.position = new Vector3(SpawnedSegments[^2].transform.position.x,
                SpawnedSegments[^2].transform.position.y + SegmentSize.x, SpawnedSegments[^2].transform.position.z);
        }
        else
        {
            SpawnedSegments.Add(Instantiate(_groundSegments[0]));
            SpawnedSegments[^1].GetComponent<S_GroundSegmentLogic>().SetObstacleProbability(0);
        }

        if (SpawnedSegments.Count > 5)
        {
            RemoveTile();
        }
    }
    
    private void RemoveTile()
    {
        Destroy(SpawnedSegments[0]);
        SpawnedSegments.RemoveAt(0);
    }
    #endregion
}