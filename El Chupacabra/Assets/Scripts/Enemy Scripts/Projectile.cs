using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float _speed;
    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
    }
    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }
    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            NewPlayerController newPlayerController = other.GetComponent<NewPlayerController>();
            if (newPlayerController != null)
            {
                newPlayerController.TakeDamage();
            }
        }
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
