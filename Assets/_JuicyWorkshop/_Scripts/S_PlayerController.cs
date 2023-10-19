using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class S_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _visuals;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Vector2 _movespeed;
    [SerializeField] private float _shootDelay;
    
    [Tooltip("0: stretching on movement")]
    [SerializeField] public List<bool> _juiceToggle;

    private float _lastShotTime;
    public static S_PlayerController Instance;
    
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
        _lastShotTime = Time.time;
    }
    void Update()
    {

        _visuals.transform.localScale = Vector3.one;
        
        // Force player to move upwards to create endless runner
        Move(new Vector3(0, _movespeed.y * Time.deltaTime, 0));
        
        // Let player move left and right
        if (Input.GetKey(KeyCode.A))
        {
            Move(new Vector3(-_movespeed.x * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(new Vector3(_movespeed.x * Time.deltaTime, 0, 0));
        }
        
        // Make player shoot automatically
        if (Time.time - _lastShotTime > _shootDelay)
        {
            Shoot();
        }
    }

    private void Move(Vector3 dif)
    {
        Vector3 newPos = new Vector3(transform.position.x + dif.x, transform.position.y + dif.y, transform.position.z + dif.z);
        if (newPos.x < S_GroundManager.Instance.SegmentSize.y * 1.5 &&
            newPos.x > -S_GroundManager.Instance.SegmentSize.y * 1.5)
        {
            transform.position = newPos;

            // If the juice option for stretching the character is on and it is moving left or right stretch the visuals
            if (_juiceToggle[0] && dif.x != 0)
            {
                _visuals.transform.localScale = new Vector3(1.25f, 0.75f);
            }
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(_bulletPrefab);
        bullet.transform.position = transform.position;
        _lastShotTime = Time.time;
    }
    #endregion
}
