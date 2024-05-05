using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseUI : MonoBehaviour
{
    [SerializeField] GameObject _options;
    [SerializeField] GameObject _save;
    [SerializeField] GameObject _load;
    [SerializeField] GameObject _mainMenu;

    public void PauseGame()
    {
        gameObject.SetActive(true);
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.GamePlay
            ? GameState.Pause
            : GameState.GamePlay;
        GameStateManager.Instance.SetState(newGameState);
        AudioListener.pause = true;
    }
    public void ContinueButton()
    {
        gameObject.SetActive(false);
        GameState currentGameState = GameStateManager.Instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.GamePlay
            ? GameState.Pause
            : GameState.GamePlay;
        GameStateManager.Instance.SetState(newGameState);
        AudioListener.pause = false;
    }
    public void OptionsButton()
    {
        gameObject.SetActive(false);
        _options.SetActive(true);
    }
    public void SaveButton()
    {
        gameObject.SetActive(false);
        _save.SetActive(true);
    }
    public void LoadButton()
    {
        gameObject.SetActive(false);
        _load.SetActive(true);
    }
    public void MainMenuButton()
    {
        gameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
