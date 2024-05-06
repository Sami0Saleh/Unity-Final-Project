using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerHPText;
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI EnemyCountText;

    public void UpdateHP(int newMaxHP , int newCurrentHP)
    {
        playerHPText.text = newCurrentHP.ToString() + " / " + newMaxHP.ToString();
    }
    public void UpdateScore(int newMaxScore , int newScore)
    {
        playerScoreText.text = newScore.ToString() + " / " + newMaxScore.ToString();
    }
    public void UpdateEnemy(int newMaxEnemy, int newEnemy)
    {
        EnemyCountText.text = newEnemy.ToString() + " / " + newMaxEnemy.ToString();
    }
}
