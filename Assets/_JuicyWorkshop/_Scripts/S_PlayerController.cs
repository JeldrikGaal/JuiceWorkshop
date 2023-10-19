using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class S_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _visuals;
    [SerializeField] private float _movespeed;
    [SerializeField] public List<bool> _juiceToggle;
    
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
        
    }
    void Update()
    {
        
        if (Input.GetKey(KeyCode.W))
        {
            Move(new Vector3(0, _movespeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(new Vector3(0, -_movespeed * Time.deltaTime, 0));
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(new Vector3(-_movespeed * Time.deltaTime, 0, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(new Vector3(_movespeed * Time.deltaTime, 0, 0));
        }
        
        
    }

    private void Move(Vector3 dif)
    {
        Vector3 newPos = new Vector3(transform.position.x + dif.x, transform.position.y + dif.y, transform.position.z + dif.z);
        if (newPos.y < S_GroundManager.Instance.SegmentSize.y || newPos.y > -S_GroundManager.Instance.SegmentSize.y)
        {
            transform.position = newPos;
        }
    }
    #endregion
}
