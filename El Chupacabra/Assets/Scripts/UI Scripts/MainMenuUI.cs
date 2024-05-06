using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject SceneLoader;
    [SerializeField] GameObject _load;
    [SerializeField] GameObject _options;
    [SerializeField] GameObject _credits;
    public void NewGameButton()
    {
        MainMenu.SetActive(false);
        SceneLoader.SetActive(true);
        PlayerPrefs.DeleteAll();
    }
    public void LoadButton()
    {
        gameObject.SetActive(false);
        _load.SetActive(true);
    }
    public void OptionsButton()
    {
        gameObject.SetActive(false);
        _options.SetActive(true);
    }
    public void CreditsButton()
    {
        gameObject.SetActive(false);
        _credits.SetActive(true);
    }
    public void QuitGameButton()
    {
        Application.Quit();
    }
}
