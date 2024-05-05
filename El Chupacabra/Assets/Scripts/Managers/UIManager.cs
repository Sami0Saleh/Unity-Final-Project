using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerScoreText;
    [SerializeField] TextMeshProUGUI playerHPText;

    public void UpdateHP(int newMaxHP , int newCurrentHP)
    {
        playerHPText.text = newCurrentHP.ToString() + " / " + newMaxHP.ToString();
    }
    public void UpdateScore(int newScore)
    {
        playerScoreText.text = newScore.ToString();
    }

}
