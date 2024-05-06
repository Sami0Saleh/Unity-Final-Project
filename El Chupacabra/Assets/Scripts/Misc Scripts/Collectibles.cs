using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Collectibles : MonoBehaviour
{
    [SerializeField] NewPlayerController _playerController;
    [SerializeField] Transform _playerTransform;
    [SerializeField] GameObject _collectEffect;

    [SerializeField] LayerMask _playerLayer;

    [SerializeField] float _rotationSpeed;
    [SerializeField] float _speed;
    [SerializeField] float _collectRange;

    private bool _playerInCollectRange = false;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    void Start()
    {
        _playerController.MaxScore ++;
    }
    void Update()
    {
        Movement();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Collect();
        }
    }

    public void Movement()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime, Space.World);
        _playerInCollectRange = Physics.CheckSphere(transform.position, _collectRange, _playerLayer);
        if (_playerInCollectRange)
        {
            transform.LookAt(_playerTransform);
            transform.Translate(Vector3.forward * _speed * Time.deltaTime);
        }
    }
    public void Collect()
    {
        if (_collectEffect)
            Instantiate(_collectEffect, transform.position, Quaternion.identity);
        _playerController.Score ++;
        _playerController.UpdateScore();
        Destroy(gameObject);
    }
    private void OnGameStateChanged(GameState newGameState)
    {
        enabled = newGameState == GameState.GamePlay;
        if (newGameState == GameState.GamePlay)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }
    }
}
