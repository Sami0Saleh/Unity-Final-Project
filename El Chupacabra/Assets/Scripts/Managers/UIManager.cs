using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _playerHPText;
    [SerializeField] TextMeshProUGUI _playerScoreText;
    [SerializeField] TextMeshProUGUI _EnemyCountText;
    public void UpdateHP(int newMaxHP , int newCurrentHP)
    {
        _playerHPText.text = newCurrentHP.ToString() + " / " + newMaxHP.ToString();
    }
    public void UpdateScore(int newMaxScore , int newScore)
    {
        _playerScoreText.text = newScore.ToString() + " / " + newMaxScore.ToString();
    }
    public void UpdateEnemy(int newMaxEnemy, int newEnemy)
    {
        _EnemyCountText.text = newEnemy.ToString() + " / " + newMaxEnemy.ToString();
    }
}
