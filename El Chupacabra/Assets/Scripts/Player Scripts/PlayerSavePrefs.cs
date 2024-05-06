using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerSavePrefs : MonoBehaviour
{
    [SerializeField] NewPlayerController _playerController;
    [SerializeField] Transform _playerTransform;

    public void SetPlayerPrefs()
    {
        PlayerPrefs.SetInt("MaxScore", _playerController.MaxScore);
        PlayerPrefs.SetInt("ScoreCount", _playerController.Score);
        PlayerPrefs.SetInt("MaxHP", _playerController.MaxHp);
        PlayerPrefs.SetInt("CurrentHP", _playerController.CurrentHp);
        PlayerPrefs.SetInt("MaxEnemyCount", _playerController.MaxEnemyCount);
        PlayerPrefs.SetInt("EnemyCount", _playerController.EnemyCount);
    }
}
