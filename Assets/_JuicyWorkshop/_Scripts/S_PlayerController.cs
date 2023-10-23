using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class S_PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject _visuals;
    [SerializeField] private GameObject _leftEye;
    [SerializeField] private GameObject _leftEyeBall;
    [SerializeField] private GameObject _rightEye;
    [SerializeField] private GameObject _rightEyeBall;
    [SerializeField] private GameObject _mouth;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Vector2 _movespeed;
    [SerializeField] private float _shootDelay;
    
    [Tooltip("0: stretching on movement, 1: face,  2: blink anim")]
    [SerializeField] public List<bool> _juiceToggle;

    private float _lastShotTime;
    public static S_PlayerController Instance;

    private float _blinkAnimStart;
    private Vector2 _blinkAnimRandomRange = new Vector2(3,7);
    private float _currentWaitTimeBlinkAnim;
    
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
        _blinkAnimStart = Time.time;
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

        if (Input.touchCount > 0)
        {
            if(Camera.main.ScreenToWorldPoint(Input.touches[0].position).x > transform.position.x)
            {
                Move(new Vector3(_movespeed.x * Time.deltaTime, 0, 0));
            }
            else
            {
                Move(new Vector3(-_movespeed.x * Time.deltaTime, 0, 0));
            }
        }
        
        // Make player shoot automatically
        if (Time.time - _lastShotTime > _shootDelay)
        {
            Shoot();
        }
        
        // Make Eyes blink
        if (_juiceToggle[2])
        {
            if (Time.time - _blinkAnimStart > _currentWaitTimeBlinkAnim)
            {
                StartCoroutine(BlinkAnim());
            }
        }
    }
    
    private void OnEnable()
    {
        S_JuiceManager.ToggleAllJuice += ToggleAllJuice;
        S_JuiceManager.ToggleSpecificJuice -= ToggleJuice;
    }
    
    private void OnDisable()
    {
        S_JuiceManager.ToggleAllJuice -= ToggleAllJuice;
        S_JuiceManager.ToggleSpecificJuice -= ToggleJuice;
    }
    #endregion
    
    private void Move(Vector3 dif)
    {
        Vector3 newPos = new Vector3(transform.position.x + dif.x, transform.position.y + dif.y, transform.position.z + dif.z);
        if (newPos.x < S_GroundManager.Instance.SegmentSize.y * 1.25f &&
            newPos.x > -S_GroundManager.Instance.SegmentSize.y * 1.25f)
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
    
    private void ToggleAllJuice(bool on)
    {
        _juiceToggle = S_JuiceManager.GetBoolList(on, _juiceToggle.Count);
        ToggleFace(on);
    }
    
    private void ToggleJuice(Component script, List<bool> toggles)
    {
        if (this != script) return;
        _juiceToggle = toggles;
        ToggleFace(toggles[1]);
    }

    private void ToggleFace(bool toggle)
    {
        _leftEye.SetActive(toggle);
        _rightEye.SetActive(toggle);
        _mouth.SetActive(toggle);
    }

    private IEnumerator BlinkAnim()
    {
        _currentWaitTimeBlinkAnim = Random.Range(_blinkAnimRandomRange.x, _blinkAnimRandomRange.y);
        _blinkAnimStart = Time.time;
        
        _leftEye.SetActive(false);
        _rightEye.SetActive(false);
        yield return new WaitForSeconds(0.05f);
        _leftEye.SetActive(true);
        _rightEye.SetActive(true);
        
    }
}
