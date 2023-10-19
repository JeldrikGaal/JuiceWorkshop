using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_GroundSegmentLogic : MonoBehaviour
{
    [SerializeField] private List<GameObject> _groundPrefabs;
    [SerializeField] private float _tileAmount = 3f;
    
    private List<GameObject> _groundTiles = new List<GameObject>();

    #region Unity Events
    void Start()
    {
        SpawnGroundTiles(_tileAmount);
    }
    
    void Update()
    {
        
    }

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

            _groundTiles.Add(Instantiate(_groundPrefabs[Random.Range(0, _groundPrefabs.Count)], Vector3.zero, Quaternion.identity, transform));
            _groundTiles[^1].transform.localPosition = pos;
        }
    }
    #endregion
}
