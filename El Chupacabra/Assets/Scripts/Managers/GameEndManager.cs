using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndManager : MonoBehaviour
{
    public void GameWon()
    {
        gameObject.SetActive(true);
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.GamePlay
            ? GameState.Pause
            : GameState.GamePlay;
        GameStateManager.Instance.SetState(GameState.Pause);
        AudioListener.pause = true;

        SceneManager.LoadScene(3);
    }
    public void PlayerDead()
    {
        gameObject.SetActive(true);
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.GamePlay
            ? GameState.Pause
            : GameState.GamePlay;
        GameStateManager.Instance.SetState(GameState.Pause);
        AudioListener.pause = true;

        SceneManager.LoadScene(4);
    }
}
